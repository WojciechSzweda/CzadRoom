using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;

namespace CzadRoom.Services
{
    public class ConnectionService : IConnectionService {

        private readonly ConcurrentDictionary<string, RoomConnection> connections = new ConcurrentDictionary<string, RoomConnection>();

        public int ConnectedUsersCount(string roomId) {
            return connections.Count(x => x.Value.RoomID == roomId);
        }

        public IEnumerable<string> ConnectedUsersID(string roomId) {
            return connections.Where(x => x.Value.RoomID == roomId).Distinct().Select(x => x.Value.UserID);
        }

        public void RemoveAllConnections() {
            connections.Clear();
        }

        public bool UserConnected(string connectionId, RoomConnection roomConnection) {
            return connections.TryAdd(connectionId, roomConnection);
        }

        public (bool, RoomConnection) UserDisconnected(string connectionId) {
            if (!connections.ContainsKey(connectionId))
                return (false, default);
            RoomConnection roomConnection;
            return (connections.TryRemove(connectionId, out roomConnection), roomConnection);
        }
    }
}
