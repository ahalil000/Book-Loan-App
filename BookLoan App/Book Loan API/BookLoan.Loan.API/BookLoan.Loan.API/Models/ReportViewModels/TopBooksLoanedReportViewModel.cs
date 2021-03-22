using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models.BookLoanViewModels;

namespace BookLoan.Models.ReportViewModels
{
    public class TopBooksLoanedReportViewModel
    {
        public int ranking { get; set; }

        public int count { get; set; }

        public string averageRating { get; set; }

        public BookViewModel bookDetails { get; set; }

        public TopBooksLoanedReportViewModel()
        {
            bookDetails = new BookViewModel();
        }
    }
}
