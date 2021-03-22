using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Services
{
    public interface ILoanService
    {
        LoanViewModel CreateNewBookLoan(int bookid);
        Task<(BookLoan.Models.LoanViewModel, BookLoan.Models.BookViewModel)> GetReturnLoan(int bookid);
        Task<BookLoan.Models.BookStatusViewModel> GetBookLoanStatus(int bookid);
        Task<LoanViewModel> GetLoan(int id);
        Task<List<LoanViewModel>> GetBookLoans(int bookid);
        Task<List<BookLoan.Models.ReportViewModels.LoanedBookReportViewModel>> GetBooksLoanedByUser(string username);
        //Task<List<BookLoan.Models.ReportViewModels.LoanedBookReportViewModel>> GetBooksLoanedByCurrentUser();
        //Task<BookLoan.Models.ReviewViewModel> GetLoanForReview(int id);
        Task<BookLoan.Models.ReviewViewModel> GetLoanForReview(int id, string username);
        Task SaveLoan(LoanViewModel vm);
        Task ReturnLoan(LoanViewModel vm);
        Task UpdateLoan(LoanViewModel vm);
    }
}
