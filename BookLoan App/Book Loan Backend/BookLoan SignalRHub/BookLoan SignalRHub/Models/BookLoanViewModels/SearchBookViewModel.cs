using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookLoan.Models
{
    public class SearchBookViewModel
    {
        private List<string> _genres; 

        public IList<BookViewModel> Book { get; set; }
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Genres { get; set; }
        public string BookGenre { get; set; }

        public SearchBookViewModel()
        {
            _genres = new List<string>();
            _genres.Add("fantasy");
            _genres.Add("fiction");
            _genres.Add("non-fiction");

            Genres = new SelectList(_genres);
        }
    }
}

