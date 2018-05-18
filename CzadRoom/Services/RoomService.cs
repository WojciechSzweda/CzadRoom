using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services
{
    public class RoomService : IRoomService {

        private static readonly List<Room> _rooms = new List<Room>();

        public string CreateRoom(string name) {
            var room = new Room { Name = name };
            _rooms.Add(room);
            return room.ID;
        }

        public void DeleteRoom(string roomId) {
            var room = _rooms.FirstOrDefault(x => x.ID == roomId);
            if (room == null)
                return;
            _rooms.Remove(room);
        }
    }
}
