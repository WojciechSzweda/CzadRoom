﻿using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services {
    public class ChatRoomService : IChatRoomService {

        private readonly IMongoDbContext _context;

        public ChatRoomService(IMongoDbContext mongoDbContext) {
            _context = mongoDbContext;
        }

        public Task AppendMessage(string roomId, string message) {
            throw new NotImplementedException();
        }

        public async Task<string> CreateRoom(ChatRoom room) {
            await _context.Rooms.InsertOneAsync(room);
            return room.ID;
        }

        public async Task<bool> DeleteRoom(string roomId) {
            DeleteResult deleteResult = await _context.Rooms.DeleteOneAsync(x => x.ID == roomId);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ChatRoom>> GetAll() {
            return await _context.Rooms.Find(_ => true).ToListAsync();
        }

        public async Task<ChatRoom> GetRoom(string roomId) {
            FilterDefinition<ChatRoom> filter = Builders<ChatRoom>.Filter.Eq(r => r.ID, roomId);
            return await _context.Rooms.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRoom(ChatRoom room) {
            ReplaceOneResult updateResult = await _context.Rooms.ReplaceOneAsync(filter: r => r.ID == room.ID, replacement: room);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> AddAccessedUserToRoom(string roomId, string userId) {
            var updateResult = await _context.Rooms.UpdateOneAsync(
                Builders<ChatRoom>.Filter.Eq(x => x.ID, roomId),
                Builders<ChatRoom>.Update.AddToSet(x => x.UsersIDWithAccess, userId)
                );
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public bool HasUserAccess(string roomId, string userId) {
            var room = _context.Rooms.Find(x => x.ID == roomId).FirstOrDefault();
            return room.UsersIDWithAccess.Contains(userId) || room.OwnerID == userId || string.IsNullOrEmpty(room.Password);
        }

        public IEnumerable<User> ConnectedUsers(IEnumerable<string> usersId, string roomId) {
            return usersId.Select(id => _context.Users.Find(user => user.ID == id).FirstOrDefault());
        }
    }
}