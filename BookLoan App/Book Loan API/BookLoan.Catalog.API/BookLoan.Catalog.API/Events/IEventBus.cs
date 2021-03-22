using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Catalog.API.Events
{
    public interface IEventBus
    {
        Task Publish(IntegrationEvent @event);

        void Subscribe(IntegrationEvent @event, 
            IntegrationEventHandler handle);

        Task Unsubscribe(IntegrationEvent @event,
            IntegrationEventHandler handle);
    }
}
