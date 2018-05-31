using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services {
    public class RoomService : IRoomService {

        private readonly IMongoDbContext _context;

        public RoomService(IMongoDbContext mongoDbContext) {
            _context = mongoDbContext;
        }

        public Task AppendMessage(string roomId, string message) {
            throw new NotImplementedException();
        }

        public async Task<string> CreateRoom(Room room) {
            await _context.Rooms.InsertOneAsync(room);
            return room.ID;
        }

        public async Task<bool> DeleteRoom(string roomId) {
            await _context.Connections.DeleteManyAsync(x => x.RoomID == roomId);
            DeleteResult deleteResult = await _context.Rooms.DeleteOneAsync(x => x.ID == roomId);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Room>> GetAll() {
            return await _context.Rooms.Find(_ => true).ToListAsync();
        }

        public async Task<Room> GetRoom(string roomId) {
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.ID, roomId);
            return await _context.Rooms.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRoom(Room room) {
            ReplaceOneResult updateResult = await _context.Rooms.ReplaceOneAsync(filter: r => r.ID == room.ID, replacement: room);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> AddAccessedUserToRoom(string roomId, string userId) {
            var updateResult = await _context.Rooms.UpdateOneAsync(
                Builders<Room>.Filter.Eq(x => x.ID, roomId),
                Builders<Room>.Update.AddToSet(x => x.UsersIDWithAccess, userId)
                );
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public bool HasUserAccess(string roomId, string userId) {
            var room = _context.Rooms.Find(x => x.ID == roomId).FirstOrDefault();
            return room.UsersIDWithAccess.Contains(userId) || room.OwnerID == userId;
        }

        public async Task UserConnected(RoomConnection roomConnection) {
            await _context.Connections.InsertOneAsync(roomConnection);
        }

        public async Task<string> UserDisconnected(string connectionId) {
            var connection = await _context.Connections.Find(x => x.ConnectionID == connectionId).FirstOrDefaultAsync();
            await _context.Connections.DeleteOneAsync(x => x.ID == connection.ID);
            return connection.RoomID;
        }

        public async Task<int> ConnectedUsersCount(string roomId) {
            return (int)await _context.Connections.CountAsync(x => x.RoomID == roomId);
        }

        public async Task RemoveAllConnections() {
            await _context.Connections.DeleteManyAsync(_ => true);
        }

        public async Task<IEnumerable<string>> ConnectedUsersID(string roomId) {
            return (await _context.Connections.Find(x => x.RoomID == roomId).ToListAsync()).Select(x => x.UserID);
        }

        public async Task<IEnumerable<User>> ConnectedUsers(string roomId) {
            var connections = await _context.Connections.Find(x => x.RoomID == roomId).ToListAsync();
            return connections.Select(x => _context.Users.Find(u => u.ID == x.UserID).FirstOrDefault());
        }
    }
}
