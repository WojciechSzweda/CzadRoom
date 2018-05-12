using CzadRoom.Contexts;
using CzadRoom.Models;
using CzadRoom.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Repositories
{
    public class UsersRepository : IUsersRepository {

        private readonly IMongoDbContext _context;

        public UsersRepository(IMongoDbContext context) {
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

        public async Task<User> GetUser(string username) {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Username, username);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(User user) {
            ReplaceOneResult updateResult = await _context.Users.ReplaceOneAsync(filter: u => u.Id == user.Id, replacement: user);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<IEnumerable<User>> GetUsers() {
            return await _context.Users.Find(_ => true).ToListAsync();
        }
    }
}
