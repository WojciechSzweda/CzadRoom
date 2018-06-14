using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;

namespace CzadRoom.Services.Interfaces
{
    public interface IDirectMessageRoomService
    {
        Task CreateDirectMessageRoom(DirectMessageRoom directMessageRoom);
        Task<DirectMessageRoom> GetDirectMessageRoom(string roomId);
        Task<DirectMessageRoom> GetDirectMessageRoomWithFriend(string userId, string friendId);
        Task<IEnumerable<DirectMessageRoom>> GetUserDirectMessageRooms(string userId);
        bool HasUserAccess(string roomId, string userId);
    }
}
