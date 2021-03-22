using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using BookLoan.Models;
using BookLoan.Loan.API.Services;
using Microsoft.Extensions.Options;


namespace BookLoan.Services
{
    public class ReviewService: IReviewService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly IAzureStorageQueueService _azureStorageQueueService;
        private readonly IOptions<AppConfiguration> _appConfiguration;

        public ReviewService(ApplicationDbContext db,
            IBookService bookService,
            IOptions<AppConfiguration> appConfiguration,
            IAzureStorageQueueService azureStorageQueueService,
            ILogger<ReviewService> logger)
        {
            _db = db;
            _logger = logger;
            _bookService = bookService;
            _appConfiguration = appConfiguration;
            _azureStorageQueueService = azureStorageQueueService;
        }


        /// <summary>
        /// SaveReview()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveReview(ReviewViewModel vm)
        {
            ReviewViewModel reviewViewModel = new ReviewViewModel()
            {
                BookID = vm.BookID,
                Heading = vm.Heading,
                Comment = vm.Comment,
                Rating = vm.Rating,
                DateReviewed = vm.DateReviewed,
                DateCreated = DateTime.Now,
                Reviewer = vm.Reviewer,
                IsVisible = false,
                Approver = ""
            };
            _db.Add(reviewViewModel);
            await _db.SaveChangesAsync();
            _azureStorageQueueService.InsertMessage(reviewViewModel);
        }


        /// <summary>
        /// UpdateReview()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateReview(ReviewViewModel vm)
        {
            vm.DateUpdated = DateTime.Now;
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// WasBookReviewedByUser()
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> WasBookReviewedByUser(int bookid, string user)
        {
            return await _db.Reviews.Where(r => r.BookID == bookid && r.Reviewer == user).AnyAsync();
        }


        /// <summary>
        /// GetReviewerBookRating
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> GetReviewerBookRating(int bookid, string user)
        {
            ReviewViewModel review = await _db.Reviews.Where(r => r.BookID == bookid && r.Reviewer == user).SingleOrDefaultAsync();
            if (review != null)
            {
                return review.Rating;
            }
            return 0;
        }


        /// <summary>
        /// GetReviewerStartBookRating()
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetReviewerBookStarRating(int bookid, string user)
        {
            int rating = await GetReviewerBookRating(bookid, user);
            if (rating > 0)
                return new string('*', rating);
            return string.Empty;
        }


        public async Task<IEnumerable<ReviewViewModel>> GetBookReviews(int bookid)
        {
            return await _db.Reviews.Where(b => b.BookID == bookid).ToListAsync();
        }
    }
}
