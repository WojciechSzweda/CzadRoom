using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task AddMessage(ChatMessage chatMessage);
        IEnumerable<ChatMessage> GetChatMessages(string roomId, int count);
        IEnumerable<ChatMessage> GetChatMessages(string roomId, DateTime dateTime, int count);
    }
}
