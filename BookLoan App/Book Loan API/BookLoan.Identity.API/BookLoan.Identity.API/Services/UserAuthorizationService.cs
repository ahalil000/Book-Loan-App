using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Services;

namespace BookLoan.Services
{

    public class UserAuthorizationService: IUserAuthorizationService
    {
        private IUserRoleService _userRoleService;

        public UserAuthorizationService(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// IsUserAuthorized()
        /// </summary>
        /// <param name="username"></param>
        /// <param name="permission"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool IsUserAuthorized(string username, string permission, string resource)
        {
            Task<bool> isInAdminRoleAction = _userRoleService.IsUserInRole(username, "Admin");
            bool isInAdminRole = isInAdminRoleAction.GetAwaiter().GetResult();
            if (isInAdminRole)
            {
                return true;
            }
            Task<bool> isInManagerRoleAction = _userRoleService.IsUserInRole(username, "Manager");
            bool isInManagerRole = isInManagerRoleAction.GetAwaiter().GetResult();
            if (isInManagerRole)
            {
                return true;
            }
            Task<bool> isInMemberRoleAction = _userRoleService.IsUserInRole(username, "Member");
            bool isInMemberRole = isInMemberRoleAction.GetAwaiter().GetResult();
            if (isInMemberRole)
            {
                if (resource == "Book")
                {
                    if ((permission == "Create") || (permission == "Delete") || (permission == "Update"))
                    {
                        return false;
                    }
                }

                if (resource == "BookLoan")
                {
                    if ((permission == "Delete") || (permission == "Update"))
                    {
                        return false;
                    }
                }

                if (resource == "Review")
                {
                    if ((permission == "Delete") || (permission == "Update"))
                    {
                        return false;
                    }
                }
            }
            return false;
        }

    }
}
