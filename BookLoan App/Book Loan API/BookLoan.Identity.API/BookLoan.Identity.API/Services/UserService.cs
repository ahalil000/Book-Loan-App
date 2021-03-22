using System;
using System.Collections.Generic;
using System.Linq;
using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Identity.API.Exceptions;
using BookLoan.Helpers;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BookLoan.Services
{
    public interface IUserService
    {
        bool Authenticate(string username, string password);
        string GetEmail(string userName);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser GetById(string id);
        Task<RegistrationResponseModel> Create(UserViewModel user, string password);
        Task<UpdateResponseModel> Update(UserViewModel user, string password = null);
        Task Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _db;
        readonly ILogger _logger;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate()
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new AppException("The username or password is empty.");

            var user = _db.Users.SingleOrDefault(x => x.UserName == username);

            // check if username exists
            if (user == null)
            {
                _logger.LogError($"The user {username} does not exist.");
                _logger.LogInformation($"The user {username} does not exist.");
                throw new AppException("The username is invalid.");
            }

            // check if password is correct          
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) 
                == PasswordVerificationResult.Failed)
            {
                _logger.LogInformation("The user password is invalid.");
                throw new AppException("The password is invalid.");
            }

            //if (!VerifyPasswordHash(password,
            //    System.Text.ASCIIEncoding.Default.GetBytes(user.PasswordHash, 20, 64),
            //    System.Text.ASCIIEncoding.Default.GetBytes(user.PasswordHash, 0, 20)))
            //    throw new AppException("The password is invalid.");

            _logger.LogInformation($"The user {username} has been successfully authenticated."); 

            // authentication successful
            return true;
        }

        /// <summary>
        /// GetEmail()
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetEmail(string userName)
        {
            var email = "";
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName);
            if (user != null)
                email = user.Email;
            return email;
        }

        /// <summary>
        /// GetAll()
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _db.Users;
        }

        /// <summary>
        /// GetById()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationUser GetById(string id)
        {
            return _db.Users.Where(u => u.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newuser"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<RegistrationResponseModel> Create(UserViewModel newuser, string password)
        {
            var user = new ApplicationUser
            {
                UserName = newuser.UserName,
                Email = newuser.Email,
                FirstName = newuser.FirstName,
                LastName = newuser.LastName,
                DOB = newuser.DOB
            };

            if ((newuser.UserName == null) || (newuser.UserName.Length == 0))
                newuser.UserName = newuser.Email;

            var responseStatus = new RegistrationResponseModel();
            var responseErrors = new List<RegistrationErrorResponseModel>();

            var result = await _userManager.CreateAsync(user, password);

            responseStatus.IsSuccessful = result.Succeeded;

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                //var code = 
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

                _logger.LogInformation("User created a new account with password.");

                // add additional claims for this new user..
                var newClaims = new List<Claim>() {
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.DateOfBirth, user.DOB.ToShortDateString())
                    };
                await _userManager.AddClaimsAsync(user, newClaims);

                responseStatus.userinfo.Id = user.Id;

                _logger.LogInformation("Claims added for user.");
            }
            else
            {
                foreach (IdentityError err in result.Errors)
                {
                    responseErrors.Add(new RegistrationErrorResponseModel()
                    {
                        ErrorCode = err.Code,
                        ErrorDescription = err.Description
                    });
                }
                responseStatus.errors = responseErrors;
            }
            return responseStatus;
        }

        /// <summary>
        /// Update()
        /// </summary>
        /// <param name="userView"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UpdateResponseModel> Update(UserViewModel userView, string password = null)
        {
            var responseStatus = new UpdateResponseModel();
            var responseErrors = new List<UpdateErrorResponseModel>();

            ApplicationUser user = _userManager.Users.Where(u => u.UserName == userView.UserName).SingleOrDefault();

            if (user == null)
            {
                throw new GeneralException($"Unable to load user with ID '{user.Id}'.");
            }

            var email = user.Email;
            if (userView.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, userView.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new GeneralException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (userView.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, userView.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new GeneralException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            // Flags for updated claims fields.
            bool bHasFNUpdated = false;
            bool bHasSNUpdated = false;
            bool bHasDOBUpdated = false;

            // custom field uodates
            if (userView.FirstName != user.FirstName)
            {
                user.FirstName = userView.FirstName;
                bHasFNUpdated = true;
            }

            if (userView.LastName != user.LastName)
            {
                user.LastName = userView.LastName;
                bHasSNUpdated = true;
            }

            if (userView.DOB != user.DOB)
            {
                user.DOB = userView.DOB;
                bHasDOBUpdated = true;
            }

            // add/update claims 
            bool bHasFNClaim = false;
            bool bHasSNClaim = false;
            bool bHasDOBClaim = false;

            dynamic existingFNClaim = "";
            dynamic existingSNClaim = "";
            dynamic existingDOBClaim = "";

            // check existing claims for user..
            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (Claim claim in userClaims)
            {
                if (claim.Type == ClaimTypes.GivenName)
                {
                    bHasFNClaim = true;
                    existingFNClaim = (Claim)claim;
                }
                if (claim.Type == ClaimTypes.Surname)
                {
                    bHasSNClaim = true;
                    existingSNClaim = (Claim)claim;
                }
                if (claim.Type == ClaimTypes.DateOfBirth)
                {
                    bHasDOBClaim = true;
                    existingDOBClaim = (Claim)claim;
                }
            }

            // add or update claims for user..
            var newFNClaim = new Claim(
                ClaimTypes.GivenName,
                user.FirstName
            );
            var newSNClaim = new Claim(
                ClaimTypes.Surname,
                user.LastName
            );
            var newDOBClaim = new Claim(
                ClaimTypes.DateOfBirth,
                user.DOB.ToShortDateString()
            );

            IdentityResult identityResult = IdentityResult.Success;

            if (!bHasFNClaim)
            {
                var rslt1 = await _userManager.AddClaimAsync(user, newFNClaim);
                if (!rslt1.Succeeded)
                {
                    foreach (IdentityError err in rslt1.Errors)
                    {
                        responseErrors.Add(new UpdateErrorResponseModel()
                        {
                            ErrorCode = err.Code,
                            ErrorDescription = err.Description
                        });
                    }
                }
            }
            else if (bHasFNUpdated)
            {
                if (existingFNClaim.GetType().ToString() == "System.Security.Claims.Claim")
                {
                    var rslt2 = await _userManager.RemoveClaimAsync(user, existingFNClaim);
                    if (rslt2.Succeeded)
                        await _userManager.AddClaimAsync(user, newFNClaim);
                    else
                    {
                        responseStatus.IsSuccessful = false;
                        foreach (IdentityError err in rslt2.Errors)
                        {
                            responseErrors.Add(new UpdateErrorResponseModel()
                            {
                                ErrorCode = err.Code,
                                ErrorDescription = err.Description
                            });
                        }
                    }
                }
            }
            if (!bHasSNClaim)
            {
                var rslt1 = await _userManager.AddClaimAsync(user, newSNClaim);
                if (!rslt1.Succeeded)
                {
                    responseStatus.IsSuccessful = false;
                    foreach (IdentityError err in rslt1.Errors)
                    {
                        responseErrors.Add(new UpdateErrorResponseModel()
                        {
                            ErrorCode = err.Code,
                            ErrorDescription = err.Description
                        });
                    }
                }
            }
            else if (bHasSNUpdated)
            {
                if (existingSNClaim.GetType().ToString() == "System.Security.Claims.Claim")
                {
                    var rslt2 = await _userManager.RemoveClaimAsync(user, existingSNClaim);
                    if (rslt2.Succeeded)
                        await _userManager.AddClaimAsync(user, newSNClaim);
                    else
                    {
                        responseStatus.IsSuccessful = false;
                        foreach (IdentityError err in rslt2.Errors)
                        {
                            responseErrors.Add(new UpdateErrorResponseModel()
                            {
                                ErrorCode = err.Code,
                                ErrorDescription = err.Description
                            });
                        }
                    }
                }
            }
            if (!bHasDOBClaim)
            {
                var rslt1 = await _userManager.AddClaimAsync(user, newDOBClaim);
                if (!rslt1.Succeeded)
                {
                    responseStatus.IsSuccessful = false;
                    foreach (IdentityError err in rslt1.Errors)
                    {
                        responseErrors.Add(new UpdateErrorResponseModel()
                        {
                            ErrorCode = err.Code,
                            ErrorDescription = err.Description
                        });
                    }
                }
            }
            else if (bHasDOBUpdated)
            {
                if (existingDOBClaim.GetType().ToString() == "System.Security.Claims.Claim")
                { 
                    try
                    {
                        var rslt2 = await _userManager.RemoveClaimAsync(user, existingDOBClaim);
                        if (rslt2.Succeeded)
                        {
                            var rslt3 = await _userManager.AddClaimAsync(user, newDOBClaim);
                            if (rslt3.Succeeded)
                            {
                                _logger.LogInformation("Claims added/updated successfully for user " + userView.UserName);
                            }
                            else
                            {
                                responseStatus.IsSuccessful = false;
                                foreach (IdentityError err in rslt3.Errors)
                                {
                                    responseErrors.Add(new UpdateErrorResponseModel()
                                    {
                                        ErrorCode = err.Code,
                                        ErrorDescription = err.Description
                                    });
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation("Cannot remove DOB claim for user " + userView.UserName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Cannot update DOB claims for user " + userView.UserName + " .Error = " + ex.Message.ToString());
                    }
                }
            }

            responseStatus.errors = responseErrors;
            responseStatus.IsSuccessful = (responseStatus.errors.Count() == 0);

            if (responseStatus.errors.Count() == 0)
            {
                _logger.LogInformation("Claims added/updated for user.");

                await _userManager.UpdateAsync(user);

                _logger.LogInformation("Profile for user " + userView.UserName + " has been updated.");

            }
            else
            {
                _logger.LogInformation("Profile for user " + userView.UserName + " CANNOT be updated.");
            }
            return responseStatus;
        }

        /// <summary>
        /// Delete()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            //get User Data from Userid
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new GeneralException($"Unable to load user with ID '{id}'.");
            }

            //List Logins associated with user
            var logins = _userManager.GetLoginsAsync(user);

            //Gets list of Roles associated with current user
            var rolesForUser = await _userManager.GetRolesAsync(user);

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new GeneralException($"Unexpected error occurred removing login for user with ID '{user.Id}'.");
            }
        }

        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash and salt (84 bytes expected).", "passwordHash");
            //if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}