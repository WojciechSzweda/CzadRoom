using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using CzadRoom.Contexts;
using MongoDB.Driver;

namespace CzadRoom.Services
{
    public class DirectMessageRoomService : IDirectMessageRoomService, IRoomService<DirectMessageRoom> {
        private readonly IMongoDbContext _mongoDbContext;

        public DirectMessageRoomService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateRoom(DirectMessageRoom directMessageRoom) {
            await _mongoDbContext.DirectMessagesRooms.InsertOneAsync(directMessageRoom);
        }

        public async Task<bool> DeleteRoom(string roomId) {
            DeleteResult deleteResult = await _mongoDbContext.DirectMessagesRooms.DeleteOneAsync(x => x.ID == roomId);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<DirectMessageRoom>> GetAll() {
            return await _mongoDbContext.DirectMessagesRooms.Find(_ => true).ToListAsync();
        }

        public async Task<DirectMessageRoom> GetRoom(string roomId) {
            return await _mongoDbContext.DirectMessagesRooms.Find(x => x.ID == roomId).FirstOrDefaultAsync();
        }

        public async Task<DirectMessageRoom> GetRoomWithFriend(string userId, string friendId) {
            return await _mongoDbContext.DirectMessagesRooms.Find(x => x.Users.Contains(userId) && x.Users.Contains(friendId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DirectMessageRoom>> GetUserRooms(string userId) {
            return await _mongoDbContext.DirectMessagesRooms.Find(x => x.Users.Contains(userId)).ToListAsync();
        }

        public bool HasUserAccess(string roomId, string userId) {
            var room = _mongoDbContext.DirectMessagesRooms.Find(x => x.ID == roomId).FirstOrDefault();
            return room.Users.Contains(userId);
        }

        Task<string> IRoomService<DirectMessageRoom>.CreateRoom(DirectMessageRoom room) {
            throw new NotImplementedException();
        }
    }
}
