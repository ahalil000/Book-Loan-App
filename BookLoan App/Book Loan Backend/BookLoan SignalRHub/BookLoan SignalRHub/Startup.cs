using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using Server;

using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;
using BackgroundTasksSample.Services;


namespace BookLoanSignalRHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole()
                .AddDebug());

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddHttpClient();

            services.AddTransient<ApiServiceHelper, ApiServiceHelper>();
            services.AddTransient<IApiServiceRetry, ApiServiceRetry>();
            services.AddTransient<IApiServiceRetryWithDelay, ApiServiceRetryWithDelay>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRouting();
            services.AddSignalR();

            services.AddTransient<IBookService, BookService>();
            services.AddHostedService<TimedHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<BookLoanHub>("/bookloanhub");
            });

            app.UseMvc();
        }
    }
}
