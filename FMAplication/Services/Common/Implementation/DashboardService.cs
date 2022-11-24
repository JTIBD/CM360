using AutoMapper;
using fm_application.Models.DailyActivity;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Reports;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Common;
using FMAplication.Models.DailyActivity;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Users;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.POSMProducts.Interfaces;
using FMAplication.Services.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Models.DailyTasks;
using FMAplication.Services.Reports.Interfaces;

namespace FMAplication.Services.Common.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<DailyCMActivity> _dailyCMActivityRepo;
        private readonly IRepository<POSMReport> _posmReportRepo;
        private readonly IRepository<AuditReport> _auditReportRepo;
        private readonly IRepository<SurveyReport> _surveyReportRepo;
        private readonly IReportService _reportService;

        public DashboardService(
            IRepository<DailyCMActivity> dailyCMActivityRepo,
            IRepository<POSMReport> posmReportRepo,
            IRepository<AuditReport> auditReportRepo,
            IRepository<SurveyReport> surveyReportRepo, 
            IReportService reportService)
        {
            this._dailyCMActivityRepo = dailyCMActivityRepo;
            this._posmReportRepo = posmReportRepo;
            this._auditReportRepo = auditReportRepo;
            this._surveyReportRepo = surveyReportRepo;
            _reportService = reportService;
        }

        public async Task<DashboardModel> GetAllDashboardDataAsync()
        {
            DashboardModel model = new DashboardModel();

            #region Current month data
            await GetCurrentMonthData(model);
            #endregion

            

            #region Last month data
            await GetLastMonthData(model);
            #endregion

            return model;
        }

        private async Task GetLastMonthData(DashboardModel model)
        {
            //var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            //var lastMonth = todayDate.AddMonths(-1);
            //var fromDateT = lastMonth.AddDays(1 - lastMonth.Day);
            //var toDateT = todayDate.AddDays(-todayDate.Day).AddSeconds(-1);
            var (fromDateT, toDateT) = DateTime.UtcNow.GetBangladeshLastMonthDateRangeInUtc();

            var posmReportData = await _reportService.GetPOSMTaskReport(1, Int32.MaxValue, "", fromDateT, toDateT);

            var lastMonthPosmData = posmReportData.Data.SelectMany(x => x.DailyPosmTasks)
                .SelectMany(a => a.DailyPosmTaskItems).ToList();

            model.TotalLastMonthPosmInstallationProductCount = lastMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Installation)
                                                                .Sum(x=>x.Quantity);

            model.TotalLastMonthPosmRepairProductCount = lastMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Repair)
                                                                            .Sum(x=>x.Quantity);

            model.TotalLastMonthPosmRemovalProductCount = lastMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Removal)
                                                        .Sum(x=>x.Quantity);

            model.TotalLastMonthPosmRemovalAndReInstallationCount = lastMonthPosmData.Where(x => 
                                                                    x.ExecutionType == PosmWorkType.RemovalAndReInstallation).Sum(x=>x.Quantity);


            var auditReportData = await _reportService.GetAuditReport(1, Int32.MaxValue, "", fromDateT, toDateT);

            var currentMonthAuditReportData = auditReportData.Data.SelectMany(x => x.DailyAuditTasks)
                .SelectMany(x => x.DailyProductsAuditTask).ToList();
            model.TotalLastMonthAuditReportProductCount = currentMonthAuditReportData.Count;



            var consumerReportData = await _reportService.GetConsumerSurveyReport(1, Int32.MaxValue, "", fromDateT, toDateT);

            var lastMonthConsumerSurveyData = consumerReportData.Data.SelectMany(x => x.DailyConsumerSurveyTasks)
                .SelectMany(x => x.DailyConsumerSurveyTaskAnswers).ToList();

            model.TotalLastMonthConsumerSurveyReportProductCount = lastMonthConsumerSurveyData.Count;


            var customerReportData = await _reportService.GetSurveyReport(1, Int32.MaxValue, "", fromDateT, toDateT);
            var lastMonthCustomerSurveyData = customerReportData.Data.SelectMany(x => x.DailySurveyTasks)
                .SelectMany(x => x.DailySurveyTaskAnswers).ToList();

            model.TotalLastMonthSurveyReportProductCount = lastMonthCustomerSurveyData.Count;


        }

        private async Task GetCurrentMonthData(DashboardModel model)
        {
            var (fromDateS, toDateS) = DateTime.UtcNow.GetBangladeshCurrentMonthDateRangeInUtc();

            var posmReportData = await _reportService.GetPOSMTaskReport(1, Int32.MaxValue, "", fromDateS, toDateS);

            var currentMonthPosmData = posmReportData.Data.SelectMany(x => x.DailyPosmTasks)
                .SelectMany(a => a.DailyPosmTaskItems).ToList();

            model.TotalCurrentMonthPosmRepairProductCount = currentMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Repair)
                                                            .Sum(x=>x.Quantity);
            model.TotalCurrentMonthPosmInstallationProductCount = currentMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Installation)
                                                            .Sum(x=>x.Quantity);

            model.TotalCurrentMonthPosmRemovalProductCount = currentMonthPosmData.Where(x => x.ExecutionType == PosmWorkType.Removal)
                                                            .Sum(x=>x.Quantity);

            model.TotalCurrentMonthPosmRemovalAndReInstallationCount = currentMonthPosmData
                .Where(x => x.ExecutionType == PosmWorkType.RemovalAndReInstallation).Sum(x=>x.Quantity);

            var auditReportData = await _reportService.GetAuditReport(1, Int32.MaxValue, "", fromDateS, toDateS);

            var currentMonthAuditReportData = auditReportData.Data.SelectMany(x => x.DailyAuditTasks)
                .SelectMany(x => x.DailyProductsAuditTask).ToList();
            model.TotalCurrentMonthAuditReportProductCount = currentMonthAuditReportData.Count;


            var consumerReportData = await _reportService.GetConsumerSurveyReport(1, Int32.MaxValue, "", fromDateS, toDateS);

            var lastMonthConsumerSurveyData = consumerReportData.Data.SelectMany(x => x.DailyConsumerSurveyTasks)
                .SelectMany(x => x.DailyConsumerSurveyTaskAnswers).ToList();

            model.TotalCurrentMonthConsumerSurveyReportProductCount = lastMonthConsumerSurveyData.Count;


            var customerReportData = await _reportService.GetSurveyReport(1, Int32.MaxValue, "", fromDateS, toDateS);
            var currentMonthCustomerSurveyData = customerReportData.Data.SelectMany(x => x.DailySurveyTasks)
                .SelectMany(x => x.DailySurveyTaskAnswers).ToList();

            model.TotalCurrentMonthSurveyReportProductCount = currentMonthCustomerSurveyData.Count;
        }


        private async Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesReportsByCurrentUserAsync(int pageIndex, int pageSize, string fmUserIdsStr)
        {
            #region Store Procedure
            try
            {
                var storeProcedure = "spGetDCMAReports";
                var parameters = new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

                var result = _dailyCMActivityRepo.GetDataBySP<DailyCMTaskReportModel>(storeProcedure, parameters);

                return result.Items.Take(5).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            #endregion
        }

        private async Task<IEnumerable<(string CMUserName, int POSMInstallationCount, int POSMRepairCount, int POSMRemovalCount)>> GetDailyCMActivitesWithPOSMReportByCurrentUserAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            var data = (await _posmReportRepo.GetAllIncludeAsync(x => 
                                new 
                                {
                                    CMUserName = x.DailyCMActivity.CM.Name, 
                                    POSMInstallationCount = x.ActionType == POSMActionType.Installation ? 1 : 0,
                                    POSMRepairCount = x.ActionType == POSMActionType.Repair ? 1 : 0,
                                    POSMRemovalCount = x.ActionType == POSMActionType.Removal ? 1 : 0,
                                },
                                x => x.ActionType == POSMActionType.Installation && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                                x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && 
                                    (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)),
                                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
                                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.CM),
                                true)).ToList();

            var result = data.GroupBy(x => x.CMUserName).Select(x =>
            (CMUserName: x.Key, POSMInstallationCount: x.Sum(y => y.POSMInstallationCount), 
            POSMRepairCount: x.Sum(y => y.POSMRepairCount), POSMRemovalCount: x.Sum(y => y.POSMRemovalCount))).ToList();

            return result;
        }
        
        private async Task<int> GetCountDailyCMActivitiesByCurrentUserAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return (await _dailyCMActivityRepo.CountFuncAsync(x => x.Date.Date >= fromDate.Date && x.Date.Date <= toDate.Date && x.Status != Status.InActive && 
                                (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.AssignedFMUserId))));
        }

        private async Task<int> GetCountPOSMReportsInstallationAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _posmReportRepo.CountFuncAsync(x => x.ActionType == POSMActionType.Installation && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }

        private async Task<int> GetCountPOSMReportsRepairAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _posmReportRepo.CountFuncAsync(x => x.ActionType == POSMActionType.Repair && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }

        private async Task<int> GetCountPOSMReportsRemovalAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _posmReportRepo.CountFuncAsync(x => x.ActionType == POSMActionType.Removal && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }

        private async Task<int> GetCountAuditReportsAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _auditReportRepo.CountFuncAsync(x => x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }

        private async Task<int> GetCountSurveyReportsAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _surveyReportRepo.CountFuncAsync(x => !x.IsConsumerSurvey && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }

        private async Task<int> GetCountConsumerSurveyReportsAsync(DateTime fromDate, DateTime toDate, bool isAdmin, IList<int> fmUserIds, bool isOnlyCompleted = false)
        {
            return await _surveyReportRepo.CountFuncAsync(x => x.IsConsumerSurvey && x.DailyCMActivity.Date.Date >= fromDate.Date &&
                            x.DailyCMActivity.Date.Date <= toDate.Date && x.DailyCMActivity.Status != Status.InActive && (!isOnlyCompleted || x.Status == Status.Completed) && (isAdmin || fmUserIds.Contains(x.DailyCMActivity.AssignedFMUserId)));
        }
    }
}
