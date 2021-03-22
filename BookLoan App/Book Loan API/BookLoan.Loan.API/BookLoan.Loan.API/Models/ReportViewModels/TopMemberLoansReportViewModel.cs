using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models.BookLoanViewModels;

namespace BookLoan.Models.ReportViewModels
{
    public class TopMemberLoansReportViewModel
    {
        public int ranking { get; set; }

        public int count { get; set; }

        public string userName { get; set; }

        public string userEmail { get; set; }
    }
}
