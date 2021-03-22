using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookLoan.Models;
using BookLoan.Services;

namespace BookLoan.Data
{
    public class SeedAccounts
    {

        private ApplicationDbContext db;
        private AppConfiguration appConfiguration;

        public SeedAccounts(ApplicationDbContext _db, AppConfiguration _appConfiguration)
        {
            db = _db;
            appConfiguration = _appConfiguration;
        }

        // In this method we will create default User roles and Admin user for login   
        public async Task GenerateUserAccounts()
        {
            var userManager = new UserStore<ApplicationUser>(db);
            var roleManager = new RoleStore<IdentityRole>(db);

            // Create Admin Role and Admin User    
            if (!roleManager.Roles.Where(a => a.Name == "Admin").Any())
            {
                // first we create Admin role 
                var role = new Microsoft.AspNetCore.Identity.IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = role.Name.ToUpper();
                await roleManager.CreateAsync(role);
            }

            // creating Member role    
            if (!roleManager.Roles.Where(a => a.Name == "Manager").Any())
            {
                var role = new Microsoft.AspNetCore.Identity.IdentityRole();
                role.Name = "Manager";
                role.NormalizedName = role.Name.ToUpper();
                await roleManager.CreateAsync(role);
            }

            // create Member role    
            if (!roleManager.Roles.Where(a => a.Name == "Member").Any())
            {
                var role = new Microsoft.AspNetCore.Identity.IdentityRole();
                role.Name = "Member";
                role.NormalizedName = role.Name.ToUpper();
                await roleManager.CreateAsync(role);
            }


            // create Admin user
            if (!userManager.Users.Where(a => a.UserName == appConfiguration.AdminEmail).Any())
            {
                try
                {
                    // Create a Admin super user who will maintain the website
                    var user = new ApplicationUser();
                    user.UserName = appConfiguration.AdminEmail;
                    user.Email = appConfiguration.AdminEmail;
                    user.NormalizedEmail = appConfiguration.AdminEmail.ToUpper();
                    user.NormalizedUserName = appConfiguration.AdminEmail.ToUpper();
                    user.SecurityStamp = Guid.NewGuid().ToString("D");
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, appConfiguration.AdminPwd);
                    user.PasswordHash = hashed;
                    var chkUser = await userManager.CreateAsync(user);

                    //Add default User to Role Admin   
                    if (chkUser.Succeeded)
                    {
                        var adminRole = roleManager.FindByNameAsync("Admin");
                        var adminUser = userManager.FindByEmailAsync(appConfiguration.AdminEmail);
                        if (adminRole != null && adminUser != null)
                        {
                            db.UserRoles.Add(new IdentityUserRole<string>()
                            {
                                RoleId = adminRole.Result.Id.ToString(),
                                UserId = adminUser.Result.Id.ToString()
                            });
                            await db.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("Error " + ex.Message.ToString());
                }
            }
        }
    }
}
