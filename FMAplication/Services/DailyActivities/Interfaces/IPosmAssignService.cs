using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Core.Params;
using FMAplication.Models.PosmAssign;
using FMAplication.Models.Users;
using FMAplication.Services.FileUtility.Implementation;
using X.PagedList;

namespace FMAplication.Services.DailyActivities.Interfaces
{
    public interface IPosmAssignService
    {
        Task<FileData> GetExcelFileForPosmAssign(ExportPosmAssignViewModel model);
        Task<(IEnumerable<PosmAssignModel> Data, List<UserDataErrorViewModel> Errors)> ExcelSaveToDatabaseAsync(
            DataTable datatable);

        Task<Pagination<PosmAssignModel>> GetPosmAssigns(PomsAssignParams searchParams);
        Task<List<PosmTaskMBModel>> GetPosmTasks(int userId, int salesPointId, DateTime date);
    }
}
