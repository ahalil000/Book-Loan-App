using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server;
using HubServiceInterfaces;
using BookLoan.Services;
using BookLoan.Models;


namespace BackgroundTasksSample.Services
{
    #region snippet1
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IBookService _bookService;
        private readonly IHubContext<BookLoanHub, IBookLoanChange> _bookLoanHub;

        public TimedHostedService(ILogger<TimedHostedService> logger,
            IHubContext<BookLoanHub, IBookLoanChange> bookLoanHub,
            IBookService bookService)
        {
            _logger = logger;
            _bookLoanHub = bookLoanHub;
            _bookService = bookService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            List<BookViewModel> list =
                _bookService.GetChangedBooksList().GetAwaiter().GetResult();

            int numberBooks = list.ToArray().Length;

            _logger.LogInformation(String.Format("Number of new books {0}", numberBooks));

            _bookLoanHub.Clients.All.SendBookCatalogChanges(numberBooks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    #endregion
}
