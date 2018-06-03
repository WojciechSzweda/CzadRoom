using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;

namespace CzadRoom.Services
{
    public class ChatMessageService : IChatMessageService {
        private readonly IMongoDbContext _mongoDbContext;

        public ChatMessageService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task AddMessage(ChatMessage chatMessage) {
            await _mongoDbContext.ChatMessages.InsertOneAsync(chatMessage);
        }

        public IEnumerable<ChatMessage> GetChatMessages(string roomId, int count) {
           return _mongoDbContext.ChatMessages.AsQueryable().Where(x => x.RoomID == roomId).OrderBy(x => x.Date).Take(count);
        }

        public IEnumerable<ChatMessage> GetChatMessages(string roomId, DateTime dateTime, int count) {
            return _mongoDbContext.ChatMessages.AsQueryable().Where(x => x.RoomID == roomId).OrderBy(x => x.Date).Where(x => x.Date < dateTime).Take(count);
        }
    }
}
