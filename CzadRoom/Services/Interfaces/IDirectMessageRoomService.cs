using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;

namespace CzadRoom.Services.Interfaces
{
    public interface IDirectMessageRoomService
    {
        Task<DirectMessageRoom> GetRoomWithFriend(string userId, string friendId);
        Task<IEnumerable<DirectMessageRoom>> GetUserRooms(string userId);
        bool HasUserAccess(string roomId, string userId);
    }
}
