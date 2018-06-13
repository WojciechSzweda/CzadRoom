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

        private readonly ConcurrentDictionary<string, RoomConnection> _connections = new ConcurrentDictionary<string, RoomConnection>();

        public int ConnectedUsersCount(string roomId) {
            return _connections.Count(x => x.Value.RoomID == roomId);
        }

        public IEnumerable<string> ConnectedUsersID(string roomId) {
            return _connections.Where(x => x.Value.RoomID == roomId).Distinct().Select(x => x.Value.UserID);
        }

        public RoomConnection GetRoomConnection(string connectionId) {
            _connections.TryGetValue(connectionId, out RoomConnection roomConnection);
            return roomConnection;
        }

        public void RemoveAllConnections() {
            _connections.Clear();
        }

        public bool UserConnected(string connectionId, RoomConnection roomConnection) {
            return _connections.TryAdd(connectionId, roomConnection);
        }

        public (bool, RoomConnection) UserDisconnected(string connectionId) {
            if (!_connections.ContainsKey(connectionId))
                return (false, default);
            return (_connections.TryRemove(connectionId, out RoomConnection roomConnection), roomConnection);
        }
    }
}
