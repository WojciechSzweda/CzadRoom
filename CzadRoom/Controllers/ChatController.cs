using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CzadRoom.Controllers
{
    public class ChatController : Controller
    {

        public ChatController() {

        }

        public IActionResult Index()
        {
            return View();
        }

        //List of rooms, create room
        public IActionResult Room(string roomId) {
            return View();
        }

        public IActionResult CreateRoom() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRoom(string name, string password) {

            return View();
        }

    }
}