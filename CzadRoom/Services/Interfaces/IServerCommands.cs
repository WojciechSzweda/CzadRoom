using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IServerCommands
    {
        string ExecuteCommand(string command);
    }
}
