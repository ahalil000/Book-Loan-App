using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Text;
using Microsoft.Extensions.Options;

//using System.Net.Http;
//using Newtonsoft.Json; // JsonConvert
//using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue, AuthenticationHeaderValue etc
using Newtonsoft.Json;
//using System.Text.Json;

using BookLoanMicroservices.Helpers;
using BookLoan.Catalog.API.Events;

//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json.Serialization;

using BookLoanMicroservices.Messaging;
using BookLoanMicroservices.Models.MessagingModels;
using BookLoan.Services;
using BookLoan.Catalog.API.Messaging;


namespace BookLoanMicroservices.Messaging
{
    public class MessageBusQueueHelper: IMessageBusHelper
    {
        //const string QueueName = "bookloanasbqueue";

        private IQueueClient _queueClient;

        public bool _messageSent;
        public bool _messageReceived;

        public ManualResetEvent MessageSentEvent;
        private readonly AppConfiguration _appConfiguration;

        private MessageBusFormat _messageBusFormat;

        //public ManualResetEvent MessageReceivedEvent;

        //public delegate Task ReceiveMessageHandler2(Message message, CancellationToken cancellation);

        private Func<Message, CancellationToken, Task> _MessageReceivedHandler;


        /// <summary>
        /// 
        /// </summary>
        public MessageBusQueueHelper(IOptionsSnapshot<AppConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration.Value;
           _messageReceived = false;
            _messageSent = false;
            _messageBusFormat = MessageBusFormat.JSON;
            //MessageSentEvent = new ManualResetEvent(false);
        }


        /// <summary>
        /// 
        /// </summary>
        Func<Message, CancellationToken, Task> IMessageBusHelper.MessageReceivedHandler
        {
            get { return _MessageReceivedHandler; }
            set { _MessageReceivedHandler = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        IQueueClient IMessageBusHelper.queueClient
        {
            get { return _queueClient;  } 
            set { _queueClient = value; } 
        }

        /// <summary>
        /// 
        /// </summary>
        MessageBusFormat IMessageBusHelper.messageBusFormat
        {
            get { return _messageBusFormat; }
            set { _messageBusFormat = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMessageReceived 
        { 
            get { return _messageReceived; }
            set { _messageReceived = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMessageSent
        {
            get { return _messageSent; }
            set { _messageSent = value; }
        }


        // Send messages

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task InitSendMessages(object message)
        {
            IsMessageReceived = false;
            IsMessageSent = false;
            //MessageSentEvent.Reset();

            const int numberOfMessages = 1;
            //queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            _queueClient = new QueueClient(_appConfiguration.ServiceBusConnection, _appConfiguration.ServiceBusQueue);

            // Send messages.
            await SendMessagesAsync(message, numberOfMessages);

            await _queueClient.CloseAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfMessagesToSend"></param>
        /// <returns></returns>
        public async Task SendMessagesAsync(object newmessage, int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    if (_messageBusFormat == MessageBusFormat.Binary)
                    {
                        // Create a new message to send to the queue
                        byte[] messagebytes = SerializationHelpers.SerializeToByteArray(newmessage);

                        var message = new Message(messagebytes);

                        await _queueClient.SendAsync(message);

                        IsMessageSent = true;
                    }
                    else
                    if (_messageBusFormat == MessageBusFormat.JSON)
                    {
                        var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newmessage)));
                        message.ContentType = "text/plain";

                        await _queueClient.SendAsync(message);

                        IsMessageSent = true;
                    }

                    //string messageBody = "";

                    //$"Message {i}";
                    //var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    //Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue
                    // await _queueClient.SendAsync(message);
                }

                //MessageSentEvent.Set();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }



        // Receive Messages

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void InitReceiveMessages()
        {
            //MessageReceivedEvent.Reset();

            _queueClient = new QueueClient(_appConfiguration.ServiceBusConnection, _appConfiguration.ServiceBusQueue);

            RegisterOnMessageHandlerAndReceiveMessages();

            //await queueClient.CloseAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            _queueClient.RegisterMessageHandler(_MessageReceivedHandler, messageHandlerOptions);
            //queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        //public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        //{

        //    // Process the message
        //    Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");


        //    BookRecordSyncModel bookRecordSyncModel = SerializationHelpers.Deserialize<BookRecordSyncModel>(message.Body);

        //    if (bookRecordSyncModel.CommandType == "Update")
        //    {
        //        // run some task that inserts the book data into another data store.


        //    }

        //    // Complete the message so that it is not received again.
        //    // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
        //    await queueClient.CompleteAsync(message.SystemProperties.LockToken);

        //    IsMessageReceived = true;

        //    //MessageReceivedEvent.Set();

        //    // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
        //    // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
        //    // to avoid unnecessary exceptions.
        //}


        public Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

    }
}
