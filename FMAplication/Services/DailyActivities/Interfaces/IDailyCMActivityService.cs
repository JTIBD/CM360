using fm_application.Models.DailyActivity;
using FMAplication.Models.DailyActivity;
using FMAplication.Domain.DailyActivities;
using FMAplication.Models.Examples;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Models.Reports;
using FMAplication.RequestModels.Reports;
using X.PagedList;
using FMAplication.Services.FileUtility.Implementation;

namespace FMAplication.Services.DailyActivities.Interfaces
{
    public interface IDailyCMActivityService
    {
        Task<(DailyCMActivityModel Data, bool IsExists)> SaveCMTask(DailyCMActivityModel model);
        Task<DailyCMActivityModel> GetCMTaskById(int id);
        Task<List<DailyCMActivityModel>> GetFilteredCMTask(SearchDailyCMActivityModel searchModel);
        Task<IEnumerable<DailyCMActivityModel>> GetDailyCMActivityAsync();
        Task<List<DailyCMActivityModel>> mapSurveyReportQuestions(List<DailyCMActivityModel> model);
        Task<(IEnumerable<DailyCMActivitySPModel>,int total)> GetDailyCMActivitesByCurrentUserAsync(int pageIndex,int pageSize,string search);
        Task<DailyCMActivityModel> UpdateStatusAsync(DailyCMActivityModel model);
        Task<IEnumerable <DailyCMActivityModel>> UpdateBatchStatusAsync(BatchStatusChangeModel model);
        Task<IEnumerable<DailyCMActivityModel>> GetDailyCMActivitesWithPOSMReportByCurrentUserAsync();
        Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesForDashboardByCurrentUserAsync(int pageIndex, int pageSize);
        Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesReportsByCurrentUserAsync(int pageIndex, int pageSize);

        Task<(IEnumerable<SurveyReportSPModel>,int total)> GetSurveyReportsByCurrentUserAsync(int pageIndex, int pageSize, string search);
        Task<(IEnumerable<AuditReportSPModel>,int total)> GetAuditReportsByCurrentUserAsync(int pageIndex, int pageSize, string search);

        Task<(IEnumerable<POSMReportSPModel>,int total)> GetPOSMReportsByCurrentUserAsync(int pageIndex,int pageSize,string search);

        Task<(IEnumerable<object>, int total)> GetDCMAReportsInDetailsByCurrentUserAsync(int pageIndex, int pageSize, string search,bool isAll);
        Task<(IEnumerable<object>,int total)> GetDCMAReportsSalesPointWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll);
        Task<(IEnumerable<object>, int total)> GetDCMAReportsTerritoryWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll);
        Task<(IEnumerable<object>, int total)> GetDCMAReportsAreaWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll);
        Task<(IEnumerable<object>,int total)> GetDCMAReportsRegionWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll);
        Task<List<DailyCMActivityModel>> GetDailyCMActivitesByCMUserAsync(int userId);
        FileData DownloadExcelFormatOfTask();
        Task<DCMAReportsInDetailsResponse> GetDCMAReportsInDetails2(int pageIndex, int pageSize, string search);
        Task<DCMAReportsInDetailsResponse> GetDCMAReportsSalesPointWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search);
        Task<DCMAReportsInDetailsResponse> GetDCMAReportsTerritoryWiseByCurrentUserAsync2(int pageIndex, int pageSize,
            string search);

        Task<DCMAReportsInDetailsResponse> GetDCMAReportsAreaWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search);
        Task<DCMAReportsInDetailsResponse> GetDCMAReportsRegionWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportsSpWise(GetExecutionReport payload);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportTeritoryWise(GetExecutionReport payload);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportAreaWise(GetExecutionReport payload);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportRegionWise(GetExecutionReport payload);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportNational(GetExecutionReport payload);
        Task<List<Dictionary<string, dynamic>>> GetExecutionReportOutletWise(GetExecutionReport payload);
    }
}
