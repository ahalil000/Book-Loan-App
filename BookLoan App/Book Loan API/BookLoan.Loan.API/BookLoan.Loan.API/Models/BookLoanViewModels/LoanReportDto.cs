using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class LoanReportDto
    {
        public string Status { get; set; }
        public string Title { get; set; }
        public DateTime DateLoaned { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime DateReturn { get; set; }
        public string Borrower { get; set; }
        public int DaysOverdue { get; set; }
    }
}

