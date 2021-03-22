using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class BookViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int YearPublished { get; set; }

        public string Genre { get; set; }

        public string Edition { get; set; }

        public string ISBN { get; set; }

        [Required]
        public string Location { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public virtual ICollection<LoanViewModel> Loans { get; set; }

        public virtual ICollection<ReviewViewModel> Reviews { get; set; }
    }
}

