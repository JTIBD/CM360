using ExcelDataReader;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Users;
using FMAplication.Services.FileImports.Interfaces;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FMAplication.Models.PosmAssign;
using FMAplication.Services.DailyActivities.Interfaces;

namespace FMAplication.Services.FileImports.Implementation
{
    public class FileImportService : IFileImportService
    {
        private readonly IWebHostEnvironment _host;
        private readonly ICMUserService _userService;
        private readonly IPosmAssignService _posmAssignService;

        public FileImportService(IWebHostEnvironment host,
            ICMUserService userService, IPosmAssignService posmAssignService)
        {
            _host = host;
            _userService = userService;
            _posmAssignService = posmAssignService;
        }

        public async Task<(IEnumerable<CMUserRegisterModel> Data, List<UserDataErrorViewModel> errors)> ExcelImportCAUserAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable datatable = dataset.Tables[0];

                    foreach (DataColumn dataColumn in datatable.Columns)
                    {
                        dataColumn.ColumnName = Regex.Replace(dataColumn.ColumnName, @"\s+", "").ToUpper();
                    }

                    return await _userService.ExcelSaveToDatabaseAsync(datatable);
                }
            }
        }

        public async Task<(IEnumerable<PosmAssignModel> Data, List<UserDataErrorViewModel> errors)> ExcelImportPosmAssignAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable datatable = dataset.Tables[0];

                    //foreach (DataColumn dataColumn in datatable.Columns)
                    //{
                    //    dataColumn.ColumnName = Regex.Replace(dataColumn.ColumnName, @"\s+", "").ToUpper();
                    //}

                    return await _posmAssignService.ExcelSaveToDatabaseAsync(datatable);
                }
            }
        }
    }
}
