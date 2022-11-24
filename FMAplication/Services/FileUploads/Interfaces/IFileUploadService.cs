using FMAplication.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Services.FileUploads.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type);
        Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type, int width, int height);
        Task<string> SaveImageAsync(string base64String, string fileName, FileUploadCode type, int width, int height);
        Task<Image> ResizeImageAsync(Image image, int width, int height);
        Task DeleteImageAsync(string filePath);
    }
}
