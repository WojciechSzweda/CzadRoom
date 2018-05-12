using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;
using CzadRoom.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers {
    public class AccountController : Controller {
        private readonly IUsersRepository _usersRepository;
        public AccountController(IUsersRepository usersRepository) {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {
            return new ObjectResult(await _usersRepository.GetUsers());
        }

        // GET: api/Users/username
        [HttpGet]
        public async Task<IActionResult> Get(string name) {
            var user = await _usersRepository.GetUser(name);
            if (user == null)
                return new NotFoundResult();
            return new ObjectResult(user);
        }

        [HttpPost]
        //TODO: change to viewmodel
        public async Task<IActionResult> Create([FromBody]User user) {
            await _usersRepository.Create(user);
            return new OkObjectResult(user);
        }

        public IActionResult Test(int id) {
            return Json(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string username, [FromBody]User user) {
            var userDB = await _usersRepository.GetUser(username);
            if (userDB == null)
                return new NotFoundResult();
            user.Id = userDB.Id;
            await _usersRepository.Update(user);
            return new OkObjectResult(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string username) {
            var userDB = await _usersRepository.GetUser(username);
            if (userDB == null)
                return Json($"{username} doesnt exists");
            await _usersRepository.Delete(username);
            return new OkResult();
        }
    }
}
