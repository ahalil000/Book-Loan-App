using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Loan.API.Services
{
    public interface IAzureStorageQueueService
    {
        bool CreateQueue();
        void InsertMessage(ReviewViewModel message);
    }
}
