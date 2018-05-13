using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CzadRoom.Controllers {
    public class AccountController : Controller {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IJwtToken _jwtToken;
        public AccountController(IUsersService usersService, IConfiguration configuration, ILogger logger, IJwtToken jwtToken) {
            _usersService = usersService;
            _configuration = configuration;
            _logger = logger;
            _jwtToken = jwtToken;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllUsers() {
            return new ObjectResult(await _usersService.GetUsers());
        }

        // GET: api/Users/username
        [HttpGet, Authorize]
        public async Task<IActionResult> Get(string name) {
            var user = await _usersService.GetUser(name);
            if (user == null)
                return new NotFoundResult();
            return new ObjectResult(user);
        }

        [HttpPost, AllowAnonymous]
        //TODO: change to viewmodel
        public async Task<IActionResult> Create([FromBody]User user) {
            var userDB = await _usersService.GetUser(user.Username);
            if (userDB != null) {
                return Json("username taken");
            }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt());
            await _usersService.Create(user);
            _logger.Log($"Created user: {user.Username}");
            return new OkObjectResult(user);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Update(string username, [FromBody]User user) {
            if (User.Identity.Name != username)
                return Unauthorized();
            var userDB = await _usersService.GetUser(username);
            if (userDB == null)
                return new NotFoundResult();
            user.Id = userDB.Id;
            await _usersService.Update(user);
            _logger.Log($"Updated user: {userDB.Username}");
            return new OkObjectResult(user);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(string username) {
            if (User.Identity.Name != username)
                return Unauthorized();
            var userDB = await _usersService.GetUser(username);
            if (userDB == null)
                return Json($"{username} doesnt exists");
            await _usersService.Delete(username);
            _logger.Log($"Deleted user: {userDB.Username}");
            return new OkResult();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]User user) {
            var userDB = await _usersService.GetUser(user.Username);
            if (userDB == null)
                return Json("user not found");
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userDB.Password)) {
                return Json("password mismatch");
            }
            var tokenString = _jwtToken.GenerateToken(userDB, DateTime.Now.AddMinutes(60));
            _logger.Log($"Login user: {userDB.Username}");
            return Ok(new { token = tokenString });
        }

        [AllowAnonymous]
        public IActionResult TestApi(int id) {
            return Json(id);
        }

        [Authorize]
        public IActionResult TestJwt() {
            var user = User.Identity as ClaimsIdentity;
            
            return Ok(User.Identity.Name);
        }
    }
}
