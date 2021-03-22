using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using BookLoan.Data;
using BookLoan.Services;
using BookLoan.Models;


namespace BookLoan.Controllers
{
    public class ManageUserController : Controller
    {
        //ApplicationDbContext _db;
        IUserRoleService _userRoleService;

        //public ManageUserController(ApplicationDbContext db, IUserRoleService userRoleService)
        public ManageUserController(IUserRoleService userRoleService)
        {
            //_db = db;
            _userRoleService = userRoleService;
        }

        // GET: ManageUsers
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageUsers/UserList
        [HttpGet("api/[controller]/UserList")]
        public async Task<ActionResult> UserList()
        {
            return Ok(await _userRoleService.GetUsers());
        }


        // GET: ManageUsers/RoleList
        [HttpGet("api/[controller]/RoleList")]
        public async Task<ActionResult> RoleList()
        {
            List<string> roles = new List<string>();
            try
            {
                roles = _userRoleService.GetAllRoles();
                return Ok(roles);
            }
            catch
            {
                return BadRequest();
            }
        }


        // GET: ManageUsers/UserRoleEdit/5
        [HttpGet("api/[controller]/UserRoleEdit/{id}")]
        public async Task<ActionResult> UserRoleEdit(string id)
        {
            var userrole = await _userRoleService.GetUserRoleDetailsById(id);
            return Ok(userrole);
        }


        /// <summary>
        /// AddRole()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("api/[controller]/GetAddRole/{id}/{role}")]
        public async Task<ActionResult> GetAddRole(string id, string role)
        {
            var userrole = await _userRoleService.GetUserRoleAction(id, "Add", role);
            return Ok(userrole);
        }


        /// <summary>
        /// DeleteRole()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("api/[controller]/DeleteRole/{id}/{role}")]
        public async Task<ActionResult> DeleteRole(string id, string role)
        {
            var userrole = await _userRoleService.GetUserRoleAction(id, "Delete", role);
            return Ok(userrole);
        }



        // POST: ManageUsers/AddRole/5
        //[ValidateAntiForgeryToken]
        [HttpPost("api/[controller]/AddRole")]
        //[HttpPost("api/[controller]/AddRole/{UserID}/{SelectedRole}")]
        public async Task<ActionResult> AddRole([FromBody]Models.ManageUserViewModels.UserRoleConfirmAction model)
        {
            try
            {
                // TODO: Add update logic here
                //if (Request.Form["Confirm"].ToString() == "Confirm")
                //{
                await _userRoleService.AddUserToRole(model.LoginName, model.SelectedRole);
                return Ok();
                //return RedirectToAction("UserRoleEdit", "ManageUser", new { id = model.UserID });
                //}
                //else
                //    return RedirectToAction("UserList", "ManageUser");
            }
            catch
            {
                return BadRequest();
                //return RedirectToAction("Index", "Home");
            }
        }


        // POST: ManageUsers/DeleteRole/5
        //[ValidateAntiForgeryToken]
        [HttpPost("api/[controller]/DeleteRole")]
        ///{UserID}/{SelectedRole}")]
        public async Task<ActionResult> DeleteRole([FromBody]Models.ManageUserViewModels.UserRoleConfirmAction model)
        {
            try
            {
                // TODO: Add update logic here
                //if (Request.Form["Confirm"].ToString() == "Confirm")
                //{
                await _userRoleService.DeleteUserFromRole(model.LoginName, model.SelectedRole);
                return Ok();
                //return RedirectToAction("UserRoleEdit", new { id = model.UserID });
                //}
                //else
                //    return RedirectToAction("UserList", "ManageUser");
            }
            catch
            {
                return BadRequest();
                //return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet("api/[controller]/GetUserRoles/{userName}")]
        public async Task<ActionResult> GetUserRoles(string userName)
        {
            List<string> userRoles = new List<string>();
            try
            {
                userRoles = await _userRoleService.GetUserRoles(userName);
                return Ok(userRoles);
            }
            catch
            {
                return BadRequest();
        //return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet("api/[controller]/GetUserRoleDetails/{userName}")]
        public async Task<ActionResult> GetUserRoleDetails(string userName)
        {
            try
            {
                BookLoan.Models.ManageUserViewModels.UserRoleViewModel userRoleViewModel = 
                    new Models.ManageUserViewModels.UserRoleViewModel();
                userRoleViewModel = await _userRoleService.GetUserRoleDetails(userName);
                return Ok(userRoleViewModel);
            }
            catch
            {
                return BadRequest();
                //return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet("api/[controller]/IsUserInRole/{userName}/{role}")]
        public async Task<IActionResult> IsUserInRole(string userName, string role)
        {
            try
            {
                bool rslt = await _userRoleService.IsUserInRole(userName, role);
                return Ok(rslt);
            }
            catch
            {
                return BadRequest();
            }

            //var user = await userManager.FindByEmailAsync(userName);
            //bool isInRole = await userManager.IsInRoleAsync(user, role);
            //return isInRole;
        }


    // GET: ManageUsers/Create
    //[HttpGet("api/[controller]/Create")]
    //public ActionResult Create()
    //{
    //    return View();
    //}

        // POST: ManageUsers/Create
        //[ValidateAntiForgeryToken]
        //[HttpPost("api/[controller]/Create")]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ManageUsers/Edit/5
        //[HttpGet("api/[controller]/Edit/{id}")]
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: ManageUsers/Edit/5
        //[ValidateAntiForgeryToken]
        //[HttpPost("api/[controller]/Edit/{id}")]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ManageUsers/Delete/5        
        //[HttpGet("api/[controller]/Delete/{id}")]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: ManageUsers/Delete/5
        //[ValidateAntiForgeryToken]
        //[HttpPost("api/[controller]/Delete/{id}")]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}