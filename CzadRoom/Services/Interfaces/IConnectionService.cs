using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IConnectionService
    {
        bool UserConnected(string connectionId, RoomConnection roomConnection);
        (bool, RoomConnection) UserDisconnected(string connectionId);
        void RemoveAllConnections();
        int ConnectedUsersCount(string roomId);
        IEnumerable<string> ConnectedUsersID(string roomId);
        RoomConnection GetRoomConnection(string connectionId);
    }
}
