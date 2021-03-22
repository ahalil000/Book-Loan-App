using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Services
{
    public interface IUserRoleService
    {
        List<string> GetAllRoles();
        Task<List<string>> GetUserRoles(string userName);
        Task<bool> IsUserInRole(string userName, string role);
        Task AddUserToRole(string userName, string role);
        Task DeleteUserFromRole(string userName, string role);
        Task<List<BookLoan.Models.ManageUserViewModels.UserViewModel>> GetUsers();
        Task<BookLoan.Models.ManageUserViewModels.UserRoleViewModel> GetUserRoleDetailsById(string userId);
        Task<BookLoan.Models.ManageUserViewModels.UserRoleViewModel> GetUserRoleDetails(string userName);
        Task<BookLoan.Models.ManageUserViewModels.UserRoleConfirmAction> GetUserRoleAction(string userId, string action, string role);
    }
}
