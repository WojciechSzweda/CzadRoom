using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzadRoom.Models;

namespace CzadRoom.Services.Interfaces
{
    public interface IDirectMessageService
    {
        Task AddDirectMessage(DirectMessage directMessage);
        IEnumerable<DirectMessage> GetDirectMessages(string roomId, string userId, int count);
        IEnumerable<DirectMessage> GetDirectMessages(string roomId, string userId, DateTime dateTime, int count);
        bool HasNewMessage(string roomId, string userId);
    }
}
