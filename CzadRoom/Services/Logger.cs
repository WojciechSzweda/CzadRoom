using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services
{
    public class Logger : ILogger {
        private IHostingEnvironment _hostingEnvironment;

        public Logger(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        public void Log(string message) {
            if (!File.Exists(Path.Combine(_hostingEnvironment.WebRootPath, "log.txt"))) {
                File.Create(Path.Combine(_hostingEnvironment.WebRootPath, "log.txt"));
            }
            File.AppendAllTextAsync(Path.Combine(_hostingEnvironment.WebRootPath, "log.txt"), $"@{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }
}
