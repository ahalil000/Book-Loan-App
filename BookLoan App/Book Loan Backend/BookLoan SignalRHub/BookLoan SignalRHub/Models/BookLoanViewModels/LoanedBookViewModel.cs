using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models.BookLoanViewModels
{
    public class LoanedBookViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int YearPublished { get; set; }

        public string Genre { get; set; }

        public string LoanedBy { get; set; }

        public DateTime LastDateLoaned { get; set; }

        public int BookID { get; set; }
    }
}
