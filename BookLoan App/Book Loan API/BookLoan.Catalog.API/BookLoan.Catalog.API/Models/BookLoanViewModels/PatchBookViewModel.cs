using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Models
{
    public class PatchBookViewModel
    {
        public List<BookViewModel> books { get; set; }
        //public BookViewModel[] books { get; set; }
    }
}
