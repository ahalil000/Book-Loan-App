using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Services
{
    public class UserRoleService: IUserRoleService
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;

        public UserRoleService(
            ApplicationDbContext _db, 
            UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }


        /// <summary>
        /// GetAllRoles()
        /// Return all available roles
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRoles()
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var allRoles = roleManager.Roles.ToList();
            List<string> stringRoles = new List<string>();
            foreach (IdentityRole item in allRoles)
            {
                stringRoles.Add(item.Name);
            }
            return stringRoles;
        }


        /// <summary>
        /// GetUserRoles()
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<BookLoan.Models.ManageUserViewModels.UserViewModel>> GetUsers()
        {
            List<BookLoan.Models.ManageUserViewModels.UserViewModel> userViews = new List<Models.ManageUserViewModels.UserViewModel>();
            userManager.Users.ToList().ForEach(async u =>
            {
                Task<bool> isInRoleResult = userManager.IsInRoleAsync(u, "Admin");
                bool isInRole = isInRoleResult.GetAwaiter().GetResult();
                if (isInRole == false)
                    userViews.Add(new Models.ManageUserViewModels.UserViewModel()
                    {
                        ID = u.Id,
                        UserName = u.UserName,
                        Email = u.Email
                    });
            });
            return userViews;
        }


        /// <summary>
        /// GetUserRoles()
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserRoles(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles != null)
                return roles.ToList();
            return new List<string>();
        }


        /// <summary>
        /// GetUserRoleDetails
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<BookLoan.Models.ManageUserViewModels.UserRoleViewModel> GetUserRoleDetails(string userName)
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var user = await userManager.FindByNameAsync(userName);
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.ToList();
            List<string> stringRoles = new List<string>();
            foreach (IdentityRole item in allRoles)
            {
                stringRoles.Add(item.Name);
            }
            return new Models.ManageUserViewModels.UserRoleViewModel()
            {
                DisplayName = user.UserName,
                LoginName = user.Email,
                UserID = user.Id,
                UserRoles = userRoles.ToList(),
                AvailableRoles = stringRoles
            };
        }


        /// <summary>
        /// GetUserRoleDetailsById
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BookLoan.Models.ManageUserViewModels.UserRoleViewModel> GetUserRoleDetailsById(string userId)
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var user = await userManager.FindByIdAsync(userId);
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.ToList();
            List<string> stringRoles = new List<string>();
            foreach (IdentityRole item in allRoles)
            {
                stringRoles.Add(item.Name);
            }
            return new Models.ManageUserViewModels.UserRoleViewModel()
            {
                DisplayName = user.UserName,
                LoginName = user.Email,
                UserID = user.Id,
                UserRoles = userRoles.ToList(),
                AvailableRoles = stringRoles
            };
        }


        /// <summary>
        /// IsUserInRole()
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> IsUserInRole(string userName, string role)
        {
            var user = await userManager.FindByEmailAsync(userName);
            bool isInRole = await userManager.IsInRoleAsync(user, role);
            return isInRole;
        }


        /// <summary>
        /// AddUserToRole()
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task AddUserToRole(string userName, string role)
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var user = await userManager.FindByEmailAsync(userName);
            bool isInRole = await this.IsUserInRole(userName, role); // await userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                if ((role == "Member") || (role == "Manager"))
                {
                    var memberRole = roleManager.FindByNameAsync(role);
                    var memberUser = userManager.FindByEmailAsync(userName);
                    if (memberRole != null && memberUser != null)
                    {
                        db.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            RoleId = memberRole.Result.Id.ToString(),
                            UserId = memberUser.Result.Id.ToString()
                        });
                        await db.SaveChangesAsync();
                    }
                }
                //await userManager.AddToRoleAsync(user, role.ToUpper());
            }
        }


        /// <summary>
        /// DeleteUserFromRole
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task DeleteUserFromRole(string userName, string role)
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var user = await userManager.FindByEmailAsync(userName);
            bool isInRole = await this.IsUserInRole(userName, role); // await userManager.IsInRoleAsync(user, role);
            if (isInRole)
            {
                if ((role == "Member") || (role == "Manager"))
                {
                    var memberRole = roleManager.FindByNameAsync(role);
                    var memberUser = userManager.FindByEmailAsync(userName);
                    if (memberRole != null && memberUser != null)
                    {
                        var currUserRole = db.UserRoles.Where(
                                    r => r.UserId == memberUser.Result.Id &&
                                         r.RoleId == memberRole.Result.Id).SingleOrDefault();
                        if (currUserRole != null)
                        {
                            db.UserRoles.Remove(currUserRole);
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// GetUserRoleAction
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<BookLoan.Models.ManageUserViewModels.UserRoleConfirmAction> GetUserRoleAction(string userId, string action, string role)
        {
            var roleManager = new RoleStore<IdentityRole>(db);
            var user = await userManager.FindByIdAsync(userId);
            return new Models.ManageUserViewModels.UserRoleConfirmAction()
            {
                Action = action,
                SelectedRole = role,
                LoginName = user.Email,
                UserID = user.Id
            };
        }
    }
}
