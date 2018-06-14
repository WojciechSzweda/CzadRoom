using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;

namespace CzadRoom.Services.Interfaces
{
    public interface IPublicRoomService
    {
        Task<IEnumerable<PublicRoom>> GetAll();
        Task<PublicRoom> GetRoom(string roomId);
        Task<string> CreateRoom(PublicRoom room);
    }
}
