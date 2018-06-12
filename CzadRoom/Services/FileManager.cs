using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services {
    public class FileManager : IFileManager {
        private readonly string _imagesPath;

        public FileManager(IHostingEnvironment hostingEnvironment) {
            _imagesPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
        }

        public void DeleteImage(string fileName) {
            if (Directory.Exists(_imagesPath))
                if (File.Exists(Path.Combine(_imagesPath, fileName)))
                    File.Delete(Path.Combine(_imagesPath, fileName));
        }

        public string GetImagePath(string fileName) {
            return Path.Combine(_imagesPath, fileName);
        }

        public string UploadImage(IFormFile file, string username) {
            var fileName = username + DateTime.Now;

            if (!Directory.Exists(_imagesPath))
                Directory.CreateDirectory(_imagesPath);

            string path = _imagesPath + Path.DirectorySeparatorChar + fileName;
            using (var fileStream = new FileStream(path, FileMode.Create))
                file.CopyTo(fileStream);

            return fileName;
        }

        public bool ValidateImage(IFormFile file) {
            return (file == null || file.Length == 0 || Path.GetExtension(file.FileName) == null);
        }
    }
}
