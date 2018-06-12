using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Comparers;
using CzadRoom.Extensions;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers {
    [Authorize]
    public class ChatController : Controller {
        private readonly IRoomService _roomService;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IChatMessageService _chatMessageService;
        private readonly IConnectionService _connectionService;

        public ChatController(IRoomService roomService, IUsersService usersService, IMapper mapper, IChatMessageService chatMessageService, IConnectionService connectionService) {
            _roomService = roomService;
            _usersService = usersService;
            _mapper = mapper;
            _chatMessageService = chatMessageService;
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index() {
            var rooms = (await _roomService.GetAll()).Select(x => _mapper.Map<ChatRoom, ChatRoomViewModel>(x, opt =>
            opt.AfterMap((src, dest) => dest.ClientCount = _connectionService.ConnectedUsersCount(src.ID))));
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(string roomId) {
            var roomDB = await _roomService.GetRoom(roomId);
            if (string.IsNullOrEmpty(roomDB.Password))
                return RedirectToAction("Room", new { roomId = roomDB.ID });

            if (roomDB.OwnerID == HttpContext.GetUserID())
                return RedirectToAction("Room", new { roomId = roomDB.ID });

            if (roomDB.UsersIDWithAccess.Contains(HttpContext.GetUserID()))
                return RedirectToAction("Room", new { roomId = roomDB.ID });
            var room = new ChatRoomJoinViewModel { Name = roomDB.Name, ID = roomDB.ID };
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> JoinRoom(ChatRoomJoinViewModel roomJoin) {
            var roomDB = await _roomService.GetRoom(roomJoin.ID);
            if (!ModelState.IsValid)
                return View(roomJoin);
            if (!string.IsNullOrEmpty(roomDB.Password) && string.IsNullOrEmpty(roomJoin.Password))
                return View(roomJoin);
            if (!BCrypt.Net.BCrypt.Verify(roomJoin.Password, roomDB.Password)) {
                return RedirectToAction("Index");
            }
            await _roomService.AddAccessedUserToRoom(roomDB.ID, HttpContext.GetUserID());
            return RedirectToAction("Room", new { roomId = roomJoin.ID });

        }

        public async Task<IActionResult> Room(string roomId) {
            var userID = HttpContext.GetUserID();
            if (!_roomService.HasUserAccess(roomId, userID))
                return RedirectToAction("JoinRoom", new { roomId });
            var roomDB = await _roomService.GetRoom(roomId);
            var users = (_roomService.ConnectedUsers(_connectionService.ConnectedUsersID(roomId), roomId)).Distinct(new UserComparer());
            var room = _mapper.Map<ChatRoom, ChatRoomViewModel>(roomDB, opt =>
                 opt.AfterMap((src, dest) => dest.UsersInRoom = users.Where(x => x.ID != userID).Select(x => _mapper.Map<UserViewModel>(x))));
            return View(room);
        }

        public IActionResult CreateRoom() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(ChatRoomCreateViewModel roomVM) {
            var room = new ChatRoom {
                Name = roomVM.Name,
                OwnerID = HttpContext.GetUserID()
            };
            if (!string.IsNullOrEmpty(roomVM.Password)) {
                room.Password = BCrypt.Net.BCrypt.HashPassword(roomVM.Password, BCrypt.Net.BCrypt.GenerateSalt());
            }
            var id = await _roomService.CreateRoom(room);

            return RedirectToAction("Room", new { roomId = id });
        }

        public struct MsgPost {
            public string roomId { get; set; }
            public int count { get; set; }
            public DateTime date { get; set; }
        }

        [HttpPost]
        public IActionResult GetMessages([FromBody]MsgPost body) {
            var userId = HttpContext.GetUserID();
            if (!_roomService.HasUserAccess(body.roomId, userId))
                return Unauthorized();
            var chatMessages = _chatMessageService.GetChatMessages(body.roomId, body.count).ToList();
            return Json(chatMessages);
        }
    }
}