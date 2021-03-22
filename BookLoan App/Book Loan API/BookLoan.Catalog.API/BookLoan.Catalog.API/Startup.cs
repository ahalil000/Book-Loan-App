using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

using BookLoan.Data;
using BookLoan.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using BookLoan.Catalog.API.Events;
using BookLoanMicroservices.Messaging;

using BookLoan.Catalog.API.Helpers;
using Microsoft.AspNet.OData.Extensions;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

using Microsoft.AspNet.OData.Batch;

using Microsoft.Extensions.Logging;


namespace BookLoan.Catalog.API
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
            services.AddLogging(config =>
            {
                config.AddConsole(opts =>
                {
                    opts.IncludeScopes = true;
                });
                config.AddDebug();
            });

            bool isInDockerContainer = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true");
            string connStr = isInDockerContainer ? "DOCKERconnstr" : "AppDbContext";

            // Db connection if inside or outside of contianer.
            if (isInDockerContainer)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONN_STR")));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(connStr)));
            }

            // jwt token authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(
                                Configuration.GetSection("AppSettings:Secret").Value)), 
                            //System.Text.Encoding.ASCII
                            //    .GetBytes(Configuration.GetSection("AppSettings:Secret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            //services.AddAuthorization();

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IEventBus, AzureEventBus>();
            services.AddTransient<IMessageBusHelper, MessageBusQueueHelper>();

            services.AddControllers();

            services.AddOData();
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            // Inject ISwaggerProvider with defaulted settings
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "BookLoan Catalog API", 
                    Version = "v1",
                    Description = "A Book Catalog ASP.NET Core Web API",
                    TermsOfService = new Uri("https://abcd.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Andrew Halil",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/ahalil"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://abcd.com/license"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header, //  "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey, //  "apiKey"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.Configure<AppConfiguration>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseODataBatching();

            var defaultODataBatchHander = new DefaultODataBatchHandler();
            defaultODataBatchHander.MessageQuotas.MaxNestingDepth = 2;
            defaultODataBatchHander.MessageQuotas.MaxOperationsPerChangeset = 10;
            defaultODataBatchHander.MessageQuotas.MaxReceivedMessageSize = 100;

            app.UseRouting();


            app.UseAuthorization();
            //app.UseMvc(o =>
            //{
            //    o.Select().Expand().Filter().OrderBy().Count();
            //    o.MapODataServiceRoute("odata", "odata", EdmHelpers.GetEdmModel(),
            //        ODataHelper.CreateDefaultODataBatchHander(2, 5, 100));
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                endpoints.MapODataRoute("odata", "api", EdmHelpers.GetEdmModel(), 
                    ODataHelper.CreateDefaultODataBatchHander(2, 5, 100));
                endpoints.MapControllers();
            });


            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLoan Catalog API");
                c.RoutePrefix = string.Empty;
            }
            );

            bool isInDockerContainer = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true");
            if (isInDockerContainer)
            {
                logger.LogInformation("Docker Db connection = " + Environment.GetEnvironmentVariable("DB_CONN_STR"));
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated(); // create database if not already created.

                //var config = app.ApplicationServices.GetService<AppConfiguration>();
                //Initialize(context);
                }
        }
    }
}
