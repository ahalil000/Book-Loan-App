using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Http; // HttpContext
using Microsoft.AspNetCore.Mvc; // IActionResult
using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue, AuthenticationHeaderValue etc

using BookLoan.Models;
using BookLoan.Services;
using BookLoan.Helpers;
using BookLoan.Interfaces;


namespace BookLoan.Services
{
    public class BookService: IBookService
    {
        ILogger _logger;

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpContext _context;
        private readonly ApiServiceHelper _apiServiceHelper;
        private readonly IApiServiceRetryWithDelay _apiServiceRetryWithDelay;

        public BookService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            ApiServiceHelper apiServiceHelper,
            IApiServiceRetryWithDelay apiServiceRetryWithDelay,
            ILogger<BookService> logger)
        {
            _logger = logger;
            _apiServiceHelper = apiServiceHelper;
            _apiServiceRetryWithDelay = apiServiceRetryWithDelay;
            _clientFactory = httpClientFactory;
            _context = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// GetChangedBooksList()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetChangedBooksList()
        {
            //string bearerToken = _context.Request.Headers["Authorization"].FirstOrDefault();
            //if (bearerToken == null)
            //    throw new Exception(String.Format("Cannot get books: No authentication token."));

            //bearerToken = bearerToken.Replace("Bearer", "");

            HttpResponseMessage response = null;
            var delayedRetry = _apiServiceRetryWithDelay;

            await delayedRetry.RunAsync(async () =>
            {
                response = await _apiServiceHelper.GetAPI(
                    "http://localhost/BookLoan.Catalog.API/api/Book/ChangedBooksList",
                    null,
                    null
                );
            });
            List<BookViewModel> bookViews = new List<BookViewModel>();

            if (response.IsSuccessStatusCode)
            {
                string respContent = response.Content.ReadAsStringAsync().Result;

                JArray books = JArray.Parse(respContent) as JArray;
                foreach (var item in books)
                {
                    BookViewModel bitem = JsonConvert.DeserializeObject<BookViewModel>(item.ToString());
                    bookViews.Add(bitem);
                }
            }
            else
            {
                string errorString = response.Headers.WwwAuthenticate.ToString().Replace("Bearer", "");
                if (!errorString.StartsWith("{") || !errorString.EndsWith("}"))
                    errorString = "{ " + errorString + " }";
                errorString = errorString.Replace("=", ":");
                ApiErrorResponse apiErrorResponse =
                    JsonConvert.DeserializeObject<ApiErrorResponse>(errorString);
                throw new AppException(apiErrorResponse.error_description);
            }

            if (bookViews == null)
            {
                throw new Exception(String.Format("Books cannot be found."));
            }

            return bookViews;
        }

        public Task<List<BookViewModel>> GetBooksFilter(string filter)
        {
            throw new NotImplementedException();
        }

        public Task<BookViewModel> GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> SaveBook(BookViewModel vm)
        {
            throw new NotImplementedException();
        }

        public Task<BookViewModel> UpdateBook(int id, BookViewModel vm)
        {
            throw new NotImplementedException();
        }

        public Task PopulateBooks()
        {
            throw new NotImplementedException();
        }

        public Task<List<BookViewModel>> GetBooks()
        {
            throw new NotImplementedException();
        }
    }
}
