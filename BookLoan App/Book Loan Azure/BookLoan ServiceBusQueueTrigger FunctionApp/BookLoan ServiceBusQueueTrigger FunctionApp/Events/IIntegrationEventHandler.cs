using System;
using Newtonsoft.Json;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;


/// <summary>
/// From eShop Containers Event definition
/// </summary>
namespace BookLoan.Catalog.API.Events
{
    //public delegate ReceiveMessageHandler2 Func<Message, CancellationToken, Task>();
    //public Func<Message, CancellationToken, Task> ReceiveMessageHandler;

    public interface IIntegrationEventHandler
    {
        ReceiveMessageHandler integrationEventHandler { get; set; }   
    }

    //public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    //    where TIntegrationEvent : IntegrationEvent
    //{
    //    Task Handle(TIntegrationEvent @event);
    //}

    //public interface IntegrationMessageHandler
    //{
    //}

}
