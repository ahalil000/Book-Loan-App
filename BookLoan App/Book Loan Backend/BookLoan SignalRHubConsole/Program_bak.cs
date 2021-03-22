using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

//using Microsoft.AspNetCore.SignalR; //.Builder.Extensions; //.Routing;
//using Microsoft.AspNetCore.SignalR.Protocol;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;
using Server;
using BackgroundTasksSample.Services;



namespace BookLoanSignalRHubConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {   
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", true);
                        
                        
                        //config.
			//config.Sources.UseSignalR(options =>
			//{ 
   // 				options.MapHub<BookLoanHub>("/bookloanhub");
			//});
                        //if (args != null) config.AddCommandLine(args);
                    })
                    .ConfigureServices((hostingContext, services) =>
                    {
                        services.AddHttpClient();

                        services.AddTransient<ApiServiceHelper, ApiServiceHelper>();
                        services.AddTransient<IApiServiceRetry, ApiServiceRetry>();
                        services.AddTransient<IApiServiceRetryWithDelay, ApiServiceRetryWithDelay>();

                        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                        services.AddRouting();
                        services.AddSignalR().
                            AddHubOptions<BookLoanHub>(options =>
                            {
                                //options
                            });
                  
                        services.AddTransient<IBookService, BookService>();
                        services.AddHostedService<TimedHostedService>();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration);
                        logging.AddConsole();
                        logging.AddDebug();
                    });

            builder.UseConsoleLifetime();
            builder.Start();
        }
    }
}
