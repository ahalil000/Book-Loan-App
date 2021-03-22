using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class OverdueLoanViewModel
    {
        public int ID { get; set; }

        public int BookID { get; set; }

        public int LoanID { get; set; }

        public string LoanedBy { get; set; }

        public DateTime DateLoaned { get; set; }

        public DateTime DateDue { get; set; }

        public bool IsOverdueNoticeSent { get; set; }

        public bool IsOverdueFeePaid { get; set; }

        public bool IsLostBookFeePaid { get; set; }

        public bool IsBookLost { get; set; }

        public bool IsBookReturned { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public LoanViewModel Loan { get; set; }
    }
}

