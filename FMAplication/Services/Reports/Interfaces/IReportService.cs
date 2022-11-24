using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Models.Common;
using FMAplication.Models.DailyTasks;
using FMAplication.RequestModels.Reports;
using FMAplication.Services.FileUtility.Implementation;

namespace FMAplication.Services.Reports.Interfaces
{
    public interface IReportService
    {
        Task<Pagination<DailyTaskModel>> GetAuditReport(int pageIndex, int pageSize, string search,
            DateTime FromDateTime, DateTime ToDateTime);
        Task<Pagination<DailyTaskModel>> GetSurveyReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime);
        Task<Pagination<DailyTaskModel>> GetConsumerSurveyReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime);
        Task<Pagination<DailyTaskModel>> GetAvReport(int pageIndex, int pageSize, string search, DateTime fromDateTime,
            DateTime toDateTime);
        Task<Pagination<DailyTaskModel>> GetCommunicationReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime);
        Task<Pagination<DailyTaskModel>> GetInformationReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime);
        Task<Pagination<DailyTaskModel>> GetPOSMTaskReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime);
        Task<FileData> ExportAuditReportToExcel(int queryParamsPageIndex, int queryParamsPageSize,
            string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);

        Task<FileData> ExportSurveyReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportConsumerSurveyReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportAvReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportCommunicationReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportInformationReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportPOSMTaskReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime);
        Task<FileData> ExportCWStockUpdateToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime, List<int> cwIdList);
        Task<FileData> ExportCWDistributionReportToExcel(ExportCWDistributionReportToExcelModel payload);
        Task<FileData> ExportCWStockReportToExcel(List<int> wareHouseIds);
        Task<FileData> ExportSPStockReportToExcel(List<int> spIds);
        Task<FileData> ExportSPWisePosmLedgerReportToExcel(ExportSPWisePosmLedgerPayload payload);
        Task<FileData> ExportSPWisePosmLedgerReportToExcel2(ExportSPWisePosmLedgerPayload payload);
    }
}