using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Identity.API.Helpers;
using BookLoan.Identity.API.Models;
using BookLoan.Identity.API;
using BookLoan.Identity.API.Extensions;


namespace BookLoan.Identity.API
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

            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

            // Configure password policies
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppConfiguration>(appSettingsSection);

            services.Configure<AppConfiguration>(Configuration.GetSection("AppSettings"));
            services.AddScoped<TokenManager, TokenManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

            //var key = Encoding.ASCII.GetBytes(appSettingsSection.GetValue<string>("Secret"));
            var key = Convert.FromBase64String(Configuration.GetSection("AppSettings:Secret").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //x.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                //        var userId = int.Parse(context.Principal.Identity.Name);
                //        var user = userService.GetById(userId);
                //        if (user == null)
                //        {
                //            // return unauthorized if user no longer exists
                //            context.Fail("Unauthorized");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                //x.Audience = "http://localhost:25138/";
                //x.Authority = "http://localhost:25138/";
                //x.Audience = "http://localhost/";
                //x.Authority = "http://localhost/";
            });

            services.AddControllers();

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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookLoan Identity API",
                    Version = "v1",
                    Description = "A BookLoan Identity ASP.NET Core Web API",
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
                    In = ParameterLocation.Header, 
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.ConfigureExceptionHandler(logger);

                //Log all errors in the application
                //app.UseExceptionHandler(errorApp =>
                //{
                //    errorApp.Run(async context =>
                //    {
                //        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                //        var exception = errorFeature.Error;

                //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                //        if (contextFeature != null)
                //        {
                //            var errorDetails = new ErrorDetail()
                //            {
                //                StatusCode = context.Response.StatusCode,
                //                Message = "Internal Server Error."
                //            };
                //            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
                //        }

                //        logger.LogError(String.Format("Stacktrace of error: {0}", exception.StackTrace.ToString()));
                //    });
                //});
            }
            app.UseCors("Cors");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLoan Identity API");
                c.RoutePrefix = string.Empty;
            }
            );

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated(); // create database if not already created.
                Initialize(context);
            }
        }


        public void Initialize(ApplicationDbContext context)
        {
            // Do any initialization code here for the DB. 
            // Can include populate lookups, data based configurations etc.
            var appOptions = new AppConfiguration();
            Configuration.GetSection("AppSettings").Bind(appOptions);

            SeedAccounts seed_acc = new SeedAccounts(context, appOptions);
            var task = Task.Run(async () => await seed_acc.GenerateUserAccounts());
            var result = task.Wait(5000);
        }
    }
}
