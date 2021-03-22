using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class ApiErrorResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
