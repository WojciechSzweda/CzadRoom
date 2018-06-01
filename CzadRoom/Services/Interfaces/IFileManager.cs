using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IFileManager
    {
        (bool ok, string fileName) UploadImage(IFormFile file, string username);
        void ResizeImage(IFormFile file, int width, int height);
        void DeleteImage(string fileName);
        string GetImagePath(string fileName);
    }
}
