using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Extensions;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public CommunityController(IRoomService roomService, IUsersService usersService, IMapper mapper) {
            _roomService = roomService;
            _usersService = usersService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var friends = (await _usersService.GetFriends(HttpContext.GetUserID())).Select(user => _mapper.Map<UserViewModel>(user));
            return View(friends);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend([FromBody]UserViewModel friend) {
            var result = await _usersService.AddFriend(HttpContext.GetUserID(), friend.ID);
            if (result)
                return Ok();
            return BadRequest();
        }
    }
}