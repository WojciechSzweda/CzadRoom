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
    public class DirectMessageRoomService : IDirectMessageRoomService {
        private readonly IMongoDbContext _mongoDbContext;

        public DirectMessageRoomService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateDirectMessageRoom(DirectMessageRoom directMessageRoom) {
            await _mongoDbContext.DirectMessagesRooms.InsertOneAsync(directMessageRoom);
        }

        public async Task<DirectMessageRoom> GetDirectMessageRoom(string roomId) {
            return await _mongoDbContext.DirectMessagesRooms.Find(x => x.ID == roomId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DirectMessageRoom>> GetUserDirectMessageRooms(string userId) {
            return await _mongoDbContext.DirectMessagesRooms.Find(x => x.Users.Contains(userId)).ToListAsync();
        }
    }
}
