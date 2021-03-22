using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Models.ReportViewModels;
using BookLoan.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using BookLoan.Loan.API.Helpers;


namespace BookLoan.Controllers
{
    //[EnableCors("CorsPolicy")]
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpClientFactory _clientFactory;

        ILoanService _loanService;
        IReviewService _reviewService;
        IReportService _reportService;

        public LoanController(ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            ILoanService loanService,
            IReviewService reviewService,
            IHttpClientFactory httpClientFactory,
            IReportService reportService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _loanService = loanService;
            _reviewService = reviewService;
            _clientFactory = httpClientFactory;
            _reportService = reportService;
        }

        // GET: LoanViewModels
        [HttpGet("api/[controller]/Index")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.Book);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LoanViewModels/Details/5
        [HttpGet("api/[controller]/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(new { id });
            }

            LoanViewModel lvm = await _loanService.GetLoan((int)id);

            if (lvm == null)
            {
                return NotFound();
            }

            return Ok(new ModelResponseHelper().GetLoanReponse(lvm));
        }


        [HttpGet("api/[controller]/BookDetails/{id}")]
        public async Task<IActionResult> BookDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoanViewModel lvm = await _loanService.GetLoan((int)id);
            if (lvm == null)
            {
                return NotFound();
            }

            var bookLoan = await _loanService.GetLoan((int)id);
            return Ok(new ModelResponseHelper().GetLoanReponse(bookLoan));
        }


        [HttpGet("api/[controller]/GetBookLoans/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<LoanViewModel>> GetBookLoans(int id)
        {
            var lvmList = await _loanService.GetBookLoans((int)id);

            return lvmList;
        }

        // GET: LoanViewModels/Create
        [HttpGet("api/[controller]/Create/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(int id)
        {
            LoanViewModel lvm = _loanService.CreateNewBookLoan(id);
            lvm.LoanedBy = User.Identity.Name;
            return Ok(lvm);
        }

        // GET: LoanViewModels/Return
        [HttpGet("api/[controller]/Return/{id}")]
        public async Task<IActionResult> Return(int id)
        {
            LoanViewModel lvm;
            (lvm, _) = await _loanService.GetReturnLoan(id);
            return Ok(lvm);
        }



        // POST: LoanViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("api/[controller]/Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Create([FromBody] LoanViewModel loanViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _loanService.SaveLoan(loanViewModel);

                    return Ok(loanViewModel);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
            return BadRequest();
        }


        [HttpPost("api/[controller]/Return2")]
        public async Task<IActionResult> Return2([FromBody] LoanViewModel loanViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _loanService.ReturnLoan(loanViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _loanService.GetLoan(loanViewModel.ID) == null) 
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Loan", new { id = loanViewModel.ID });
            }
            return View(loanViewModel);
        }


        // POST: LoanViewModels/Return
        [HttpPost("api/[controller]/Return")]
        public async Task<IActionResult> Return([FromBody] LoanViewModel loanViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _loanService.ReturnLoan(loanViewModel);

                    return Ok(loanViewModel);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (await _loanService.GetLoan(loanViewModel.ID) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest(new { ex.Message });
                    }
                }
                catch (Exception ex1)
                {
                    return BadRequest(new { ex1.Message });
                }
            }
            return Ok(loanViewModel);
        }


        // GET: LoanViewModels/Edit/5
        [HttpGet("api/[controller]/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanViewModel = await _loanService.GetLoan((int)id);
            if (loanViewModel == null)
            {
                return NotFound();
            }
            return Ok(loanViewModel); 
        }

        // POST: LoanViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("api/[controller]/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LoanedBy,DateLoaned,DateReturn,OnShelf,DateCreated,DateUpdated,BookID")] LoanViewModel loanViewModel)
        {
            if (id != loanViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _loanService.UpdateLoan(loanViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _loanService.GetLoan(loanViewModel.ID) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return await this.Details(loanViewModel.BookID);
            }
            return Ok(loanViewModel); 
        }

        // GET: LoanViewModels/Delete/5
        [HttpGet("api/[controller]/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanViewModel = await _context.Loans
                .Include(l => l.Book)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (loanViewModel == null)
            {
                return NotFound();
            }

            return Ok(loanViewModel); 
        }

        // POST: LoanViewModels/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpPost("api/[controller]/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanViewModel = await _context.Loans.SingleOrDefaultAsync(m => m.ID == id);
            _context.Loans.Remove(loanViewModel);
            await _context.SaveChangesAsync();
            return Ok(new { id }); 
        }


        // GET: LoanViewModels/GetBookLoanStatus/5
        [HttpGet("api/[controller]/GetBookLoanStatus/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBookLoanStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                BookLoan.Models.BookStatusViewModel blvStat =
                    new BookLoan.Models.BookStatusViewModel();
                blvStat = await _loanService.GetBookLoanStatus((int)id);

                if (blvStat == null)
                {
                    return NotFound();
                }

                // Return loaned book status.
                return Ok(blvStat);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // GET: LoanViewModels/GetBooksLoanedByUser/username
        [HttpGet("api/[controller]/GetBooksLoanedByUser/{username}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<LoanedBookReportViewModel>> GetBooksLoanedByUser(string username)
        {
            return await _loanService.GetBooksLoanedByUser(username);
        }


        // GET: LoanViewModels/GetTopMemberLoans
        [HttpGet("api/[controller]/GetTopMemberLoans")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTopMemberLoans()
        {
            var loanRanking = await _reportService.MostOutstandingLoansReport();
            return Ok(loanRanking);
        }


        // GET: LoanViewModels/GetTopBooksLoaned
        [HttpGet("api/[controller]/GetTopBooksLoaned")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTopBooksLoaned()
        {
            var loanRanking = await _reportService.TopLoanedBooksReport();
            return Ok(loanRanking);
        }

        // GET: LoanViewModels/GetRecentLoansCount
        [HttpGet("api/[controller]/GetRecentLoansCount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRecentLoansCount()
        {
            var numberRecentLoans = await _reportService.GetRecentLoansCount();
            return Ok(numberRecentLoans);
        }

        // GET: LoanViewModels/GetRecentReturnsCount
        [HttpGet("api/[controller]/GetRecentReturnsCount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRecentReturnsCount()
        {
            var numberRecentReturns = await _reportService.GetRecentReturnsCount();
            return Ok(numberRecentReturns);
        }

        [HttpGet("api/[controller]/GetLoanReportAllMembers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetLoanReportAllMembers()
        {
            var loanReportAllMembers = await _reportService.OnLoanReport();
            return Ok(loanReportAllMembers);
        }


        [HttpPost("api/[controller]/ReviewCreate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ReviewCreate([FromBody] ReviewViewModel reviewViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _reviewService.SaveReview(reviewViewModel);

                    return Ok(reviewViewModel);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpGet("api/[controller]/GetBookReviews/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBookReviews(int id)
        {
            var bookReviews = await _reviewService.GetBookReviews(id);
            return Ok(bookReviews);
        }


        [NonAction]
        private bool LoanViewModelExists(int id)
        {
            return _context.Loans.Any(e => e.ID == id);
        }
    }
}
