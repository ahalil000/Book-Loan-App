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

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;


using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;
using Server;
using BackgroundTasksSample.Services;


namespace BookLoanSignalRHubConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
