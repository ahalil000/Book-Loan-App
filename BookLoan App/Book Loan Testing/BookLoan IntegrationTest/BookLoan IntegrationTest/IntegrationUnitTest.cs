using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Services;
using BookLoan.Catalog.API.Events;
using BookLoanIntegrationTest.Models;


namespace BookLoanIntegrationTest
{
    public class Tests
    {
        private string lastResult;

        // Variables particular to methods.
        private static ILogger<BookService> logger;
        private IEventBus eventBus;
        private IOptions<AppConfiguration> appConfiguration;
        private IHttpClientFactory httpClientFactory;
        //private HttpContext context;

        // Shared setup properties.
        private IConfigurationRoot config;
        private string connection;
        private DbContextOptionsBuilder ctxbld;
        private ServiceCollection services;
        private ApplicationDbContext dbctxt;
        private ServiceProvider serviceProvider;


        [SetUp]
        public void Setup()
        {
            lastResult = "";

            ctxbld = new DbContextOptionsBuilder();

            services = new ServiceCollection();

            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("configuration.json")
                .Build();

            connection = config.GetConnectionString("AppDbContext");

            // For services testing.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddLogging(config =>
            {
                config.AddConsole(opts =>
                {
                    opts.IncludeScopes = true;
                });
                config.AddDebug();
            });

            // For HTTP testing.
            services.AddOptions();
            services.AddHttpClient();

            // App configurstions
            services.Configure<AppConfiguration>(config.GetSection("AppSettings"));

            // Build container services pipelines
            serviceProvider = services.BuildServiceProvider();

            // Obtain instances of container services.        
            httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            appConfiguration = serviceProvider.GetService<IOptions<AppConfiguration>>();

            dbctxt = serviceProvider.GetService<ApplicationDbContext>();
            logger = serviceProvider.GetService<ILogger<BookService>>();
            eventBus = serviceProvider.GetService<IEventBus>();
        }


        [Test]
        public async Task TestBookSelectQuery()
        {
            BookService esvc = new BookService(dbctxt, logger, eventBus);

            List<BookViewModel> lst = await esvc.GetBooks();

            Assert.IsTrue(lst.Count > 0);
        }


        [Test]
        public async Task TestAuthentication()
        {
            var userCredentials = new Configuration.UserCredentials()
            {
                UserName = "andy@adb.com.au",
                Password = "Abcd1234!"
            };

            string authToken = String.Empty;

            //context = new DefaultHttpContext();

            var tokenRequest = new HttpRequestMessage(
                HttpMethod.Post,
                appConfiguration.Value.UrlTokenAPI + "api/Users/Token");

            var tokenClient = httpClientFactory.CreateClient();

            Dictionary<string, string> keyvalues =
                new Dictionary<string, string>();
            keyvalues.Add("UserName", userCredentials.UserName);
            keyvalues.Add("Password", userCredentials.Password);

            var content = JsonConvert.SerializeObject(keyvalues);

            StringContent stringContent =
                new StringContent(content, System.Text.Encoding.UTF8,
                    "application/json");

            var tokenResponse = await tokenClient.PostAsync(tokenRequest.RequestUri, stringContent);

            if (tokenResponse.IsSuccessStatusCode)
            {
                string respContent = tokenResponse.Content.ReadAsStringAsync().Result;

                JObject tokenObjects = JsonConvert.DeserializeObject<JObject>(respContent);
                foreach (JToken tokenObject in tokenObjects.Values())
                {
                    if (tokenObject.Values("values") != null)
                    {
                        authToken = tokenObject.Value<string>("token").ToString();
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(authToken))
                Assert.Fail("Authentication token cannot be obtained. Check credentials.");
            else
                Assert.Pass("Authentication token successfully obtained.");

            lastResult = authToken;
        }


        [Test]
        public async Task TestHttpGetWithAuthentication()
        {
            await TestAuthentication();

            string authToken = lastResult;

            if (String.IsNullOrEmpty(authToken))
                Assert.Fail("Authentication token cannot be obtained. Check credentials.");

            //context = new DefaultHttpContext();

            string appUri = appConfiguration.Value.UrlCatalogAPI + "api/Book/List";

            var getRequest = new HttpRequestMessage(HttpMethod.Get, appUri);

            var getClient = httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(authToken))
            {
                getClient.DefaultRequestHeaders.Accept.Clear();
                getClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                getClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);
            }

            var getResponse = await getClient.SendAsync(getRequest);

            List<BookViewModel> bookViews = new List<BookViewModel>();

            if (getResponse.IsSuccessStatusCode)
            {
                string respContent = getResponse.Content.ReadAsStringAsync().Result;

                JArray books = JArray.Parse(respContent);
                foreach (var item in books)
                {
                    BookViewModel bitem = JsonConvert.DeserializeObject<BookViewModel>(item.ToString());
                    bookViews.Add(bitem);
                }
            }
            else
            {
                string errorString = getResponse.Headers.WwwAuthenticate.ToString().Replace("Bearer", "");
                if (!errorString.StartsWith("{") || !errorString.EndsWith("}"))
                    errorString = "{ " + errorString + " }";
                errorString = errorString.Replace("=", ":");
                ApiErrorResponse apiErrorResponse =
                    JsonConvert.DeserializeObject<ApiErrorResponse>(errorString);
                Assert.Fail("Error calling API method. " + apiErrorResponse.ErrorDescription);
            }

            if (bookViews.Count == 0)
            {
                Assert.Fail("Books cannot be retrieved.");
            }
            else
            {
                Assert.Pass($"{bookViews.Count} books retrieved.");
            }
        }
    }
}