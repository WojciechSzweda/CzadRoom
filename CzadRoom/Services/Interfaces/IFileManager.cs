using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IFileManager
    {
        bool ValidateImage(IFormFile file);
        string UploadImage(IFormFile file, string username);
        void DeleteImage(string fileName);
        string GetImagePath(string fileName);
    }
}
