using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IRoomService _roomService;

        public ChatController(IRoomService roomService) {
            _roomService = roomService;
        }

        //List of rooms, create room
        public async Task<IActionResult> Index()
        {
            
            var rooms =  (await _roomService.GetAll()).Select(x => new RoomViewModel { RoomID = x.ID, Name = x.Name });
            return View(rooms);
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
            var id = await _roomService.CreateRoom(new Room { Name = roomVM.Name, Password = roomVM.Password });

            return RedirectToAction("Room", new { roomId = id });
        }

    }
}