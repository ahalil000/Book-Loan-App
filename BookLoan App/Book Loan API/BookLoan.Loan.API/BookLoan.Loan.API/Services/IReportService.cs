using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IReportService
    {
        Task<List<BookLoan.Models.LoanReportDto>> OnLoanReport(string currentuser = null);
        Task<List<BookLoan.Models.BookStatusViewModel>> OnLoanReportV1(string currentuser = null);
        Task<List<BookLoan.Models.LoanReportDto>> MyOnLoanReport();
        Task<List<BookLoan.Models.ReportViewModels.TopBooksLoanedReportViewModel>> TopLoanedBooksReport();
        Task<List<BookLoan.Models.ReportViewModels.TopMemberLoansReportViewModel>> MostOutstandingLoansReport();
        Task<bool> CurrentUserAnyOverdueLoans();
        Task<int> GetRecentLoansCount();
        Task<int> GetRecentReturnsCount();
    }
}
