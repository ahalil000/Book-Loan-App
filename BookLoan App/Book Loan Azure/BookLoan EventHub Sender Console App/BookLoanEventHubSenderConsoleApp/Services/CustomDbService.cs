using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookLoanEventHubSenderConsoleApp
{
    public class CustomDBService: ICustomDbService 
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _db;
        private readonly SamplingWindowConfiguration _samplingWindowConfiguration;

        private int _result;

        public int GetResult => _result;

        public CustomDBService(
            IConfiguration configuration, 
            ILogger<CustomDBService> logger, 
            ApplicationDbContext db,
            SamplingWindowConfiguration samplingWindowConfiguration)
        {
            this._logger = logger;
            this._db = db;
            this._samplingWindowConfiguration = samplingWindowConfiguration;
        }

        public void ReadSamplingWindow()
        {
            var sampleWindow = _db.SamplingWindows.SingleOrDefault();

            DateTime dateTime = new DateTime(1900, 1, 1);

            this._samplingWindowConfiguration.StartSampleDateTime = dateTime;
            this._samplingWindowConfiguration.EndSampleDateTime = dateTime;

            if (sampleWindow != null)
            {
                this._samplingWindowConfiguration.StartSampleDateTime = 
                    sampleWindow.StartSampleDateTime;
                this._samplingWindowConfiguration.EndSampleDateTime =
                    sampleWindow.EndSampleDateTime;
            }
        }

        public async Task UpdateSamplingWindow()
        {
            var sampleWindow = _db.SamplingWindows.SingleOrDefault();

            if (sampleWindow != null)
            {
                sampleWindow.StartSampleDateTime =
                    this._samplingWindowConfiguration.StartSampleDateTime;
                sampleWindow.EndSampleDateTime =
                    this._samplingWindowConfiguration.EndSampleDateTime;
                _db.Update(sampleWindow);
                await _db.SaveChangesAsync();
            }
            else
            {
                SamplingWindowViewModel samplingWindow =
                    new SamplingWindowViewModel()
                    {
                        StartSampleDateTime = new DateTime(1900, 1, 1),
                        EndSampleDateTime = new DateTime(1900, 1, 1)
                    };
                await _db.AddAsync(sampleWindow);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<List<LoginAuditViewModel>> RetrieveNextAuditEntries()
        {
            this.ReadSamplingWindow();
            DateTime startDateTime = _samplingWindowConfiguration.StartSampleDateTime;
            DateTime endDateTime = _samplingWindowConfiguration.EndSampleDateTime;

            if ((startDateTime.Year == 1900) && (startDateTime.Month == 1) && (startDateTime.Day == 1))
            {
                Console.WriteLine("Error RetrieveNextAuditEntries() ..no sampling window dates found!");
                _logger.LogInformation($"Error RetrieveNextAuditEntries() ..no sampling window dates found!");
                return new List<LoginAuditViewModel>();
            }

            Console.WriteLine("Running RetrieveNextAuditEntries() ..in Background Worker Service.");
            _logger.LogInformation($"Running RetrieveNextAuditEntries() ..in Background Worker Service.");

            Console.WriteLine($"RetrieveNextAuditEntries() .. start-date={startDateTime.ToString()}, end-date={endDateTime.ToString()}.");
            _logger.LogInformation($"RetrieveNextAuditEntries() .. start-date={startDateTime.ToString()}, end-date={endDateTime.ToString()}.");

            var auditEntries = 
                _db.LoginAudits
                .Where(a => a.WhenLoggedIn >= startDateTime && a.WhenLoggedIn < endDateTime)
                .ToList();

            if (auditEntries.Any())
            {
                // Get latest logged date of audit entries and set it to the low end of next window.
                var latestLoginDate = auditEntries.OrderByDescending(a => a.WhenLoggedIn).First().WhenLoggedIn;
                this._samplingWindowConfiguration.StartSampleDateTime = latestLoginDate.AddMilliseconds(1);
                this._samplingWindowConfiguration.EndSampleDateTime = latestLoginDate.AddMinutes(5);
            }
            else
            {
                // When no audit entries advance the next window by five minutes.
                this._samplingWindowConfiguration.StartSampleDateTime = startDateTime.AddMinutes(5);
                this._samplingWindowConfiguration.EndSampleDateTime = startDateTime.AddMinutes(10);
            }
            await this.UpdateSamplingWindow();

            return auditEntries;


        }
    }
}