using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IChatService
    {
        
        void SendMessageToRoom(string message);
    
    }
}
