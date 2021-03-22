using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using BookLoan.Catalog.API.Events;
using Microsoft.Azure.ServiceBus;
using BookLoanMicroservices.Messaging;


namespace BookLoan.Catalog.API.Events
{
    public class AzureEventBus : IEventBus
    {
        private readonly IMessageBusHelper _messageBusHelper;
        public Func<Message, CancellationToken, Task> ReceiveMessageHandler;

        public AzureEventBus(IMessageBusHelper messageBusHelper) 
        {
            _messageBusHelper = messageBusHelper;
        }

        public async Task Publish(IntegrationEvent @event)
        {
            await _messageBusHelper.InitSendMessages(@event);
        }

        public void Subscribe(IntegrationEvent @event, IntegrationEventHandler handler)
        {
            _messageBusHelper.InitReceiveMessages();
            _messageBusHelper.MessageReceivedHandler += handler.integrationEventHandler();
        }

        public async Task Unsubscribe(IntegrationEvent @event, IntegrationEventHandler handler)
        {
            if (!_messageBusHelper.queueClient.IsClosedOrClosing)
            {
                _messageBusHelper.MessageReceivedHandler -= handler.integrationEventHandler();
                await _messageBusHelper.queueClient.CloseAsync();
            }
        }
    }
}
