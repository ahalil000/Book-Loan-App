using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BookLoan.Models;
using BookLoan.Models.ReportViewModels;

using System.Net.Http;
using AutoMapper;


namespace BookLoan.Services
{
    public class ReportService : IReportService
    {
        ApplicationDbContext _db;
        ILogger _logger;
        ILoanService _loanService;
        IBookService _bookService;
        HttpContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public ReportService(ApplicationDbContext db,
            IHttpContextAccessor httpContextAccessor,
            IBookService bookService,
            ILoanService loanService,
            IHttpClientFactory httpClientFactory,
            ILogger<ReportService> logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _bookService = bookService;
            _loanService = loanService;
            _clientFactory = httpClientFactory;
            _context = httpContextAccessor.HttpContext;
            _mapper = mapper;
        }
    

        public async Task<bool> CurrentUserAnyOverdueLoans()
        {
            string curruser = _context.User.Identity.Name;
            List<BookLoan.Models.LoanReportDto> bookStatusViews =
                await MyOnLoanReport();
            return bookStatusViews.Any(a => a.Status == "OVERDUE");
        }


        public async Task<List<BookLoan.Models.LoanReportDto>> MyOnLoanReport()
        {
            string curruser = _context.User.Identity.Name;
            return await OnLoanReport(curruser);
        }

        public async Task<List<BookLoan.Models.BookStatusViewModel>> OnLoanReportV1(string currentuser = null)
        {
            List<BookLoan.Models.BookStatusViewModel> loanstats = new List<Models.BookStatusViewModel>();

            var books = await _bookService.GetBooks();

            foreach (Models.BookViewModel book in books)
            {
                BookLoan.Models.BookStatusViewModel bsvm = await _loanService.GetBookLoanStatus(book.ID);
                if (((currentuser != null) && (bsvm.Borrower == currentuser))
                        || (currentuser == null))
                {
                    loanstats.Add(new Models.BookStatusViewModel()
                    {
                        ID = book.ID,
                        Author = book.Author,
                        Title = book.Title,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        Edition = book.Edition,
                        Location = book.Location,
                        YearPublished = book.YearPublished,
                        OnShelf = bsvm.OnShelf,
                        DateLoaned = bsvm.DateLoaned,
                        DateReturn = bsvm.DateReturn,
                        DateDue = bsvm.DateDue,
                        Status = bsvm.Status,
                        Borrower = bsvm.Borrower
                    });
                }
            }
            return loanstats;
        }


        public async Task<List<BookLoan.Models.LoanReportDto>> OnLoanReport(string currentuser = null)
        {
            List<LoanReportDto> loanstats; 

            var books = await _bookService.GetBooks();

            LoanReportDto[] arrLoanStatus =
                Array.FindAll(
                    Array.ConvertAll(books.ToArray(),
                        new Converter<BookViewModel, LoanReportDto>(BookToBookLoanStatus)),
                    c => ((currentuser != null) && (c.Borrower == currentuser)) || (currentuser == null)
            );

            Array.Sort<LoanReportDto>(arrLoanStatus,
                (a, b) =>
                {
                    if (a.Borrower == null)
                    {
                        if (b.Borrower == null)
                            return 0;
                        else
                            return -1;
                    }
                    else
                    {
                        if (b.Borrower == null)
                            return 1;
                        else
                            return string.Compare(a.Borrower, b.Borrower);
                    }
                });

            loanstats = arrLoanStatus.ToList<LoanReportDto>();

            return loanstats;
        }

        private LoanReportDto BookToBookLoanStatus(BookViewModel input)
        {
            BookLoan.Models.BookStatusViewModel bookStatus = _loanService.GetBookLoanStatus(input.ID).GetAwaiter().GetResult();
            bookStatus.Title = input.Title;

            var loanReport =_mapper.Map<LoanReportDto>(bookStatus);
            
            return loanReport;
        }


        //public async Task<List<BookLoan.Models.BookStatusViewModel>> OverdueReport()
        //{
        //    List<BookLoan.Models.BookStatusViewModel> loanstats = new List<Models.BookStatusViewModel>();

        //    var bookloans = await _db.Loans.
        //        Where(a => a.DateDue <= DateTime.Now).
        //        ToListAsync();
        //    foreach (Models.LoanViewModel loan in bookloans)
        //    {
        //        BookLoan.Models.BookStatusViewModel bsvm = await _loanService.GetBookLoanStatus(loan.BookID);
        //        BookLoan.Models.BookViewModel bvm = await _bookService.GetBook(loan.BookID);
        //        loanstats.Add(new Models.BookStatusViewModel()
        //        {
        //            ID = bvm.ID,
        //            Author = bvm.Author,
        //            Title = bvm.Title,
        //            Genre = bvm.Genre,
        //            ISBN = bvm.ISBN,
        //            Edition = bvm.Edition,
        //            Location = bvm.Location,
        //            YearPublished = bvm.YearPublished,
        //            OnShelf = bsvm.OnShelf,
        //            DateLoaned = bsvm.DateLoaned,
        //            DateReturn = bsvm.DateReturn,
        //            Status = bsvm.Status
        //        });
        //    }
        //    return loanstats;
        //}



        // GET: LoanViewModels/GetBookLoanStatus/5
        public BookLoan.Models.BookStatusViewModel GetBookLoanStatus(int? bookid)
        {
            BookLoan.Models.BookStatusViewModel bsvm = new Models.BookStatusViewModel() { Status = "N/A" };

            List<BookLoan.Models.LoanViewModel> loans = _db.Loans.
                Where(m => m.BookID == bookid).ToList();

            if (!loans.Any())
                bsvm.Status = "Available";

            foreach (BookLoan.Models.LoanViewModel rec in loans.OrderByDescending(a => a.DateReturn))
            {
                bool foundState = false;

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


        public async Task<List<TopBooksLoanedReportViewModel>> TopLoanedBooksReport()
        {
            var loanRanking = new List<TopBooksLoanedReportViewModel>();

            var books = await _db.Books.ToListAsync();
            foreach (BookViewModel book in books)
            {
                loanRanking.Add(new TopBooksLoanedReportViewModel()
                {
                    bookDetails = book,
                    count = _db.Loans.Where(a => a.BookID == book.ID).Count()
                });
            }

            loanRanking = loanRanking.OrderByDescending(s => s.count).ToList();

            var ranking = 1;
            loanRanking.ForEach(r =>
            {
                r.ranking = ranking;
                ranking++;
            });
            return loanRanking;
        }

        public async Task<List<TopMemberLoansReportViewModel>> MostOutstandingLoansReport()
        {
            var loanRanking = new List<TopMemberLoansReportViewModel>();

            var loans = await _db.Loans.ToListAsync();
            foreach (LoanViewModel loan in loans)
            {
                var loanrec = loanRanking.Where(r => r.userEmail == loan.LoanedBy).SingleOrDefault();
                if (loanrec == null)
                {
                    loanRanking.Add(new TopMemberLoansReportViewModel()
                    {
                        userEmail = loan.LoanedBy,
                        userName = loan.LoanedBy,
                        count = 0,
                        ranking = 0
                    });
                }
                else
                    loanrec.count++;
            }

            loanRanking = loanRanking.OrderByDescending(s => s.count).ToList();

            var ranking = 1;
            loanRanking.ForEach(r =>
            {
                r.ranking = ranking;
                ranking++;
            });
            return loanRanking;
        }


        /// <summary>
        /// GetRecentLoansCount()
        /// Get number loaned in the past 24 hours
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecentLoansCount()
        {
            DateTime dtUpdated = DateTime.Now.AddHours(-24);
            var loans = await _db.Loans.Where(a => a.DateLoaned > dtUpdated).ToListAsync();
            return loans.Count();
        }

        /// <summary>
        /// GetRecentReturnsCount()
        /// Get number loans returned in the past 24 hours
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecentReturnsCount()
        {
            DateTime dtUpdated = DateTime.Now.AddHours(-24);
            var loans = await _db.Loans.Where(a => a.DateReturn > dtUpdated).ToListAsync();
            return loans.Count();
        }


        public async Task<List<TopBooksLoanedReportViewModel>> TopLoanedBooksReport_Old()
        {
            var loanRanking = new List<TopBooksLoanedReportViewModel>();

            var loans = await _db.Loans.ToListAsync();

            var groupedLoansByBook = loans.GroupBy(
                book => book.BookID,
                books => books.BookID,
                (book, books) => new
                {
                    Key = book,
                    Count = books.Count()
                });

            foreach (var result in groupedLoansByBook)
            {
                int bookID = Convert.ToInt32(result.Key.ToString());
                var book = _db.Books.Where(b => b.ID == bookID).SingleOrDefault();
                if (book != null)
                    loanRanking.Add(
                        new Models.ReportViewModels.TopBooksLoanedReportViewModel()
                        {
                            bookDetails = book,
                            count = result.Count
                        }
                    );
            }

            loanRanking = loanRanking.OrderByDescending(c => c.count).ToList();

            var ranking = 1;
            loanRanking.ForEach(r =>
            {
                r.ranking = ranking;
                ranking++;
            });

            return loanRanking;
        }


    }
}
