using System;
using System.Collections.Generic;
using System.Text;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoanTest.TDD.Moq
{
    public interface MockApplicationDbContext
    {
        int Add(object record);
        int Delete(object record);
        int SaveChanges();
    }

    public interface MockBookService
    {
        int bookCount { get; set; }
        List<BookViewModel> GetBooks();
        List<BookViewModel> GetBooksFilter(string filter);
    }
}
