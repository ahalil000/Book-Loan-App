using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using BookLoan.WebJob.Services;
using BookLoan.WebJob.Interfaces;
using BookLoan.WebJob.Data;


namespace BookLoan.WebJob
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
            _provider = services.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddConsole(opts => opts.IncludeScopes = true);
            });

            var environment = "";

            var hostBuilder = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    environment = hostingContext.HostingEnvironment.EnvironmentName;
                });

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            if (environment.Length > 0)
                configBuilder.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables();

            IConfiguration config = configBuilder.Build();

            this._configuration = config;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("AppDbContext")));

            services.AddSingleton(config);
            services.AddScoped<ICustomDbService, CustomDBService>();
        }
    }
    #endregion
}
