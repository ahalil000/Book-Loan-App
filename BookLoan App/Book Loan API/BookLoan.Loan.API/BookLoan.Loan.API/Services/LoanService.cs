using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Security.Claims;
using BookLoan.Data;
//using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using BookLoan.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BookLoan.Models;

using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using Microsoft.Extensions.Http;
using System.Net.Http;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;


namespace BookLoan.Services
{
    public class LoanService: ILoanService
    {
        private readonly IHttpClientFactory _clientFactory;

        ApplicationDbContext _db;
        ILogger _logger;
        //IBookService _bookService;
        //IReviewService _reviewService;
        //UserManager<ApplicationUser> _userManager;
        HttpContext _context;

        public LoanService(ApplicationDbContext db, 
                //UserManager<ApplicationUser> userManager, 
                IHttpContextAccessor httpContextAccessor,
                //IBookService bookService,
                //IReviewService reviewService,
                IHttpClientFactory httpClientFactory,
                ILogger<LoanService> logger)
        {
            _db = db;
            _logger = logger;
            //_userManager = userManager;
            _context = httpContextAccessor.HttpContext;
            _clientFactory = httpClientFactory;
            //_bookService = bookService;
            //_reviewService = reviewService;
        }

        /// <summary>
        /// GetLoan()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LoanViewModel> GetLoan(int id)
        {
            var loanViewModel = await _db.Loans
                .Include(l => l.Book)
                .SingleOrDefaultAsync(m => m.ID == id);
            return loanViewModel;
        }


        /// <summary>
        /// SaveLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveLoan(LoanViewModel vm)
        {
            vm.DateCreated = DateTime.Now;
            vm.DateUpdated = DateTime.Now;
            _db.Add(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// UpdateLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task ReturnLoan(LoanViewModel vm)
        {
            vm.DateReturn = DateTime.Now;
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// UpdateLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateLoan(LoanViewModel vm)
        {
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// CreateNewBookLoan()
        /// </summary>
        /// <returns></returns>
        public LoanViewModel CreateNewBookLoan(int bookid)
        {
            LoanViewModel lvm = new LoanViewModel();
            lvm.DateLoaned = DateTime.Now;
            lvm.DateReturn = new DateTime(1, 1, 1); // fix 16/11/2019
            lvm.DateCreated = DateTime.Now;
            lvm.DateDue = DateTime.Now;
            lvm.OnShelf = true;
            lvm.BookID = bookid;
            return lvm;
        }


        /// <summary>
        /// GetLoan()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<LoanViewModel>> GetBookLoans(int bookid)
        {
            var loans = await _db.Loans
                //.Include(l => l.Book)
                .Where(m => m.BookID == bookid).ToListAsync();
            return loans;
        }


        /// <summary>
        /// Return Book() - Return book from loan
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public async Task<(BookLoan.Models.LoanViewModel, BookLoan.Models.BookViewModel)> GetReturnLoan(int bookid)
        {

            LoanViewModel lvm = new LoanViewModel();
            BookViewModel bvm = new BookViewModel();

            BookViewModel book = new BookViewModel();

            var request = new HttpRequestMessage(HttpMethod.Get,
                        "http://localhost/BookLoan.Catalog.API/api/Book/List");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

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

            if (!bookViews.Any())
            {
                throw new Exception(String.Format("Books cannot be found."));
            }

            var bookloans = await GetBookLoans(bookid);

            if (bookloans == null)
            {
                throw new Exception(String.Format("Loans for Book {0} cannot be found.", bookid));
            }

            // Get latest loan for the book. 
            foreach (LoanViewModel item in bookloans.OrderByDescending(o => o.DateLoaned))
            {
                lvm.ID = item.ID;
                lvm.DateLoaned = item.DateLoaned;
                lvm.DateDue = item.DateDue;
                lvm.DateReturn = DateTime.Now;
                lvm.DateUpdated = DateTime.Now;
                lvm.OnShelf = false;
                lvm.LoanedBy = "";
                lvm.BookID = item.BookID;
                bvm.Title = book.Title;
                bvm.Author = book.Author;
                break;
            }

            return (lvm, bvm);
        }


        // GET: LoanViewModels/GetBookLoanStatus/5
        public async Task<BookLoan.Models.BookStatusViewModel> GetBookLoanStatus(int bookid)
        {
            BookLoan.Models.BookStatusViewModel bsvm = new Models.BookStatusViewModel() { Status = "N/A" };

            List<BookLoan.Models.LoanViewModel> loans = await _db.Loans.
                Where(m => m.BookID == bookid).ToListAsync();

            if (!loans.Any())
                bsvm.Status = "Available";

            foreach (BookLoan.Models.LoanViewModel rec in loans.OrderByDescending(a => a.DateLoaned))
            {
                bool foundState = false;

                bsvm.Borrower = rec.LoanedBy;
                bsvm.DateLoaned = rec.DateLoaned;
                bsvm.DateReturn = rec.DateReturn;
                bsvm.DateDue = rec.DateDue;

                if (DateTime.Now >= rec.DateReturn)
                {
                    bsvm.Status = "Available";
                    foundState = true;
                }
                if (DateTime.Now <= rec.DateDue)
                {
                    bsvm.Status = "On Loan";
                    if ((DateTime.Now > rec.DateReturn) && (rec.DateReturn.Year != 1))
                        bsvm.Status = "Available";
                    foundState = true;
                }
                if ((DateTime.Now > rec.DateDue) && (rec.DateDue.Year != 1))
                {
                    bsvm.Status = "Overdue";
                    foundState = true;
                }
                if (foundState)
                    break;
            }
            return bsvm;
        }


        /// <summary>
        /// GetBooksLoanedByUser
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookLoan.Models.ReportViewModels.LoanedBookReportViewModel>> GetBooksLoanedByUser(string username)
        {
            string curruser = username;

            var bookloans = new Dictionary<int, BookLoan.Models.ReportViewModels.LoanedBookReportViewModel>();

            var userloans = await _db.Loans
                .Include(l => l.Book)
                .Where(m => m.LoanedBy == curruser).ToListAsync();
            foreach (BookLoan.Models.LoanViewModel itm in userloans)
            {

                if (!bookloans.ContainsKey(itm.BookID))
                {
                    bookloans.Add(
                        itm.BookID,
                        new Models.ReportViewModels.LoanedBookReportViewModel()
                        {
                            Author = itm.Book.Author,
                            Title = itm.Book.Title,
                            Genre = itm.Book.Genre,
                            YearPublished = itm.Book.YearPublished,
                            BookID = itm.BookID,
                            LastDateLoaned = itm.DateLoaned,
                            ID = itm.ID,
                            WasBookReviewed = false, // await _reviewService.WasBookReviewedByUser(itm.BookID, curruser),
                            Rating = "0" // await _reviewService.GetReviewerBookStarRating(itm.BookID, curruser)
                        }
                    );
                }
                else
                {
                    if (itm.DateLoaned > bookloans[itm.BookID].LastDateLoaned)
                        bookloans[itm.BookID].LastDateLoaned = itm.DateLoaned;
                }
            }
            return bookloans.Values.ToList();
        }


        /// <summary>
        /// GetLoanForReview()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookLoan.Models.ReviewViewModel> GetLoanForReview(int id, string username)
        {
            string curruser = username;

            var userloan = await _db.Loans
                .Include(l => l.Book)
                .Where(m => m.ID == id).SingleOrDefaultAsync();
            if (userloan != null)
            {
                return new Models.ReviewViewModel()
                {
                    Author = userloan.Book.Author,
                    Title = userloan.Book.Title,
                    BookID = userloan.BookID,
                    ID = userloan.ID,
                    Reviewer = curruser,
                    DateReviewed = DateTime.Now,
                    Comment = "",
                    Heading = "",
                    Rating = 0
                };
            }
            return null;
        }

    }
}
