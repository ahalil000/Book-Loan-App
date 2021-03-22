using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using BookLoan.Models;

namespace BookLoanMicroservices.Models.MessagingModels
{
    [Serializable]
    public class BookRecordSyncModel
    {
        public string CommandType { get; set; }
        public BookViewModel bookData;
    }
}
