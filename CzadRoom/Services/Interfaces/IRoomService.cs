using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces {
    public interface IRoomService {
        Task<IEnumerable<Room>> GetAll();
        Task<Room> GetRoom(string roomId);
        Task<string> CreateRoom(Room room);
        Task<bool> UpdateRoom(Room room);
        Task<bool> DeleteRoom(string roomId);
        Task AppendMessage(string roomId, string message);
        Task<bool> AddAccessedUserToRoom(string roomId, string userId);
        bool HasUserAccess(string roomId, string userId);

        Task UserConnected(RoomConnection roomConnection);
        Task<string> UserDisconnected(string connectionId);
        Task RemoveAllConnections();
        Task<int> ConnectedUsersCount(string roomId);
        Task<IEnumerable<string>> ConnectedUsersID(string roomId);
        Task<IEnumerable<User>> ConnectedUsers(string roomId);
    }
}
