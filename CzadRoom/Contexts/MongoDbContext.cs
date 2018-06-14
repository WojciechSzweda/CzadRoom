using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CzadRoom.Contexts
{
    public class MongoDbContext : IMongoDbContext {
        private readonly IMongoDatabase _db;
        public MongoDbContext(IOptions<Settings.MongoSettings> options) {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
        public IMongoCollection<ChatRoom> Rooms => _db.GetCollection<ChatRoom>("Rooms");
        public IMongoCollection<ChatMessage> ChatMessages => _db.GetCollection<ChatMessage>("ChatMessages");
        public IMongoCollection<DirectMessageRoom> DirectMessagesRooms => _db.GetCollection<DirectMessageRoom>("DirectMessageRooms");
        public IMongoCollection<PublicRoom> PublicRooms => _db.GetCollection<PublicRoom>("PublicRooms");
        public IMongoCollection<DirectMessage> DirectMessages => _db.GetCollection<DirectMessage>("DirectMessages");
    }
}
