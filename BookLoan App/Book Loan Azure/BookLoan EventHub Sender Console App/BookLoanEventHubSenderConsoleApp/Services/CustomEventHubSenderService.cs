using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;

namespace BookLoanEventHubSenderConsoleApp
{
    public class CustomEventHubSenderService: ICustomEventHubSenderService
    {
        private readonly ILogger _logger;
        private readonly string _eventHubName;
        private readonly string _eventHubConnectionString;

        public CustomEventHubSenderService(
            IConfiguration configuration, 
            ILogger<CustomEventHubSenderService> logger)
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
        }

        public async Task SendEventBatch(List<LoginAuditViewModel> entries)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine($"There are no batch event entries to push.");
                return;
            }

            // Create a producer client that you can use to send events to an event hub
            await using (var producerClient = new EventHubProducerClient(_eventHubConnectionString, _eventHubName))
            {
                // Create a batch of events 
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                foreach (LoginAuditViewModel item in entries)
                {
                    this.InsertMessage(item, eventBatch);
                }

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                _logger.LogInformation($"SendEventBatch() .. a batch of {entries.Count} events has been published.");
                Console.WriteLine($"A batch of {entries.Count} events has been published.");
            }
        }

        public void InsertMessage(LoginAuditViewModel message, EventDataBatch eventBatch)
        {
            var payload = new
            {
                ID = message.ID.ToString(),
                UserLogin = message.UserLogin.ToString(),
                Country = message.Country.ToString(),
                WhenLoggedIn = message.WhenLoggedIn.ToString()
            };

            var eventMessage = JsonConvert.SerializeObject(payload);

            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventMessage)));

            Console.WriteLine($"Inserted: {message}");
        }
    }
}