using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces {
    public interface IRoomService {
        Task<IEnumerable<ChatRoom>> GetAll();
        Task<ChatRoom> GetRoom(string roomId);
        Task<string> CreateRoom(ChatRoom room);
        Task<bool> UpdateRoom(ChatRoom room);
        Task<bool> DeleteRoom(string roomId);
        Task AppendMessage(string roomId, string message);
        Task<bool> AddAccessedUserToRoom(string roomId, string userId);
        bool HasUserAccess(string roomId, string userId);

        IEnumerable<User> ConnectedUsers(IEnumerable<string> usersId,string roomId);
    }
}
