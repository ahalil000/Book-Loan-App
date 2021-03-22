using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoanIntegrationTest
{
    public class AppConfiguration
    {
        public AppConfiguration() { }

        public string UrlCatalogAPI { get; set; }
        public string UrlLoanAPI { get; set; }
        public string UrlTokenAPI { get; set; }
    }
}
