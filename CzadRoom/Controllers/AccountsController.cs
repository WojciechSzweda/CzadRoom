using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CzadRoom.Attributes;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CzadRoom.Controllers
{
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IJwtToken _jwtToken;
        public AccountsController(IUsersService usersService, IJwtToken jwtToken) {
            _usersService = usersService;
            _jwtToken = jwtToken;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() {
            return new ObjectResult(await _usersService.GetUsers());
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailUnique(string Email) {
            return Json(true);
        }

        [HttpGet, JwtAuthorize]
        [Route("Get")]
        public async Task<IActionResult> Get(string name) {
            var user = await _usersService.GetUserByName(name);
            if (user == null)
                return new NotFoundResult();
            return new ObjectResult(user);
        }

        [HttpPut, JwtAuthorize]
        [Route("Update")]
        public async Task<IActionResult> Update(string username, [FromBody]User user) {
            if (User.Identity.Name != username)
                return Unauthorized();
            var userDB = await _usersService.GetUserByName(username);
            if (userDB == null)
                return new NotFoundResult();
            user.ID = userDB.ID;
            await _usersService.Update(user);
            return new OkObjectResult(user);
        }

        [HttpDelete, JwtAuthorize]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string username) {
            if (User.Identity.Name != username)
                return Unauthorized();
            var userDB = await _usersService.GetUserByName(username);
            if (userDB == null)
                return Json($"{username} doesnt exists");
            await _usersService.Delete(username);
            return new OkResult();
        }

        [JwtAuthorize]
        [Route("TestJwt")]
        public IActionResult TestJwt() {
            var user = User.Identity as ClaimsIdentity;

            return Ok(User.Identity.Name);
        }

        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody]UserLoginViewModel user) {
            var userDB = await _usersService.GetUserByName(user.Username);
            if (userDB == null)
                return View();
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userDB.Password)) {
                return Json("password mismatch");
            }

            var token = _jwtToken.GenerateToken(userDB, DateTime.Now.AddMinutes(60));
            return new ObjectResult(new { token });
        }
    }
}