using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Extensions;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IDirectMessageRoomService _directMessageRoomService;
        private readonly IDirectMessageService _directMessageService;

        public CommunityController(IUsersService usersService, IMapper mapper, 
            IDirectMessageRoomService directMessageRoomService, IDirectMessageService  directMessageService) {
            _usersService = usersService;
            _mapper = mapper;
            _directMessageRoomService = directMessageRoomService;
            _directMessageService = directMessageService;
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

        public async Task<IActionResult> DirectMessage(string friendId) {
            var userID = HttpContext.GetUserID();
            var room = await _directMessageRoomService.GetDirectMessageRoomWithFriend(userID, friendId);
            if (room == null) {
                room = new DirectMessageRoom(userID, friendId);
                await _directMessageRoomService.CreateDirectMessageRoom(room);
            }
            var roomVM = _mapper.Map<DirectMessageRoomViewModel>(room);
            roomVM.Users = room.Users.Select(id => _mapper.Map<UserViewModel>(_usersService.GetUser(id).Result));
            return View(roomVM);
        }

        public struct MsgPost {
            public string RoomId { get; set; }
            public int Count { get; set; }
            public DateTime Date { get; set; }
        }

        [HttpPost]
        public IActionResult GetMessages([FromBody]MsgPost body) {
            var userId = HttpContext.GetUserID();
            if (!_directMessageRoomService.HasUserAccess(body.RoomId, userId))
                return Unauthorized();
            var directMessages = _directMessageService.GetDirectMessages(body.RoomId, userId, body.Count);
            var directMessagesVM = ConvertDMsToViewModel(directMessages).ToList();
            return Json(directMessagesVM);
        }

        IEnumerable<DirectMessageViewModel> ConvertDMsToViewModel(IEnumerable<DirectMessage> directMessages) {
            var users = directMessages.Select(x => x.FromID).Distinct().ToDictionary(x => x, x => _mapper.Map<UserViewModel>(_usersService.GetUser(x).Result));
            return directMessages.Select(dm => _mapper.Map<DirectMessage, DirectMessageViewModel>(dm, opt =>
            opt.AfterMap((src, dest) => dest.From = users[src.FromID])));
        }
    }
}