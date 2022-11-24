using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Models.PosmAssign;

namespace FMAplication.Services.FileImports.Interfaces
{
    public interface IFileImportService
    {
        Task<(IEnumerable<CMUserRegisterModel> Data, List<UserDataErrorViewModel> errors)> ExcelImportCAUserAsync(
            IFormFile file);

        Task<(IEnumerable<PosmAssignModel> Data, List<UserDataErrorViewModel> errors)> ExcelImportPosmAssignAsync(
            IFormFile file);
    }
}
