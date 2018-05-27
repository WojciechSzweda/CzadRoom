using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public ChatController(IRoomService roomService, IUsersService usersService) {
            _roomService = roomService;
            _usersService = usersService;
        }

        //List of rooms, create room
        public async Task<IActionResult> Index() {
            var rooms = (await _roomService.GetAll()).Select(x => new RoomViewModel {
                RoomID = x.ID,
                Name = x.Name,
                HasPassword = !string.IsNullOrEmpty(x.Password) });
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(string roomId) {
            var roomDB = await _roomService.GetRoom(roomId);
            if (string.IsNullOrEmpty(roomDB.Password))
                return RedirectToAction("Room", new { roomId = roomDB.ID });
            if (roomDB.OwnerID == GetUserIdFromHttpContext())
                return RedirectToAction("Room", new { roomId = roomDB.ID });
            var room = new RoomJoinViewModel { Name = roomDB.Name, ID = roomDB.ID };
            return View(room);
        }

        [HttpPost]
        public IActionResult JoinRoom(RoomJoinViewModel roomJoin) {
            
            return RedirectToAction("Room", new { roomId = roomJoin.ID });
        }

        public async Task<IActionResult> Room(string roomId) {
            var roomDB = await _roomService.GetRoom(roomId);
            var room = new RoomViewModel { Name = roomDB.Name, RoomID = roomDB.ID };
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

    }
}