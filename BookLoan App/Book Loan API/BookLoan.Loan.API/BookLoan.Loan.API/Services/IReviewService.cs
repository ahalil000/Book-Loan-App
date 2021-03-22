using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Services
{
    public interface IReviewService
    {
        Task SaveReview(ReviewViewModel vm);
        Task UpdateReview(ReviewViewModel vm);
        Task<int> GetReviewerBookRating(int bookid, string user);
        Task<bool> WasBookReviewedByUser(int bookid, string user);
        Task<string> GetReviewerBookStarRating(int bookid, string user);
        Task<IEnumerable<ReviewViewModel>> GetBookReviews(int bookid);
    }
}
