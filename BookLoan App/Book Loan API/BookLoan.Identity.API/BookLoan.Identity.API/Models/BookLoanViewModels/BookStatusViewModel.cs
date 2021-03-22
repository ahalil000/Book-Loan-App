using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class BookStatusViewModel: BookViewModel
    {
        public string Status { get; set; }

        public DateTime DateLoaned { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime DateReturn { get; set; }

        public string Borrower { get; set; }

        public bool OnShelf { get; set; }

        public string DateLoanedFormattedString
        {
            get { return (DateLoaned.Year == 1) ? "N/A" : DateLoaned.ToString("yyyy-MM-dd"); }
        }

        public string DateDueFormattedString
        {
            get { return (DateDue.Year == 1) ? "N/A" : DateDue.ToString("yyyy-MM-dd"); }
        }

        public string DateReturnFormattedString
        {
            get { return (DateReturn.Year == 1) ? "N/A" : DateReturn.ToString("yyyy-MM-dd"); }
        }
    }
}

