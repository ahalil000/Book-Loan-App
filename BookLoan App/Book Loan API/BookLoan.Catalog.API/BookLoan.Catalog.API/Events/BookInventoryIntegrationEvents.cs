using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;


namespace BookLoan.Catalog.API.Events
{
    [Serializable]
    public class BookInventoryChangedIntegrationEvent: IntegrationEvent
    {
        public int BookId { get; private set; }
        public string NewEdition { get; private set; }
        public string OldEdition { get; private set; }

        public BookInventoryChangedIntegrationEvent(int bookId, string newEdition,
            string oldEdition)
        {
            BookId = bookId;
            NewEdition = newEdition;
            OldEdition = oldEdition;
        }
    }

    [Serializable]
    public class BookInventoryNewIntegrationEvent: IntegrationEvent
    {
        public int BookId { get; private set; }
        public BookViewModel newBook { get; private set; }

        public BookInventoryNewIntegrationEvent(int bookId, BookViewModel newBookModel)
        {
            BookId = bookId;
            newBook = newBookModel;
        }
    }

}
