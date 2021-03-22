using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IAppConfiguration
    {
        string AdminEmail { get; set; }
        string AdminPwd { get; set; }
        string AppName { get; set; }
        string AppVersion { get; set; }
        string ServiceBusConnection { get; set; }
        string ServiceBusQueue { get; set; }
    }
}
