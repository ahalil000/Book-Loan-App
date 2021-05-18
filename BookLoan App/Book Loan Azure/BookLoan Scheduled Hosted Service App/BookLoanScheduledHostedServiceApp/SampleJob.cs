using System;
using System.Threading.Tasks;
using Quartz;

namespace BookLoanScheduledHostedServiceApp
{
    public class SampleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("This is a triggered job.");
        }
    }

    public class LunchJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Off for lunch now!");
        }
    }

    public class TransferPayJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Transfer monthly pay cheques.");
        }
    }

}
