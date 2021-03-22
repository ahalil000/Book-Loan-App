using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class LoanViewModel
    {
        public int ID { get; set; }

        public string LoanedBy { get; set; }

        public DateTime DateLoaned { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime DateReturn { get; set; }

        public bool OnShelf { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int BookID { get; set; }

        public BookViewModel Book { get; set; }

        public virtual ICollection<OverdueLoanViewModel> OverdueLoans { get; set; }
    }
}

