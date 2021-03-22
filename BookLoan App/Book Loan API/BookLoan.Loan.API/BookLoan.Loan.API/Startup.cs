using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

using BookLoan.Data;
using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;
using BookLoan.Loan.API.Services;

using AutoMapper;


namespace BookLoan.Loan.API
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
            bool isInDockerContainer = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true");
            string connStr = isInDockerContainer ? "DOCKERconnstr" : "AppDbContext";

            services.AddAutoMapper(typeof(Startup));

            services.AddOptions();

            services.PostConfigure<AppConfiguration>(opt =>
            {
                if (isInDockerContainer)
                {
                    opt.UrlCatalogAPI = Environment.GetEnvironmentVariable("URL_CATALOG_API");
                }
            });


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


            // jwt token authentication (client)
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
                }
            );

            services.AddHttpClient();

            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IAzureStorageQueueService, AzureStorageQueueService>();

            services.AddScoped<ApiServiceHelper, ApiServiceHelper>();
            services.AddTransient<IApiServiceRetry, ApiServiceRetry>();
            services.AddTransient<IApiServiceRetryWithDelay, ApiServiceRetryWithDelay>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

            //services.AddMvcCore(options =>
            //{
            //    foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
            //    {
            //        outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //    }

            //    foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
            //    {
            //        inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //    }
            //});

            // Inject ISwaggerProvider with defaulted settings
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookLoan Loan API",
                    Version = "v1",
                    Description = "A BookLoan Loan ASP.NET Core Web API",
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
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLoan Loan API");
                c.RoutePrefix = string.Empty;
            }
            );

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated(); // create database if not already created.
                //var config = app.ApplicationServices.GetService<AppConfiguration>();
                //Initialize(context);
            }
        }

        //public void Initialize(ApplicationDbContext context)
        //{
        //    // Do any initialization code here for the DB. 
        //    // Can include populate lookups, data based configurations etc.
        //    var appOptions = new AppConfiguration();
        //    Configuration.GetSection("AppSettings").Bind(appOptions);

        //    SeedData seed_data = new SeedData(context, appOptions);
        //    var task = Task.Run(async () => await seed_data.PopulateBooks());
        //    //var result = task.Wait(5000);
        //}


    }
}
