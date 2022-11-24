using FMAplication.Controllers.FileUtility;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Users;
using FMAplication.Services.FileUtility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Services.FileUtility.Interfaces
{
    public interface IFileService
    {
        FileData GetFileContentFromPath(string path);
        void Delete(string path);
        void AutoExcelFitColumns(int count, ExcelWorksheet workSheet);
        ParsedExcelData ParseExcel(IFormFile file);
        FileData GetFileData(byte[] bytes);
        void SetTableStyle(ExcelWorksheet workSheet, int headersCount);
        void SetHeaderStyle(ExcelWorksheet workSheet, int headCount);
        void InsertHeaders(List<string> headers, ExcelWorksheet workSheet);
    }
}
