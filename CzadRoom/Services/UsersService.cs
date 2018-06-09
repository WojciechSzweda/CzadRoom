using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services {
    public class UsersService : IUsersService {

        private readonly IMongoDbContext _context;

        public UsersService(IMongoDbContext context) {
            _context = context;
        }

        public async Task Create(User user) {
            await _context.Users.InsertOneAsync(user);
        }

        public async Task<bool> Delete(string username) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Username, username);
            DeleteResult deleteResult = await _context.Users.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<User> GetUser(string id) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.ID, id);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByName(string username) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Username, username);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(User user) {
            ReplaceOneResult updateResult = await _context.Users.ReplaceOneAsync(filter: u => u.ID == user.ID, replacement: user);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<IEnumerable<User>> GetUsers() {
            return await _context.Users.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFriends(string userId) {
            var userDb = await _context.Users.Find(x => x.ID == userId).FirstOrDefaultAsync();
            return userDb.FriendsID.Select(id => _context.Users.Find(user => user.ID == id).FirstOrDefault());
        }
    }
}
