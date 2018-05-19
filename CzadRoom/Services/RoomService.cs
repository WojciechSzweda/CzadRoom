using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services
{
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
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.ID, roomId);
            DeleteResult deleteResult = await _context.Rooms.DeleteOneAsync(filter);
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
    }
}
