using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public class AppConfiguration
    {
        public AppConfiguration() { }

        public string AdminEmail { get; set; } 
 
        public string AdminPwd { get; set; } 
 
        public string AppName { get; set; } 

        public string AppVersion { get; set; } 

        public decimal OverdueBookFee { get; set; }

        public decimal LostBookFee { get; set; }

        public string ServiceBusConnection { get; set; }

        public string ServiceBusQueue { get; set; }
    }
}
