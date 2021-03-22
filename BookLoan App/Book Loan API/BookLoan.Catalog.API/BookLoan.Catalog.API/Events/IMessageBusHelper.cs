using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using Microsoft.Azure.ServiceBus;
using BookLoan.Catalog.API.Messaging;


namespace BookLoan.Catalog.API.Events
{
    public interface IMessageBusHelper
    {
        Func<Message, CancellationToken, Task> MessageReceivedHandler { get; set; }
        IQueueClient queueClient { get; set; }
        MessageBusFormat messageBusFormat { get; set; }

        bool IsMessageReceived { get; set; }
        bool IsMessageSent { get; set; }
        Task InitSendMessages(object message);
        Task SendMessagesAsync(object newmessage, int numberOfMessagesToSend);
        void InitReceiveMessages();
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs);
    }
}
