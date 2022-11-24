using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.AzureStorageService.Interfaces
{
    public interface IBlobStorageService
    {
        void DeleteBlobData(string fileUrl);
        string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType, string folderName);
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType, string folderName);
    }
}
