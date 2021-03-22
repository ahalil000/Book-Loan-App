using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IUserAuthorizationService
    {
        bool IsUserAuthorized(string username, string permission, string resource);
    }
}
