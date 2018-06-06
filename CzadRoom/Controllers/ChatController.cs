﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Comparers;
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

        public ChatController(IRoomService roomService, IUsersService usersService, IMapper mapper, IChatMessageService chatMessageService) {
            _roomService = roomService;
            _usersService = usersService;
            _mapper = mapper;
            _chatMessageService = chatMessageService;
        }

        public async Task<IActionResult> Index() {
            var rooms = (await _roomService.GetAll()).Select(x => _mapper.Map<Room, RoomViewModel>(x, opt =>
            opt.AfterMap(async (src, dest) => dest.ClientCount = (await _roomService.ConnectedUsers(src.ID)).Distinct(new UserComparer()).Count())));
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(string roomId) {
            var roomDB = await _roomService.GetRoom(roomId);
            if (string.IsNullOrEmpty(roomDB.Password))
                return RedirectToAction("Room", new { roomId = roomDB.ID });

            if (roomDB.OwnerID == GetUserIdFromHttpContext())
                return RedirectToAction("Room", new { roomId = roomDB.ID });

            if (roomDB.UsersIDWithAccess.Contains(GetUserIdFromHttpContext()))
                return RedirectToAction("Room", new { roomId = roomDB.ID });
            var room = new RoomJoinViewModel { Name = roomDB.Name, ID = roomDB.ID };
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> JoinRoom(RoomJoinViewModel roomJoin) {
            var roomDB = await _roomService.GetRoom(roomJoin.ID);
            if (!ModelState.IsValid)
                return View(roomJoin);
            if (!string.IsNullOrEmpty(roomDB.Password) && string.IsNullOrEmpty(roomJoin.Password))
                return View(roomJoin);
            if (!BCrypt.Net.BCrypt.Verify(roomJoin.Password, roomDB.Password)) {
                return RedirectToAction("Index");
            }
            await _roomService.AddAccessedUserToRoom(roomDB.ID, GetUserIdFromHttpContext());
            return RedirectToAction("Room", new { roomId = roomJoin.ID });

        }

        public async Task<IActionResult> Room(string roomId) {
            var userID = GetUserIdFromHttpContext();
            if (!_roomService.HasUserAccess(roomId, userID))
                return RedirectToAction("JoinRoom", new { roomId });
            var roomDB = await _roomService.GetRoom(roomId);
            var users = (await _roomService.ConnectedUsers(roomId)).Distinct(new UserComparer());
            var room = _mapper.Map<Room, RoomViewModel>(roomDB, opt =>
                 opt.AfterMap((src, dest) => dest.UsersInRoom = users.Where(x => x.ID != userID).Select(x => _mapper.Map<UserViewModel>(x))));
            return View(room);
        }

        public IActionResult CreateRoom() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomCreateViewModel roomVM) {
            var room = new Room {
                Name = roomVM.Name,
                OwnerID = GetUserIdFromHttpContext()
            };
            if (!string.IsNullOrEmpty(roomVM.Password)) {
                room.Password = BCrypt.Net.BCrypt.HashPassword(roomVM.Password, BCrypt.Net.BCrypt.GenerateSalt());
            }
            var id = await _roomService.CreateRoom(room);

            return RedirectToAction("Room", new { roomId = id });
        }

        public string GetUserIdFromHttpContext() {
            return HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public struct MsgPost {
            public string roomId { get; set; }
            public int count { get; set; }
            public DateTime date { get; set; }
        }

        [HttpPost]
        public IActionResult GetMessages([FromBody]MsgPost body) {
            var userId = GetUserIdFromHttpContext();
            if (!_roomService.HasUserAccess(body.roomId, userId))
                return Unauthorized();
            var chatMessages = _chatMessageService.GetChatMessages(body.roomId, body.count).ToList();
            return Json(chatMessages);
        }
    }
}