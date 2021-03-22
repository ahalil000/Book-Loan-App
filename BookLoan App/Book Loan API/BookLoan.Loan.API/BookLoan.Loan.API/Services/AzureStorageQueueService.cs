using System.Collections.Generic;
using System.Linq;

using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Text;

using BookLoan.Models;
using BookLoan.Services;


namespace BookLoan.Loan.API.Services
{
    public class AzureStorageQueueService: IAzureStorageQueueService
    {
        private readonly IOptions<AppConfiguration> _appConfiguration;
        private QueueClient _queueClient;
        private readonly string _queueName;

        public AzureStorageQueueService(IOptions<AppConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _queueName = _appConfiguration.Value.QueueName;           
        }

        public bool CreateQueue()
        {
            try
            {
                // Get the connection string from app settings
                string connectionString = _appConfiguration.Value.StorageConnectionString;

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                _queueClient = new QueueClient(connectionString, _queueName);

                // Create the queue
                _queueClient.CreateIfNotExists();

                if (_queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{_queueClient.Name}'");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Make sure the Azure storage emulator is running and try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azure storage emulator is running and try again.");
                return false;
            }
        }


        public void InsertMessage(ReviewViewModel message)
        {
            if (!this.CreateQueue())
                return;

            if (_queueClient.Exists())
            {
                var payload = new {
                    ID = message.ID.ToString(),
                    BookID = message.BookID.ToString(),
                    Approver = message.Reviewer.ToString(),
                    Rating = message.Rating.ToString(),
                    Comment = message.Comment.ToString()
                };

                var queuedMessage = JsonConvert.SerializeObject(payload);

                _queueClient.SendMessage(queuedMessage);
            }

            Console.WriteLine($"Inserted: {message}");
        }
    }
}
