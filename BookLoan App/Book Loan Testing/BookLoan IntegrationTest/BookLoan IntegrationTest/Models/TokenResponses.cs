using System;
using System.Collections.Generic;
using System.Text;

namespace BookLoanIntegrationTest.Models
{
    public class TokenResponses
    {
        public List<TokenValue> values { get; set; }

        public TokenResponses()
        {
            values = new List<TokenValue>();
        }
    }
}
