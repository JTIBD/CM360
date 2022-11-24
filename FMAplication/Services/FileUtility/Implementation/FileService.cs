using ExcelDataReader;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Users;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.FileImports.Interfaces;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OfficeOpenXml;
using FMAplication.Controllers.FileUtility;
using OfficeOpenXml.Style;

namespace FMAplication.Services.FileUtility.Implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _host;
        private readonly ICMUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public FileService(IWebHostEnvironment host,
            ICMUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _host = host;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Delete(string path)
        {
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        public FileData GetFileContentFromPath(string path)
        {
            byte[] filedata = System.IO.File.ReadAllBytes(path);
            string contentType;
            string fileName = Path.GetFileName(path);
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);


            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };

            _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());

            FileData fileData = new FileData()
            {
                ContentType = contentType,
                Data = filedata,
                Name = fileName,
            };

            return fileData;
        }
       
        public void AutoExcelFitColumns(int headerCount, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headerCount; i++)
            {
                workSheet.Column(i + 1).AutoFit();
            }
        }

        public ParsedExcelData ParseExcel(IFormFile file)
        {
            ExcelPackage package = new ExcelPackage(file.OpenReadStream());
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

            int rows = worksheet.Dimension.Rows;
            int columns = worksheet.Dimension.Columns;
            ParsedExcelData parsedExcelData = new ParsedExcelData();
            List<List<object>> rowData = new List<List<object>>();
            for (int i = 1; i <= rows; i++)
            {
                var singleRow = GetRow(worksheet, i, columns);
                rowData.Add(singleRow);
            }

            parsedExcelData.Rows = rowData;
            return parsedExcelData;
        }

        public FileData GetFileData(byte[] bytes)
        {
            string fileName = Guid.NewGuid() + ".xlsx";
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            FileData fileData = new FileData()
            {
                ContentType = contentType,
                Data = bytes,
                Name = fileName
            };
            return fileData;
        }

        public void SetTableStyle(ExcelWorksheet workSheet, int headersCount)
        {
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
        }

        public void SetHeaderStyle(ExcelWorksheet workSheet, int headCount)
        {
            var header = workSheet.Cells[1, 1, 1, headCount];
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(194, 194, 194));
            header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        public void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }

        private List<object> GetRow(ExcelWorksheet worksheet, int row, int columns)
        {
            List<object> rowData = new List<object>();
            for (int j = 1; j <= columns; j++)
            {

                object content = worksheet.Cells[row, j].Value;
                rowData.Add(content);
            }
            return rowData;
        }

    }
}
