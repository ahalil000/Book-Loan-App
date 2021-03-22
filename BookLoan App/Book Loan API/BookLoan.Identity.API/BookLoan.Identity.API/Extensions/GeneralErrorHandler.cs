using System;
using BookLoan.Identity.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace BookLoan.Identity.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var errorDetails = new ErrorDetail()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        };
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
                        
                        logger.LogError(String.Format("Stacktrace of error: {0}", contextFeature.Error.StackTrace.ToString()));
                    }
                });
            });
        }
    }
}
