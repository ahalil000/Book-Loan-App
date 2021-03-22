using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    [Serializable]
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

        public BookViewModel() { }

        public BookViewModel(BookViewModel obj)
        {
            this.ID = obj.ID;
            this.Title = obj.Title;
            this.Author = obj.Author;
            this.YearPublished = obj.YearPublished;
            this.Genre = obj.Genre;
            this.Edition = obj.Edition;
            this.ISBN = obj.ISBN;
            this.Location = obj.Location;
            this.DateCreated = obj.DateCreated;
            this.DateUpdated = obj.DateUpdated;
        }
    }
}

