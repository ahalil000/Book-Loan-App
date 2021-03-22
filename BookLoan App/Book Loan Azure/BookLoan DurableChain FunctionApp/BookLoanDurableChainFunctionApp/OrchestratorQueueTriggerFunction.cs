using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using BookLoan.Models;


namespace BookLoanDurableChainFunctionApp
{
    public static class OrchestratorQueueTriggerFunction
    {
        [FunctionName("OrchestratorQueueTriggerFunction")]
        public static void Run(
            [QueueTrigger("bookreviewqueue-items", 
            Connection = "AZURE_STORAGE_CONNECTION_STRING_DEV")]string myQueueItem, 
            [DurableClient] IDurableOrchestrationClient starter, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function starting: {myQueueItem}");

            // retrieve parameters from message and use as inputs to orchestrations..
            var receivedObject = JsonConvert.DeserializeObject<ReviewViewModel>(myQueueItem);

            starter.StartNewAsync("OrchestratorChainFirstFunction", receivedObject);
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
