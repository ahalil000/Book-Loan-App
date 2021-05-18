using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BookLoanEventHubSenderConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configBuilder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

                    IConfiguration config = configBuilder.Build();

                    services.AddDbContext<ApplicationDbContext>(options =>                        
                        options.UseSqlServer(config.GetConnectionString("AppDbContext")
                        ), ServiceLifetime.Transient, ServiceLifetime.Scoped);

                    services.AddSingleton(config);

                    var samplingWindow = new SamplingWindowConfiguration()
                    {
                        StartSampleDateTime = DateTime.Now,
                        EndSampleDateTime = DateTime.Now
                    };

                    services.AddTransient<ICustomDbService, CustomDBService>();
                    services.AddTransient<ICustomEventHubSenderService, CustomEventHubSenderService>();

                    services.AddSingleton(samplingWindow);

                    services.AddHostedService<LoginEventJobService>();
                });
    }
}
