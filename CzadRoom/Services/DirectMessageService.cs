using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;

namespace CzadRoom.Services {
    public class DirectMessageService : IDirectMessageService {
        private readonly IMongoDbContext _mongoDbContext;

        public DirectMessageService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task AddDirectMessage(DirectMessage directMessage) {
            await _mongoDbContext.DirectMessages.InsertOneAsync(directMessage);
        }

        public IEnumerable<DirectMessage> GetDirectMessages(string roomId, string userId, int count) {
            var messages = _mongoDbContext.DirectMessages.AsQueryable()
                 .Where(x => x.RoomID == roomId)
                 .OrderByDescending(x => x.Date)
                 .Take(count)
                 .OrderBy(x => x.Date);
            var newMessages = messages.Where(x => x.FromID != userId && !x.Read);
            foreach (var msg in newMessages) {
                _mongoDbContext.DirectMessages.UpdateOne(x => x.ID == msg.ID, Builders<DirectMessage>.Update.Set(m => m.Read, true));
                _mongoDbContext.DirectMessages.UpdateOne(x => x.ID == msg.ID, Builders<DirectMessage>.Update.Set(m => m.ReadAt, DateTime.Now));
            }

            return messages;
        }

        public bool HasNewMessage(string roomId, string userId) {
            var message = _mongoDbContext.DirectMessages.AsQueryable()
                 .Where(x => x.RoomID == roomId)
                 .OrderByDescending(x => x.Date)
                 .FirstOrDefault();
            return message != null && message.FromID != userId && !message.Read;
        }

        public IEnumerable<DirectMessage> GetDirectMessages(string roomId, string userId, DateTime dateTime, int count) {
            var messages = _mongoDbContext.DirectMessages.AsQueryable()
                .Where(x => x.RoomID == roomId)
                .OrderByDescending(x => x.Date)
                .Where(x => x.Date < dateTime)
                .Take(count)
                .OrderBy(x => x.Date);
            var newMessages = messages.SkipWhile(x => x.Read).Where(x => x.FromID != userId);
            foreach (var msg in newMessages) {
                _mongoDbContext.DirectMessages.UpdateOne(x => x.ID == msg.ID, Builders<DirectMessage>.Update.Set(m => m.Read, true));
                _mongoDbContext.DirectMessages.UpdateOne(x => x.ID == msg.ID, Builders<DirectMessage>.Update.Set(m => m.ReadAt, DateTime.Now));
            }

            return messages;
        }

        public async Task MarkAsRead(string roomId, string userId) {
            var message = _mongoDbContext.DirectMessages.AsQueryable()
                 .Where(x => x.RoomID == roomId)
                 .OrderByDescending(x => x.Date)
                 .FirstOrDefault(x => x.FromID != userId);
            if (message == null)
                return;
            var msgDb = await _mongoDbContext.DirectMessages.Find(x => x.ID == message.ID).FirstOrDefaultAsync();
            await _mongoDbContext.DirectMessages.UpdateOneAsync(x => x.ID == message.ID, Builders<DirectMessage>.Update.Set(m => m.Read, true));
            await _mongoDbContext.DirectMessages.UpdateOneAsync(x => x.ID == message.ID, Builders<DirectMessage>.Update.Set(m => m.ReadAt, DateTime.Now));
        }
    }
}
