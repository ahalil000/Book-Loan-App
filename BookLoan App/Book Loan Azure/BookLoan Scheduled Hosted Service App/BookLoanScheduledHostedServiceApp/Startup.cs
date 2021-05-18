using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Json;

using Quartz;
using Quartz.Impl;
using BackgroundTasksSample.Services;


namespace BookLoanScheduledHostedServiceApp
{
#region Startup
    public class Startup
    {
        private IConfiguration _configuration;
        private readonly IServiceProvider _provider;

        public IServiceProvider serviceProvider
        {
            get { return this._provider; }
        }
        public IConfiguration configuration
        {
            get { return this._configuration; }
        }

        public Startup()
        {
            var services = new ServiceCollection();
            this.ConfigureServices(services);
            //_provider = 
            services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //var environment = "";

            //var hostBuilder = new HostBuilder()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureAppConfiguration((hostingContext, config) =>
            //    {
            //        environment = hostingContext.HostingEnvironment.EnvironmentName;
            //    });

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            //if (environment.Length > 0)
            //    configBuilder.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
            //configBuilder.AddEnvironmentVariables();

            IConfiguration config = configBuilder.Build();

            this._configuration = config;

            var pollingInterval = 0;

            try
            {
                pollingInterval = this._configuration.GetSection("ScheduleSettings").GetValue<int>("pollingInterval");
            }
            catch (Exception ex)
            {
                pollingInterval = 30;
            }

            Console.Out.WriteLineAsync($"Polling interval set to {pollingInterval}");

            services.AddQuartz(q =>
            {
                // construct a scheduler factory
                StdSchedulerFactory factory = new StdSchedulerFactory();

                // get a scheduler
                IScheduler scheduler = factory.GetScheduler().GetAwaiter().GetResult();
                scheduler.Start();

                // base quartz scheduler, job and trigger configuration
                IJobDetail job = JobBuilder.Create<SampleJob>()
                    .WithIdentity("mySampleJob", "group1")
                    .Build();

                // Trigger the job to run now, and then every [pollingInterval] seconds
                ITrigger trigger = TriggerBuilder.Create()
                  .WithIdentity("mySampleTrigger1", "group1")
                  .StartNow()
                  .WithSimpleSchedule(x => x
                      .WithIntervalInSeconds(pollingInterval)
                      .RepeatForever())
                  .Build();

                scheduler.ScheduleJob(job, trigger);

                // Schedule the daily lunch break.
                IJobDetail lunchJob = JobBuilder.Create<LunchJob>()
                    .WithIdentity("mySampleLunchJob", "group1")
                    .Build();

                ITrigger cronTriggerLunch = TriggerBuilder.Create()
                  .WithIdentity("mySampleTrigger2", "group1")
                  .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(2, 00))
                  .ForJob(lunchJob)
                  .Build();

                scheduler.ScheduleJob(lunchJob, cronTriggerLunch);

                // Schedule monthly staff pay transfer.
                IJobDetail payTransferJob = JobBuilder.Create<TransferPayJob>()
                  .WithIdentity("mySamplePayTransferJob", "group1")
                  .Build();

                ITrigger cronTriggerTheyGetPaid = TriggerBuilder.Create()
                  .WithIdentity("mySampleTrigger3", "group1")
                  .WithCronSchedule("0 1 2 4 * ? ")
                  .ForJob(payTransferJob)
                  .Build();

                scheduler.ScheduleJob(payTransferJob, cronTriggerTheyGetPaid);
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            services.AddHostedService<TimedHostedService>();
        }
    }
#endregion
}
