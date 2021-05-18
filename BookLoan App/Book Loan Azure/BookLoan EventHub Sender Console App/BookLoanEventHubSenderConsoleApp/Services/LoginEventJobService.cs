using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BookLoanEventHubSenderConsoleApp
{
    #region snippet1
    internal class LoginEventJobService : IHostedService, IDisposable
    {
        private readonly ICustomDbService _customDbService;
        private readonly ICustomEventHubSenderService _customEventHubSenderService;

        private readonly ILogger _logger;
        private Timer _timer;

        private readonly int _pollingInterval;
        private readonly int _numberOfEvents;

        private int _numberEventsSubmitted;

        public LoginEventJobService(IConfiguration configuration, 
            ICustomDbService customDbService,
            ICustomEventHubSenderService customEventHubSenderService, 
            ILogger<LoginEventJobService> logger)
        {
            _logger = logger;

            _customDbService = customDbService;
            _customEventHubSenderService = customEventHubSenderService;

            _numberEventsSubmitted = 0;

            _pollingInterval = 0;

            try
            {
                _pollingInterval = configuration.GetSection("AppSettings").GetValue<int>("PollingInterval");
            }
            catch (Exception ex)
            {
                _pollingInterval = 30;
            }

            _numberOfEvents = 0;

            try
            {
                _numberOfEvents = configuration.GetSection("AppSettings").GetValue<int>("NumberOfEvents");
            }
            catch (Exception ex)
            {
                _numberOfEvents = 30;
            }
            
            Console.Out.WriteLineAsync($"Polling interval set to {_pollingInterval}");
            Console.Out.WriteLineAsync($"Number of events set to {_numberOfEvents}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login Event Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(_pollingInterval));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (this._numberEventsSubmitted < this._numberOfEvents)
            {
                this._numberEventsSubmitted++;
                _logger.LogInformation($"Login Event {_numberEventsSubmitted} Being Retrieved.");

                // Do some processing here.
                var loginAudits = _customDbService.RetrieveNextAuditEntries().GetAwaiter().GetResult();

                _logger.LogInformation($"Number of Login Events read: {loginAudits.Count}.");

                foreach (LoginAuditViewModel val in loginAudits)
                {
                    string msg = $"Dat: {val.WhenLoggedIn.ToLongDateString()}, Time: {val.WhenLoggedIn.ToLongTimeString()}, User: {val.UserLogin}, Location: {val.Country}.";
                    _logger.LogInformation(msg);
                }

                // Add audit to event hub.
                _customEventHubSenderService.SendEventBatch(loginAudits);
            }
            else
            {
                _logger.LogInformation("Login Event Service idle. All events retrieved.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login Event Service is stopping.");

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
