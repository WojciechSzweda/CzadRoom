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
        private readonly IUsersService _usersRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public AccountController(IUsersService usersRepository, IConfiguration configuration, ILogger logger) {
            _usersRepository = usersRepository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllUsers() {
            return new ObjectResult(await _usersRepository.GetUsers());
        }

        // GET: api/Users/username
        [HttpGet, Authorize]
        public async Task<IActionResult> Get(string name) {
            var user = await _usersRepository.GetUser(name);
            if (user == null)
                return new NotFoundResult();
            return new ObjectResult(user);
        }

        [HttpPost, AllowAnonymous]
        //TODO: change to viewmodel
        public async Task<IActionResult> Create([FromBody]User user) {
            var userDB = await _usersRepository.GetUser(user.Username);
            if (user != null) {
                return Json("username taken");
            }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt());
            await _usersRepository.Create(user);
            _logger.Log($"Created user: {user.Username}");
            return new OkObjectResult(user);
        }

        [AllowAnonymous]
        public IActionResult TestApi(int id) {
            return Json(id);
        }

        [Authorize]
        public IActionResult TestJwt() {
            var user = User.Identity as ClaimsIdentity;
            return Ok();
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Update(string username, [FromBody]User user) {
            var userDB = await _usersRepository.GetUser(username);
            if (userDB == null)
                return new NotFoundResult();
            user.Id = userDB.Id;
            await _usersRepository.Update(user);
            _logger.Log($"Updated user: {userDB.Username}");
            return new OkObjectResult(user);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(string username) {
            var userDB = await _usersRepository.GetUser(username);
            if (userDB == null)
                return Json($"{username} doesnt exists");
            await _usersRepository.Delete(username);
            _logger.Log($"Deleted user: {userDB.Username}");
            return new OkResult();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]User user) {
            var userDB = await _usersRepository.GetUser(user.Username);
            if (userDB == null)
                return Json("user not found");
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userDB.Password)) {
                return Json("password mismatch");
            }
            var tokenString = GenerateToken(userDB);
            _logger.Log($"Login user: {userDB.Username}");
            return Ok(new { token = tokenString });
        }

        private string GenerateToken(User user) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
         _configuration["Jwt:Issuer"],
         expires: DateTime.Now.AddMinutes(60),
         signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
