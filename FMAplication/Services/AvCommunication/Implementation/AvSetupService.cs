using FMAplication.common;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.Models.AvCommunications;
using FMAplication.Repositories;
using FMAplication.Services.AvCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Extensions;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using FMAplication.Services.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.AvCommunication.Implementation
{
    public class AvSetupService : IAvSetupService
    {
        private readonly IRepository<AvSetup> _avSetup;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<Domain.Brand.Brand> _brand;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly ICommonService _common;
        private readonly IRepository<DailyAVTask> _dailyAv;


        public AvSetupService(IRepository<AvSetup> avSetup, IRepository<SalesPoint> salesPoint,
            IRepository<CMUser> cmUser, IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping, 
            IRepository<Domain.Brand.Brand> brand, IRepository<DailyTask> dailyTask, ICommonService common,
            IRepository<DailyAVTask> dailyAv)
        {
            _avSetup = avSetup;
            _salesPoint = salesPoint;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _brand = brand;
            _dailyTask = dailyTask;
            _common = common;
            _dailyAv = dailyAv;
        }

        public async Task<bool> IsAvSetUpActive(AvSetupModel model)
        {
            return _avSetup.IsExist(av => av.Id == model.Id && av.Code == model.Code && av.Status == Status.Active);
        }

        public async Task<List<AvSetupModel>> Create(List<AvSetupModel> payload)
        {
            if (payload is null || payload.Count == 0) throw new AppException("No AV provided");
            List<AvSetupModel> list = new List<AvSetupModel>();

            DateTime fromDate = payload[0].FromDate;
            DateTime toDate = payload[0].ToDate;

            _common.ValidateDateRange(fromDate, toDate);

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();

            var salespoints = (await _salesPoint.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            List<AvSetup> avSetups = new List<AvSetup>();

            var user = AppIdentity.AppUser;

            foreach (var s in payload)
            {
                var sp = salespoints.Find(x => x.SalesPointId == s.SalesPointId);
                AvSetup sv = new AvSetup()
                {
                    Code = $"AV_{sp.Code}",
                    CreatedBy = user.UserId,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate,
                    SalesPointId = sp.SalesPointId,
                    AvId = s.AvId,
                    UserType = s.UserType,                    
                };
                avSetups.Add(sv);
            }

            var existingModels = await GetExistingAvSetups(payload);
            var existingModelIds = existingModels.Select(x => x.Id).ToList();
            var existingSurveys = _avSetup.FindAll(x => existingModelIds.Contains(x.Id)).ToList();

            if (existingSurveys.Count > 0)
            {
                var dateRangeViolatedSurveys = existingSurveys.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var conflictingSps = salespoints.FindAll(x => dateRangeViolatedSurveys.Any(v => x.SalesPointId == v.SalesPointId)).ToList();
                    var spNames = conflictingSps.Select(x => x.Name).ToList();
                    throw new AppException($"The date range conflicts with existing survey in {string.Join(",", spNames)}");
                }

                foreach (var srv in existingSurveys)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _avSetup.UpdateListAsync(existingSurveys);
            }

            await _avSetup.CreateListAsync(avSetups);

            list = avSetups.MapToModel();

            return list;
        }

        public async Task<List<AvSetupModel>> GetExistingAvSetups(List<AvSetupModel> avSetups)
        {
            var list = await _common.GetExistingSetups(_avSetup, avSetups.Cast<BaseSetupModel>().ToList());
            return list.MapToModel();
        }

        public async Task<AvSetupModel> GetAvSetupById(int id)
        {
            var entry = await _avSetup.FindIncludeAsync(x => x.Id == id, i2 => i2.Av);
            if (entry is null) throw new AppException("Entry not found");
            var model = entry.MapToModel();
            model.SalesPoint = _salesPoint.GetAllActive().FirstOrDefault(x => x.SalesPointId == model.SalesPointId)
                ?.ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<Pagination<AvSetupModel>> GetSetups(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search,int salespointId)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _avSetup.FindAllInclude(x => x.FromDate >= fromDateTime && x.ToDate <= toDateTime, i2 => i2.Av);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var sIds = _salesPoint.GetAll().Where(x => x.Code.Contains(search) || x.Name.Contains(search) ).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId) || x.Av.CampaignName.Contains(search));
            }
            if(salespointId != 0)
            {
                query = query.Where(x => x.SalesPointId == salespointId);
            }

            query = query.OrderByDescending(x => x.CreatedTime);
            var list = await query.ToPagedListAsync(pageIndex, pageSize);
            var surveyModels = list.ToList().MapToModel();
            _common.InsertSalesPoints(surveyModels);

            Pagination<AvSetupModel> paginatedList = new Pagination<AvSetupModel>(pageIndex, pageSize, list.TotalItemCount, surveyModels);

            return paginatedList;
        }

        public async Task<List<AvSetupMBModel>> GetAvSetupsOfTodayByUser(int userId)
        {
            var user = await _cmUser.FindAsync(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.FindAll(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var list = _avSetup.FindAllInclude(x => x.Status == Status.Active && sIds.Contains(x.SalesPointId) && x.FromDate <= todayDate && todayDate <= x.ToDate &&
                (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.Av).ToList();


            var brandIds = list.Select(x => x.Av.BrandId).Distinct().ToList();
            var brands = _brand.FindAll(x => brandIds.Contains(x.Id)).ToList();
            foreach (var l in list)
            {
                l.Av.Brand = brands.FirstOrDefault(x => x.Id == l.Av.BrandId);
            }
            var data = list.MapToMBModel();


            foreach (var d in data)
            {
                var result = await GetOrCreateDailyTask(userId, new DailyTaskMBModel { DateTime = todayDate, SalesPointId = d.SalesPointId });
                d.DailyTaskId = result.Id;
            }


            var dailyTaskIds = data.Select(x => x.DailyTaskId).ToList();
            var submittedDailyAvTask = _dailyAv.GetAllActive().Include(inc => inc.DailyTask)
                .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

            var submittedDailyTaskIds = submittedDailyAvTask.Select(x => x.DailyTaskId).ToList();

            if (submittedDailyTaskIds.Count > 0)
                data = data.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

            return data;
        }

        public async Task<List<AvSetupMBModel>> GetAvSetupsOfTodayByUser(DailyTask task)
        {
            try
            {
                var user = await _cmUser.FindAsync(x => x.Id == task.CmUserId);
                AssignedUserType userType = (AssignedUserType)user.UserType;
                var list = _avSetup.FindAllInclude(x => x.Status == Status.Active && x.SalesPointId == task.SalesPointId && x.FromDate <= task.DateTime && task.DateTime <= x.ToDate &&
                                                        (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.Av).ToList();

                var brandIds = list.Select(x => x.Av.BrandId).Distinct().ToList();
                var brands = _brand.FindAll(x => brandIds.Contains(x.Id)).ToList();
                foreach (var l in list)
                {
                    l.Av.Brand = brands.FirstOrDefault(x => x.Id == l.Av.BrandId);
                }
                var data = list.MapToMBModel();


                foreach (var d in data)
                {
                    var result = await GetOrCreateDailyTask(task.CmUserId, new DailyTaskMBModel { DateTime = task.DateTime, SalesPointId = d.SalesPointId });
                    d.DailyTaskId = result.Id;
                }


                var dailyTaskIds = data.Select(x => x.DailyTaskId).ToList();
                var submittedDailyAvTask = _dailyAv.GetAllActive().Include(inc => inc.DailyTask)
                    .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

                var submittedDailyTaskIds = submittedDailyAvTask.Select(x => x.DailyTaskId).ToList();

                if (submittedDailyTaskIds.Count > 0)
                    data = data.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

                return data;
            }
            catch (Exception e)
            {
                return new List<AvSetupMBModel>();
            }
        }

        public async Task<DailyTaskMBModel> GetOrCreateDailyTask(int cmUserId, DailyTaskMBModel model)
        {
            var checkExisting = await _dailyTask.FindAsync(x =>
                x.SalesPointId == model.SalesPointId && x.CmUserId == cmUserId &&
                x.DateTime.Date == model.DateTime.Date);

            if (checkExisting != null)
                return checkExisting.ToMap<DailyTask, DailyTaskMBModel>();

            var dailyTaskModel = new DailyTask();
            dailyTaskModel.SalesPointId = model.SalesPointId;
            dailyTaskModel.CmUserId = cmUserId;
            dailyTaskModel.DateTime = model.DateTime;

            await _dailyTask.CreateAsync(dailyTaskModel);
            var dailyTask = dailyTaskModel.ToMap<DailyTask, DailyTaskMBModel>();

            return dailyTask;
        }

        public async Task<AvSetupModel> UpdateAvSetup(AvSetupModel payload)
        {
            AvSetup avSetup = await _avSetup.FindAsync(x => x.Id == payload.Id);
            if (avSetup is null) throw new AppException("AvSetup not found");
            var existingAvSetups = await GetExistingAvSetups(new List<AvSetupModel>() { payload });
            existingAvSetups = existingAvSetups.FindAll(x => x.Id != payload.Id);
            var fromDate = payload.FromDate;
            var toDate = payload.ToDate;
            if (existingAvSetups.Count > 0)
            {
                var dateRangeViolatedSurveys = existingAvSetups.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var spName = "";
                    if (payload.SalesPoint is object) spName = payload.SalesPoint.Name;
                    else spName = _salesPoint.Find(x => x.SalesPointId == payload.SalesPointId).Name;
                    throw new AppException($"The date range conflicts with existing Av setup in {spName}");
                }

                var srvIds = existingAvSetups.Select(x => x.Id).ToList();
                var serveys = _avSetup.FindAll(x => srvIds.Contains(x.Id)).ToList();

                foreach (var srv in serveys)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _avSetup.UpdateListAsync(serveys);
            }
            if (avSetup.ToDate != payload.ToDate)
            {
                avSetup.ToDate = payload.ToDate;
            }
            if (avSetup.FromDate != payload.FromDate)
            {
                avSetup.FromDate = payload.FromDate;
            }

            if (avSetup.Status != payload.Status) avSetup.Status = payload.Status;
            if (avSetup.AvId != payload.AvId) avSetup.AvId= payload.AvId;
            await _avSetup.UpdateAsync(avSetup);
            return avSetup.MapToModel();
        }
    }
}
