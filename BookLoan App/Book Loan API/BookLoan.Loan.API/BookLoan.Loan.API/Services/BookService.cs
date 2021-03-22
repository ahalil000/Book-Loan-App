using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using Microsoft.Extensions.Logging;
using BookLoan.Models;

using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Http; // HttpContext
using Microsoft.AspNetCore.Mvc; // IActionResult
using Microsoft.Extensions.Options;
//using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue, AuthenticationHeaderValue etc


using BookLoan.Helpers;
using BookLoan.Interfaces;


namespace BookLoan.Services
{
    public class BookService: IBookService
    {
        ApplicationDbContext _db;
        ILogger _logger;

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpContext _context;
        private readonly ApiServiceHelper _apiServiceHelper;
        private readonly IApiServiceRetryWithDelay _apiServiceRetryWithDelay;
        private readonly IOptions<AppConfiguration> _appConfiguration;

        public BookService(
            ApplicationDbContext db, 
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            ApiServiceHelper apiServiceHelper,
            IApiServiceRetryWithDelay apiServiceRetryWithDelay,
            ILogger<BookService> logger,
            IOptions<AppConfiguration> appConfiguration)
        {
            _db = db;
            _logger = logger;
            _apiServiceHelper = apiServiceHelper;
            _apiServiceRetryWithDelay = apiServiceRetryWithDelay;
            _clientFactory = httpClientFactory;
            _context = httpContextAccessor.HttpContext;
            _appConfiguration = appConfiguration;
        }

        /// <summary>
        /// GetBooks()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooks()
        {
            //return await _db.Books.ToListAsync();

            string bearerToken = _context.Request.Headers["Authorization"].FirstOrDefault();
            if (bearerToken == null)
                throw new Exception(String.Format("Cannot get books: No authentication token."));

            bearerToken = bearerToken.Replace("Bearer", "");

            //_context.Request.Cookies["token"];
            //string bearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluQGJvb2tsb2FuLmNvbSIsIm5iZiI6MTU4MTQyNDM4MCwiZXhwIjoxNTgxNDI2MTgwLCJpYXQiOjE1ODE0MjQzODB9.CvATaOujiJZXTeAqG25u7Pos5h0wNLuSCtbHAVtCIew";

            HttpResponseMessage response = null;
            var delayedRetry = _apiServiceRetryWithDelay;

            //http://localhost/BookLoan.Catalog.API/api/Book/List,

            await delayedRetry.RunAsync(async () =>
            {
                response = await _apiServiceHelper.GetAPI(
                    _appConfiguration.Value.UrlCatalogAPI + "api/Book/List",
                    null,
                    bearerToken
                );
            });
            List<BookViewModel> bookViews = new List<BookViewModel>();

            if (response.IsSuccessStatusCode)
            {
                string respContent = response.Content.ReadAsStringAsync().Result;

                JArray books = JArray.Parse(respContent);
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


        /// <summary>
        /// GetBooksFilter()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooksFilter(string filter)
        {
            List<BookViewModel> books = await GetBooks();
            List<BookViewModel> filteredbooks = books.Where(b => b.Title.Contains(filter)).ToList();
            return filteredbooks;
        }


        // GET: LoanViewModels/GetBookLoanStatus/5

        /// <summary>
        /// GetBook()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookViewModel> GetBook(int id)
        {
            List<BookLoan.Models.BookViewModel> books = await GetBooks();

            BookViewModel result = null;

            foreach (BookViewModel book in books)
            {
                if (book.ID == id)
                {
                    result = book;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// SaveBook()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveBook(BookViewModel vm)
        {
            //http://localhost/BookLoan.Catalog.API/api/Book/Create);

            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    _appConfiguration.Value.UrlCatalogAPI + "api/Book/Create");

            var client = _clientFactory.CreateClient();

            Dictionary<string, string> keyvalues =
                new Dictionary<string, string>();
            keyvalues.Add("Title", vm.Title);
            keyvalues.Add("Author", vm.Author);
            keyvalues.Add("YearPublished", vm.YearPublished.ToString());
            keyvalues.Add("Genre", vm.Genre);
            keyvalues.Add("Edition", vm.Edition);
            keyvalues.Add("ISBN", vm.ISBN);
            keyvalues.Add("Location", vm.Location);

            var content = JsonConvert.SerializeObject(keyvalues);

            StringContent stringContent =
                new StringContent(content, System.Text.Encoding.UTF8,
                    "application/json");

            var response = await client.PostAsync(request.RequestUri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }

            throw new UnauthorizedAccessException();
        }

        /// <summary>
        /// UpdateBook()
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<BookViewModel> UpdateBook(int Id, BookViewModel vm)
        {
            //http://localhost/BookLoan.Catalog.API/api/Book/Edit
            
            var request = new HttpRequestMessage(
                        HttpMethod.Post,
                        _appConfiguration.Value.UrlCatalogAPI + "api/Book/Edit");

            var client = _clientFactory.CreateClient();

            Dictionary<string, string> keyvalues =
                new Dictionary<string, string>();
            keyvalues.Add("id", Id.ToString());
            keyvalues.Add("Title", vm.Title);
            keyvalues.Add("Author", vm.Author);
            keyvalues.Add("YearPublished", vm.YearPublished.ToString());
            keyvalues.Add("Genre", vm.Genre);
            keyvalues.Add("Edition", vm.Edition);
            keyvalues.Add("ISBN", vm.ISBN);
            keyvalues.Add("Location", vm.Location);

            var content = JsonConvert.SerializeObject(keyvalues);

            StringContent stringContent =
                new StringContent(content, System.Text.Encoding.UTF8,
                    "application/json");

            var response = await client.PostAsync(request.RequestUri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return vm;
            }

            throw new Exception($"Unable to update book {Id}");
        }

        /// <summary>
        /// PopulateBooks()
        /// </summary>
        public async Task PopulateBooks()
        {
            // Get books from Book Service BookLoan.Catalog.API
            List<BookViewModel> books = await GetBooks();

            // populate books 

            if (books.Count() == 0)
                return;

            if (_db.Books.Count() == 0)
                return;

            try
            {
                foreach (BookViewModel book in books)
                {
                    BookViewModel bvm = new BookViewModel()
                    {
                        Title = book.Title,
                        Author = book.Author,
                        Genre = book.Genre,
                        YearPublished = book.YearPublished,
                        Edition = book.Edition,
                        ISBN = book.ISBN,
                        Location = book.Location,
                        DateCreated = DateTime.Today,
                        DateUpdated = DateTime.Today
                    };
                    _db.Add(bvm);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
            }
        }
    }
}
