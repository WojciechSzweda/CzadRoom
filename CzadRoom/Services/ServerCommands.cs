using CzadRoom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services
{
    public class ServerCommands : IServerCommands {

        private Dictionary<string, Func<string, string>> _commands = new Dictionary<string, Func<string, string>>();

        public ServerCommands() {
            _commands.Add("time", (_) => DateTime.Now.ToString());
        }

        public string ExecuteCommand(string command) {
            if (string.IsNullOrEmpty(command))
                return "Command does not exist";
            var parameters = command.Split(" ");
            command = parameters[0];
            Func<string, string> Method;
            if (_commands.TryGetValue(command, out Method))
                return Method(parameters.Length > 1 ? parameters[1]: string.Empty);
            return "Command does not exist";
        }
    }
}
