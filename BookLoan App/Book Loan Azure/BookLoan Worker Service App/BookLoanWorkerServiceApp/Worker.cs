using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookLoanWorkerServiceApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, cancellationToken);
        }
    }
}
