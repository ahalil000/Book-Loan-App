using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Loan.API.Helpers
{
    public class ModelResponseHelper
    {
        public LoanViewModel GetLoanReponse(LoanViewModel source)
        {
            if (source == null)
                return null;
            return new LoanViewModel()
            {
                ID = source.ID,
                BookID = source.BookID,
                DateCreated = source.DateCreated,
                DateDue = source.DateDue,
                DateLoaned = source.DateLoaned,
                DateReturn = source.DateReturn,
                DateUpdated = source.DateUpdated,
                LoanedBy = source.LoanedBy,
                OnShelf = source.OnShelf
            };
        }

    }
}
