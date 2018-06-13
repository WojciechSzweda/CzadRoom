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
    public class PublicRoomService : IPublicRoomService {
        private readonly IMongoDbContext _mongoDbContext;

        public PublicRoomService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<string> CreateRoom(PublicRoom room) {
            await _mongoDbContext.PublicRooms.InsertOneAsync(room);
            return room.ID;
        }

        public async Task<IEnumerable<PublicRoom>> GetAll() {
            return await _mongoDbContext.PublicRooms.Find(_ => true).ToListAsync();
        }

        public async Task<PublicRoom> GetRoom(string roomId) {
            return await _mongoDbContext.PublicRooms.Find(x => x.ID == roomId).FirstOrDefaultAsync();
        }
    }
}
