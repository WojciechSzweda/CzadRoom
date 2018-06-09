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
    public class DirectMessagesService : IDirectMessageService {
        private readonly IMongoDbContext _mongoDbContext;

        public DirectMessagesService(IMongoDbContext mongoDbContext) {
            _mongoDbContext = mongoDbContext;
        }

        public async Task AddDirectMessage(DirectMessage directMessage) {
            await _mongoDbContext.DirectMessages.InsertOneAsync(directMessage);
        }

        public IEnumerable<DirectMessage> GetDirectMessages(string roomId, int count) {
            return _mongoDbContext.DirectMessages.AsQueryable()
                 .Where(x => x.RoomID == roomId)
                 .OrderByDescending(x => x.Date)
                 .Take(count)
                 .OrderBy(x => x.Date);
        }

        public IEnumerable<DirectMessage> GetDirectMessages(string roomId, DateTime dateTime, int count) {
            return _mongoDbContext.DirectMessages.AsQueryable()
                .Where(x => x.RoomID == roomId)
                .OrderByDescending(x => x.Date)
                .Where(x => x.Date < dateTime)
                .Take(count)
                .OrderBy(x => x.Date);
        }
    }
}
