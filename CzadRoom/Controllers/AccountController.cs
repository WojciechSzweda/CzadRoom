using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CzadRoom.Attributes;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CzadRoom.Controllers {
    public class AccountController : Controller {
        private readonly IUsersService _usersService;
        private readonly ILogger _logger;
        public AccountController(IUsersService usersService, ILogger logger) {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register() {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userVM) {
            if (!ModelState.IsValid) {
                return View();
            }
            var userDB = await _usersService.GetUserByName(userVM.Username);
            if (userDB != null) {
                ViewData["Error"] = "Username already taken";
                return View();
            }
            var checkEmailUser = await _usersService.GetUserByEmail(userVM.Email);
            if (checkEmailUser != null) {
                ViewData["Error"] = "Email already in use";
                return View();
            }
            //TODO: implement mapper
            //TODO: nickname creator
            var user = new User { Email = userVM.Email, Nickname = userVM.Nickname, Username = userVM.Username };
            user.Password = BCrypt.Net.BCrypt.HashPassword(userVM.Password, BCrypt.Net.BCrypt.GenerateSalt());
            await _usersService.Create(user);
            _logger.Log($"Created user: {user.Username}");
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null) {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel user, string returnUrl = null) {

            var userDB = await _usersService.GetUserByName(user.Username);
            if (userDB == null) {
                ViewData["Error"] = true;
                return View();
            }
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userDB.Password)) {
                return Json("password mismatch");
            }
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreateClaimsPrincipal(userDB));
            _logger.Log($"Login user: {userDB.Username}");
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal CreateClaimsPrincipal(User user) {
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.ID)
            };
            var userIdentity = new ClaimsIdentity(claims, "login");
            return new ClaimsPrincipal(userIdentity);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailUnique(string email) {
            var userDB = await _usersService.GetUserByEmail(email);
            return Json(userDB == null);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserNameUnique(string username) {
            var userDB = await _usersService.GetUserByName(username);
            return Json(userDB == null);
        }

        public IActionResult Test(int id) {
            return Json(id);
        }
    }
}
