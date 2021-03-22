using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Catalog.API.Helpers
{

    public class BookLoanComparer : IEqualityComparer<BookViewModel>
    {
        // items are equal if their names and item numbers are equal.
        public bool Equals(BookViewModel x, BookViewModel y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the items' properties are equal.
            return (x.ID.Equals(y.ID) &&
                    x.Author.Equals(y.Author) &&
                    x.Title.Equals(y.Title) &&
                    x.Edition.Equals(y.Edition) &&
                    x.Genre.Equals(y.Genre) &&
                    x.ISBN.Equals(y.ISBN) &&
                    x.Location.Equals(y.Location) &&
                    x.YearPublished.Equals(y.YearPublished) &&
                    x.DateCreated.Equals(y.DateCreated) &&
                    x.DateUpdated.Equals(y.DateUpdated));
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(BookViewModel item)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(item, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashItemName = item.ID.GetHashCode();

            //Calculate the hash code for the item.
            return hashItemName;
        }
    }
}
