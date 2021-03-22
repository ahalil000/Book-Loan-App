using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;
using Newtonsoft.Json;
using System.Text;

//using Microsoft.Azure.WebJobs.ServiceBus;
using BookLoan.Catalog.API.Events;
using BookLoanMicroservices.Helpers;


namespace BookLoanServiceBusQueueTriggerFunctionApp
{
    public static class ServiceBusQueueTriggerFunction
    {

        [FunctionName("ServiceBusQueueTriggerFunction")]
        public static void Run([ServiceBusTrigger("bookloanasbqueue",
            Connection = "AzureWebJobsServiceBusQueue")]Message myQueueItem,
            ILogger log)
        {
            // Deserialize the body of the message..
            var body = Encoding.UTF8.GetString(myQueueItem.Body);
            BookInventoryChangedIntegrationEvent bookInventoryChangedIntegrationEvent =
                JsonConvert.DeserializeObject<BookInventoryChangedIntegrationEvent>(body);
 
            log.LogInformation($"C# ServiceBus queue trigger function processed message: { bookInventoryChangedIntegrationEvent.BookId}");
            log.LogInformation($"C# ServiceBus queue trigger function processed message old edition: {bookInventoryChangedIntegrationEvent.OldEdition}");
            log.LogInformation($"C# ServiceBus queue trigger function processed message new edition: {bookInventoryChangedIntegrationEvent.NewEdition}");
        }



        //[FunctionName("ServiceBusQueueTriggerFunction")]
        //public static void Run([ServiceBusTrigger("bookloanasbqueue",
        //    Connection = "AzureWebJobsServiceBusQueue")]Message myQueueItem,
        //    ILogger log)
        //{
        //    // Deserialize the body of the message..
        //    BookInventoryChangedIntegrationEvent bookInventoryChangedIntegrationEvent = 
        //        SerializationHelpers.Deserialize<BookInventoryChangedIntegrationEvent>(myQueueItem.Body);

        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: { bookInventoryChangedIntegrationEvent.BookId}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message old edition: {bookInventoryChangedIntegrationEvent.OldEdition}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message new edition: {bookInventoryChangedIntegrationEvent.NewEdition}");
        //}


        //[FunctionName("ServiceBusQueueTriggerFunction")]
        //public static void Run([ServiceBusTrigger("bookloanasbqueue", 
        //    Connection = "AzureWebJobsServiceBusQueue")]Message myQueueItem, 
        //    ILogger log)
        //{
        //    // Deserialize the body of the message..
        //    BookInventoryChangedIntegrationEvent bookInventoryChangedIntegrationEvent =
        //        myQueueItem.GetBody<BookInventoryChangedIntegrationEvent>();
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: { bookInventoryChangedIntegrationEvent.BookId}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message old edition: {bookInventoryChangedIntegrationEvent.OldEdition}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message new edition: {bookInventoryChangedIntegrationEvent.NewEdition}");
        //}

        //[FunctionName("ServiceBusQueueTriggerFunction")]
        //public static void Run([ServiceBusTrigger("bookloanasbqueue",
        //    Connection = "AzureWebJobsServiceBusQueue")]BookInventoryChangedIntegrationEvent myQueueItem,
        //    ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem.BookId}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message old edition: {myQueueItem.OldEdition}");
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message new edition: {myQueueItem.NewEdition}");
        //}

        //[FunctionName("ServiceBusQueueTriggerFunction")]
        //public static void Run([ServiceBusTrigger("bookloanasbqueue",
        //    Connection = "AzureWebJobsServiceBusQueue")]string myQueueItem,
        //    ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        //}


    }
}
