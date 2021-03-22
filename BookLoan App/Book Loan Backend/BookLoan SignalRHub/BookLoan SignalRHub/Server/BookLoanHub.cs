using System;
using System.Collections.Generic;
using HubServiceInterfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using BookLoan.Services;
using BookLoan.Models;

namespace Server
{
#region BookLoanHub
    public class BookLoanHub : Hub<IBookLoanChange>
    {
        private readonly BookService bookService;

        public BookLoanHub(BookService _bookService)
        {
            bookService = _bookService;
        }

        public async Task SendBookCatalogChanges()
        {
            List<BookViewModel> list = 
                await bookService.GetChangedBooksList();
            int numberBooks = list.ToArray().Length;
            await Clients.All.SendBookCatalogChanges(numberBooks);
        }
    }
#endregion
}