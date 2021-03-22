using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Threading.Tasks;

using BookLoan.Models;
using BookLoan.Services;
using BookLoan.Helpers;

//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.Extensions.Options;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Claims;

namespace WebApi.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private TokenManager _tokenManager;

        public UsersController(
            IUserService userService,
            TokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [AllowAnonymous]
        [HttpPost("api/[controller]/token")]
        public IActionResult Token([FromBody]UserViewModel userDto)
        {
            try
            {
                IActionResult authenticatedResult = this.Authenticate(userDto);

                // return basic user info (without password) and token to store client side
                return new OkObjectResult(authenticatedResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [AllowAnonymous]
        [HttpPost("api/[controller]/authenticate")]
        public IActionResult Authenticate([FromBody]UserViewModel userDto)
        {
            try
            {
                bool isAuthenticated = _userService.Authenticate(userDto.UserName, userDto.Password);

                if (!isAuthenticated)
                    return BadRequest(new { message = "Username or password is incorrect" });

                string email = _userService.GetEmail(userDto.UserName);

                string tokenString = _tokenManager.GenerateToken(userDto.UserName, email);

                // return basic user info (without password) and token to store client side
                return Ok(new
                {
                    Username = userDto.UserName,
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }

        [AllowAnonymous]
        [HttpPost("api/[controller]/register")]
        public async Task<IActionResult> Register([FromBody]UserViewModel userDto)
        {
            try 
            {
                // save 
                var response = await _userService.Create(userDto, userDto.Password);
                if (response.IsSuccessful) 
                    return Ok(response);
                return BadRequest(response);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("api/[controller]/GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            //var userDtos =  _mapper.Map<IList<UserDto>>(users);
            List<UserViewModel> userViews = new List<UserViewModel>(); 
            foreach (ApplicationUser user in users)
            {
                userViews.Add(new UserViewModel()
                {
                    UserName = user.UserName,
                    DOB = user.DOB,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            return Ok(userViews);
        }

        [HttpGet("api/[controller]/{id}")]
        public IActionResult GetById(string id)
        {
            var user =  _userService.GetById(id);
            UserViewModel userView = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DOB = user.DOB
            };
            return Ok(userView);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]UserViewModel userView)
        {
            userView.Id = id.ToString();

            try 
            {
                var response = await _userService.Update(userView, userView.Password);
                if (response.IsSuccessful)
                    return Ok(response);
                return BadRequest(response);
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("api/[controller]/{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
