using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces {
    public interface IChatRoomService {
        Task<bool> AddAccessedUserToRoom(string roomId, string userId);
        bool HasUserAccess(string roomId, string userId);
        IEnumerable<User> ConnectedUsers(IEnumerable<string> usersId,string roomId);
    }
}
