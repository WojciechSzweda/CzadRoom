using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string id);
        Task<User> GetUserByName(string username);
        Task<User> GetUserByEmail(string email);
        Task Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(string username);
        Task<IEnumerable<User>> GetFriends(string userId);
    }
}
