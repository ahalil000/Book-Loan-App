using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using BookLoan.Services;
using System.Threading.Tasks;

namespace BookLoan.Helpers
{
    public class TokenManager
    {
        private AppConfiguration _appConfiguration;
        private IUserRoleService _userRoleService;

        //private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public TokenManager(
            IOptionsSnapshot<AppConfiguration> appConfiguration,
            IUserRoleService userRoleService)
        {
            _appConfiguration = appConfiguration.Value;
            _userRoleService = userRoleService;
        }

        public string GenerateToken(string username, string email = "")
        {
            byte[] key = Convert.FromBase64String(_appConfiguration.Secret);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
        
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // build claims user roles
            Task<List<string>> userRoles; 

            var claims = new List<Claim>();

            if (username == email)
                userRoles = _userRoleService.GetUserRoles(username); 
            else 
                userRoles = _userRoleService.GetUserRoles(email);

            foreach (var userRole in userRoles.GetAwaiter().GetResult())
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            descriptor.Subject.AddClaims(claims);

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return username;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(_appConfiguration.Secret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}