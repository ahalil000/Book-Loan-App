using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


namespace BookLoanEventHubReceiverConsoleApp
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

                    //IConfiguration config = configBuilder.Build();

                    //services.AddSingleton(config);

                    services.AddTransient<ICustomEventHubReceiverService, CustomEventHubReceiverService>();

                    services.AddHostedService<TimerEventJobService>();
                });
    }
}
