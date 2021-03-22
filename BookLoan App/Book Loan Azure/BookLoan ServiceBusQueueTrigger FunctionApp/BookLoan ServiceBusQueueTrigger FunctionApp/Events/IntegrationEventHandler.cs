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
    public delegate Func<Message, CancellationToken, Task> ReceiveMessageHandler();

    [Serializable]
    public class IntegrationEventHandler: IIntegrationEventHandler
    {
        public ReceiveMessageHandler integrationEventHandler { get; set; }
    }
}
