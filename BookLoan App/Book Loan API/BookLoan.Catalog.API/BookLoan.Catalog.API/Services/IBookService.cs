using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;
using BookLoan.Data;

namespace BookLoan.Services
{
    public interface IBookService
    {
        Task ApplyPatching(PatchBookViewModel vm);
        Task<List<BookViewModel>> GetBooks();
        Task<List<BookViewModel>> GetBooksFilter(string filter);
        Task<BookLoan.Models.BookViewModel> GetBook(int id);
        Task SaveBook(BookViewModel vm);
        Task<BookViewModel> UpdateBook(int id, BookViewModel vm);
        Task<List<BookViewModel>> GetNLatestBooksV1(int numberBooks);
        Task<List<BookViewModel>> GetNLatestBooksV2(int numberBooks);
        Task<List<BookViewModel>> GetNLatestBooksV3(int numberBooks);
        Task<List<BookViewModel>> GetNLatestBooksV4(int numberBooks);
        Task<List<BookViewModel>> GetNLatestBooksV5(int numberBooks);
    }
}
