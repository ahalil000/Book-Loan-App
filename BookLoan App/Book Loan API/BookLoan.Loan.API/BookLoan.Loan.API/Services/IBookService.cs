using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;
using Microsoft.AspNetCore.Mvc; // IActionResult


namespace BookLoan.Services
{
    public interface IBookService
    {
        Task<List<BookViewModel>> GetBooks();
        Task<List<BookViewModel>> GetBooksFilter(string filter);
        Task<BookLoan.Models.BookViewModel> GetBook(int id);
        Task<IActionResult> SaveBook(BookViewModel vm);
        Task<BookViewModel> UpdateBook(int id, BookViewModel vm);
        Task PopulateBooks();
    }
}
