using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models.BookLoanViewModels;

namespace BookLoan.Models.ReportViewModels
{
    public class LoanedBookReportViewModel: LoanedBookViewModel
    {
        public bool WasBookReviewed { get; set; }

        public string Rating { get; set; }
    }
}
