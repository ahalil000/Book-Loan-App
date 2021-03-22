using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using BookLoan.WebJob.Interfaces;

namespace BookLoan.WebJob
{
    public class Functions
    {
        private const string QUEUE_NAME = "ahstoragequeue2211";

        public static void ProcessQueueMessage([QueueTrigger(QUEUE_NAME)] string message, ILogger logger)
        {
            logger.LogInformation(message);
            Startup startup = new Startup();
            var dbService = startup.serviceProvider.GetRequiredService<ICustomDbService>();
            dbService.RunDBProcess();
            logger.LogInformation($"Number of books in catalog is {dbService.GetResult}.");
        }
    }
}
