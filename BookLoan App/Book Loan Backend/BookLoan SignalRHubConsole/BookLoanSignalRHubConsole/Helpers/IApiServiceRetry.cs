using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLoan.Interfaces
{
    public interface IApiServiceRetry
    {
        Task RunAsync(Func<Task> func);
        void IncrementRetry();
    }
}
