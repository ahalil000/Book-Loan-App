using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Models;
using BookLoan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookLoan.Services;
using BookLoan.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.JsonPatch;


namespace BookLoan.Controllers
{
    //[Route("api/[controller]")]
    public class BookController : Controller
    {
        readonly ApplicationDbContext _db;
        readonly IBookService _bookService;
        private readonly ILogger _logger;

        public BookController(ApplicationDbContext db,
            ILogger<BookController> logger,
            IBookService bookService)
        {
            _db = db;
            _logger = logger;
            _bookService = bookService;
        }


        [HttpPatch("api/[controller]/BookPatch")]
        public async Task<IActionResult> BookPatch([FromBody]
            JsonPatchDocument<PatchBookViewModel> patchDoc)
        {
            if (patchDoc != null)
            {
                var originalBooks = await _bookService.GetBooks();
                var originalBooksCopy = originalBooks.ConvertAll(x => new BookViewModel(x));
                var patchOriginalBooksCopy = new PatchBookViewModel()
                {
                    books = originalBooksCopy
                };

                patchDoc.ApplyTo(patchOriginalBooksCopy, ModelState);
                await _bookService.ApplyPatching(patchOriginalBooksCopy);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(patchOriginalBooksCopy);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// List()
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/[controller]/List")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<BookViewModel>> List()
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _db.Books.ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        /// <summary>
        /// AllBooks()
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/[controller]/AllBooks")]
        public async Task<List<BookViewModel>> AllBooks()
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _db.Books.ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        /// <summary>
        /// GetChangedBooksList()
        /// Get books changed or added in the past 24 hours
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/[controller]/ChangedBooksList")]
        public async Task<List<BookViewModel>> GetChangedBooksList()
        {
            try
            {
                DateTime dtUpdated = DateTime.Now.AddHours(-24);
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _db.Books.Where(a => a.DateUpdated > dtUpdated).ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        [HttpGet("api/[controller]/GetNLatestBooksV1/{numberBooks}")]
        public async Task<List<BookViewModel>> GetNLatestBooksV1(int numberBooks)
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _bookService.GetNLatestBooksV1(numberBooks);
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        [HttpGet("api/[controller]/GetNLatestBooksV2/{numberBooks}")]
        public async Task<List<BookViewModel>> GetNLatestBooksV2(int numberBooks)
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _bookService.GetNLatestBooksV2(numberBooks);
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        [HttpGet("api/[controller]/GetNLatestBooksV3/{numberBooks}")]
        public async Task<List<BookViewModel>> GetNLatestBooksV3(int numberBooks)
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _bookService.GetNLatestBooksV3(numberBooks);
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        [HttpGet("api/[controller]/GetNLatestBooksV4/{numberBooks}")]
        public async Task<List<BookViewModel>> GetNLatestBooksV4(int numberBooks)
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _bookService.GetNLatestBooksV4(numberBooks);
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }


        [HttpGet("api/[controller]/GetNLatestBooksV5/{numberBooks}")]
        public async Task<List<BookViewModel>> GetNLatestBooksV5(int numberBooks)
        {
            try
            {
                List<BookViewModel> bvm = new List<BookViewModel>();
                var books = await _bookService.GetNLatestBooksV5(numberBooks);
                return books;
            }
            catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }


        // GET: Book/Details/5
        /// <summary>
        /// Details()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/[controller]/Details/{id}")]
        //[Authorize(Policy = "BookReadAccess")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound(new { id });
            }
            try
            {
                BookStatusViewModel bvm = new BookStatusViewModel();
                var book = await _db.Books.Where(a => a.ID == id).SingleOrDefaultAsync();
                //var loanStatus = await _loanservice.GetBookLoanStatus(id);
                if (book != null)
                {
                    bvm.ID = book.ID;
                    bvm.Title = book.Title;
                    bvm.Author = book.Author;
                    bvm.Genre = book.Genre;
                    bvm.Location = book.Location;
                    bvm.YearPublished = book.YearPublished;
                    bvm.Edition = book.Edition;
                    bvm.Genre = book.Genre;
                    bvm.ISBN = book.ISBN;
                    bvm.DateCreated = book.DateCreated;
                    bvm.DateUpdated = book.DateUpdated;

                    //bvm.Status = loanStatus.Status.ToString().ToUpper();
                    //bvm.DateLoaned = loanStatus.DateLoaned;
                    //bvm.DateReturn = loanStatus.DateReturn;
                }
                //BookLoan.Views.Book.DetailsModel detailsModel = new DetailsModel(_db);
                //loanStatus.Status = loanStatus.Status.ToUpper();
                //detailsModel.BookViewModel = bvm;
                //detailsModel.BookStatusViewModel = loanStatus;

                return Ok(bvm);
            }
            catch (Exception ex)
            {
                return BadRequest( new { ex.Message });
            }
        }


        // POST: Book/Create
        /// <summary>
        /// Create()
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost("api/[controller]/Create")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(IFormCollection collection)
        public async Task<ActionResult> Create([FromBody] BookViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                //int Id = System.Convert.ToInt32(collection["BookViewModel.ID"].ToString());
                string sTitle = model.Title; //  collection["BookViewModel.Title"].ToString();
                string sAuthor = model.Author; // collection["BookViewModel.Author"].ToString();
                int iYearPublished = System.Convert.ToInt32(model.YearPublished.ToString());
                //System.Convert.ToInt32(collection["BookViewModel.YearPublished"].ToString());
                string sGenre = model.Genre; //collection["BookViewModel.Genre"].ToString();
                string sEdition = model.Edition; // collection["BookViewModel.Edition"].ToString();
                string sISBN = model.ISBN; //  collection["BookViewModel.ISBN"].ToString();
                string sLocation = model.Location; // collection["BookViewModel.Location"].ToString();

                BookViewModel book = new BookViewModel()
                {
                    Title = sTitle,
                    Author = sAuthor,
                    YearPublished = iYearPublished,
                    Genre = sGenre,
                    Edition = sEdition,
                    ISBN = sISBN,
                    Location = sLocation,
                    DateCreated = DateTime.Today
                };

                if (ModelState.IsValid)
                {
                    await _bookService.SaveBook(book);
                    return RedirectToAction("Details", "Book", new { id = book.ID });
                }
                //ViewData["BookID"] = new SelectList(_db.Books, "ID", "Author", book.ID);
                return Ok(book); //  View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Cannot create book record.");
            }
        }


        // POST: Book/Edit/5
        /// <summary>
        /// Edit()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost("api/[controller]/Edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [FromBody] BookViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                int Id = model.ID; // System.Convert.ToInt32(collection["BookViewModel.ID"].ToString());
                string sTitle = model.Title; //collection["BookViewModel.Title"].ToString();
                string sAuthor = model.Author; // collection["BookViewModel.Author"].ToString();
                int iYearPublished = System.Convert.ToInt32(model.YearPublished);
                //System.Convert.ToInt32(collection["BookViewModel.YearPublished"].ToString());
                string sGenre = model.Genre; // collection["BookViewModel.Genre"].ToString();
                string sEdition = model.Edition; // collection["BookViewModel.Edition"].ToString();
                string sISBN = model.ISBN; // collection["BookViewModel.ISBN"].ToString();
                string sLocation = model.Location; // collection["BookViewModel.Location"].ToString();

                BookViewModel updatedata = new BookViewModel()
                {
                    ID = Id,
                    Title = sTitle,
                    Author = sAuthor,
                    Edition = sEdition,
                    Genre = sGenre,
                    ISBN = sISBN,
                    Location = sLocation,
                    YearPublished = iYearPublished,
                    DateUpdated = DateTime.Now
                };

                BookViewModel updated = await _bookService.UpdateBook(id, updatedata);
                if (updated != null)
                    return RedirectToAction("Details", "Book", new { id = updated.ID });

                return Ok(true);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST: Book/Update/5
        /// <summary>
        /// Edit()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut("api/[controller]/Update/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, [FromBody] BookViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                int Id = model.ID; // System.Convert.ToInt32(collection["BookViewModel.ID"].ToString());
                string sTitle = model.Title; //collection["BookViewModel.Title"].ToString();
                string sAuthor = model.Author; // collection["BookViewModel.Author"].ToString();
                int iYearPublished = System.Convert.ToInt32(model.YearPublished);
                //System.Convert.ToInt32(collection["BookViewModel.YearPublished"].ToString());
                string sGenre = model.Genre; // collection["BookViewModel.Genre"].ToString();
                string sEdition = model.Edition; // collection["BookViewModel.Edition"].ToString();
                string sISBN = model.ISBN; // collection["BookViewModel.ISBN"].ToString();
                string sLocation = model.Location; // collection["BookViewModel.Location"].ToString();

                BookViewModel updatedata = new BookViewModel()
                {
                    ID = Id,
                    Title = sTitle,
                    Author = sAuthor,
                    Edition = sEdition,
                    Genre = sGenre,
                    ISBN = sISBN,
                    Location = sLocation,
                    YearPublished = iYearPublished,
                    DateUpdated = DateTime.Now
                };

                BookViewModel updated = await _bookService.UpdateBook(id, updatedata);
                if (updated != null)
                    return RedirectToAction("Details", "Book", new { id = updated.ID });

                return Ok(true);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: Book/Delete/5
        /// <summary>
        /// Delete()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost("api/[controller]/Delete/{id}")]
        [ValidateAntiForgeryToken]
        //[Route("Delete/{id}")]
        public ActionResult Delete(int id, BookLoan.Models.BookViewModel collection) 
            // IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}