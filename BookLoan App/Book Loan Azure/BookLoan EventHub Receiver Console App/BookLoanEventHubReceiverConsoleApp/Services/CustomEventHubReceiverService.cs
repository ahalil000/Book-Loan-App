using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BookLoanEventHubReceiverConsoleApp
{
    public class CustomEventHubReceiverService : ICustomEventHubReceiverService
    {
        private readonly ILogger _logger;
        private readonly string _eventHubName;
        private readonly string _eventHubConnectionString;

        private readonly string _blobContainerName;
        private readonly string _storageConnectionString;


        public CustomEventHubReceiverService(
            IConfiguration configuration, 
            ILogger<CustomEventHubReceiverService> logger)
        {
            this._logger = logger;

            try
            {
                _eventHubName = configuration.GetSection("AppSettings").GetValue<string>("EventHubName");
            }
            catch (Exception ex)
            {
                _eventHubName = "";
            }

            try
            {
                _eventHubConnectionString = configuration.GetSection("AppSettings").GetValue<string>("EventHubConnection");
            }
            catch (Exception ex)
            {
                _eventHubConnectionString = "";
            }

            try
            {
                _blobContainerName = configuration.GetSection("AppSettings").GetValue<string>("BlobContainerName");
            }
            catch (Exception ex)
            {
                _blobContainerName = "";
            }

            try
            {
                _storageConnectionString = configuration.GetSection("AppSettings").GetValue<string>("BlobStorageConnectionString");
            }
            catch (Exception ex)
            {
                _storageConnectionString = "";
            }

        }

        public async Task ProcessEventHubEvents()
        {
            // Read from the default consumer group: $Default
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Create a blob container client that the event processor will use 
            BlobContainerClient storageClient = new BlobContainerClient(_storageConnectionString, _blobContainerName);

            // Create an event processor client to process events in the event hub
            EventProcessorClient processor = new EventProcessorClient(storageClient, consumerGroup, _eventHubConnectionString, _eventHubName);

            // Register handlers for processing events and handling errors
            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            // Start the processing
            await processor.StartProcessingAsync();

            // Wait for 30 seconds for the events to be processed
            await Task.Delay(TimeSpan.FromSeconds(30));

            // Stop the processing
            await processor.StopProcessingAsync();
        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            // Write the body of the event to the console window
            Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}