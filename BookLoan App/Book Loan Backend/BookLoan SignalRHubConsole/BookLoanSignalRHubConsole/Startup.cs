using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using BackgroundTasksSample.Services;
using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;


namespace Server
{
#region Startup
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole()
                .AddDebug());

            services.AddHttpClient();

            services.AddTransient<ApiServiceHelper, ApiServiceHelper>();
            services.AddTransient<IApiServiceRetry, ApiServiceRetry>();
            services.AddTransient<IApiServiceRetryWithDelay, ApiServiceRetryWithDelay>();
            services.AddTransient<IBookService, BookService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                            .WithOrigins("http://localhost:4200")
                            .AllowCredentials();
                }));

            services.AddSignalR();

            services.AddHostedService<TimedHostedService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseSignalR((routes) =>
            {
                routes.MapHub<BookLoanHub>("/bookloanhub");
            });
        }
    }
#endregion
}
