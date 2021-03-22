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
        public BookLoanHub()
        {
        }

        public async Task SendBookCatalogChanges(int numberBookLoanChanges)
        {
            await Clients.All.SendBookCatalogChanges(numberBookLoanChanges);
        }
    }
#endregion
}