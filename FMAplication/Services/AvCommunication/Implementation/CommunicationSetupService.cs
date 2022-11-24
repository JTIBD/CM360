using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using FMAplication.Repositories;
using FMAplication.Services.AvCommunication.Interfaces;
using FMAplication.Services.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.AvCommunication.Implementation
{
    public class CommunicationSetupService : ICommunicationSetupService
    {
        private readonly IRepository<CommunicationSetup> _communicationSetup;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<Domain.Brand.Brand> _brand;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly ICommonService _common;
        private readonly IRepository<DailyCommunicationTask> _dailyCommunication;

        public CommunicationSetupService(IRepository<CommunicationSetup> communicationSetup, IRepository<SalesPoint> salesPoint,
            IRepository<CMUser> cmUser, IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping, 
            IRepository<Domain.Brand.Brand> brand, 
            IRepository<DailyTask> dailyTask, ICommonService common, 
            IRepository<DailyCommunicationTask> dailyCommunication)
        {
            _communicationSetup = communicationSetup;
            _salesPoint = salesPoint;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _brand = brand;
            _dailyTask = dailyTask;
            _common = common;
            _dailyCommunication = dailyCommunication;
        }
        public  async  Task<List<CommunicationSetupModel>> Save(List<CommunicationSetupModel> payload)
        {
            if (payload is null || payload.Count == 0) throw new AppException("No Communication Data provided");
            List<CommunicationSetupModel> list = new List<CommunicationSetupModel>();

            DateTime fromDate = payload[0].FromDate;
            DateTime toDate = payload[0].ToDate;

            _common.ValidateDateRange(fromDate, toDate);

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();
            var salespoints = (await _salesPoint.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            List<CommunicationSetup> communicationSetups = new List<CommunicationSetup>();

            var user = AppIdentity.AppUser;

            foreach (var s in payload)
            {
                var sp = salespoints.Find(x => x.SalesPointId == s.SalesPointId);
                CommunicationSetup communicationSetup = new CommunicationSetup()
                {
                    Code = $"Communication_{sp.Code}",
                    CreatedBy = user.UserId,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate,
                    SalesPointId = sp.SalesPointId,
                    AvCommunicationId = s.AvCommunicationId,
                    UserType = s.UserType,
                };
                communicationSetups.Add(communicationSetup);
            }

            var existingModels = await GetExistingCommunicationSetups(payload);
            var existingModelIds = existingModels.Select(x => x.Id).ToList();
            var existingCommunications = _communicationSetup.FindAll(x => existingModelIds.Contains(x.Id)).ToList();

            if (existingCommunications.Count > 0)
            {
                var dateRangeViolatedCommunicationSetups = existingCommunications.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate>= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedCommunicationSetups.Count > 0)
                {
                    var conflictingSps = salespoints.FindAll(x => dateRangeViolatedCommunicationSetups.Any(v => x.SalesPointId == v.SalesPointId)).ToList();
                    var spNames = conflictingSps.Select(x => x.Name).ToList();
                    throw new AppException($"The date range conflicts with existing communication in {string.Join(",", spNames)}");
                }

                foreach (var communication in existingCommunications)
                {
                    communication.ToDate = fromDate.AddSeconds(-1);
                }
                await _communicationSetup.UpdateListAsync(existingCommunications);
            }

            await _communicationSetup.CreateListAsync(communicationSetups);

            list = communicationSetups.MapToModel();
            return list;
        }
        public async Task<Pagination<CommunicationSetupModel>> GetAll(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search,int salesPointId)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _communicationSetup.FindAllInclude(x => x.FromDate >= fromDateTime && x.ToDate <= toDateTime,
                 i2 => i2.AvCommunication);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var sIds = _salesPoint.GetAll().Where(x => x.Code.Contains(search) || x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId) || x.AvCommunication.CampaignName.Contains(search) );
            }
            if (salesPointId != 0) query = query.Where(x => x.SalesPointId == salesPointId);

            query = query.OrderByDescending(x => x.CreatedTime);
            var list = await query.ToPagedListAsync(pageIndex, pageSize);
            var data = list.ToList().MapToModel();
            _common.InsertSalesPoints(data);
            Pagination<CommunicationSetupModel> paginatedList = new Pagination<CommunicationSetupModel>(pageIndex, pageSize, list.TotalItemCount, data);
            return paginatedList;
        }

        public async Task<CommunicationSetupModel> GetById(int id)
        {
            var entry = await _communicationSetup.FindIncludeAsync(x => x.Id == id, i2 => i2.AvCommunication);
            if (entry is null) throw new AppException("Communication not found");
            var model = entry.MapToModel();
            model.SalesPoint = _salesPoint.GetAllActive().FirstOrDefault(x => x.SalesPointId == model.SalesPointId)
                ?.ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<CommunicationSetupModel> Update(CommunicationSetupModel payload)
        {
            CommunicationSetup communicationSetup = await _communicationSetup.FindAsync(x => x.Id == payload.Id);
            if (communicationSetup is null) throw new AppException("Communication setup not found");

            var existingCommunications = await GetExistingCommunicationSetups(new List<CommunicationSetupModel>() { payload });
            existingCommunications = existingCommunications.FindAll(x => x.Id != payload.Id);

            var fromDate = payload.FromDate;
            var toDate = payload.ToDate;
            if (existingCommunications.Count > 0)
            {
                var dateRangeViolatedSurveys = existingCommunications.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var spName = "";
                    if (payload.SalesPoint is object) spName = payload.SalesPoint.Name;
                    else spName = (await _salesPoint.FindAsync(x => x.SalesPointId == payload.SalesPointId)).Name;
                    throw new AppException($"The date range conflicts with existing Communication setup in {spName}");
                }

                var ids = existingCommunications.Select(x => x.Id).ToList();
                var communications = _communicationSetup.FindAll(x => ids.Contains(x.Id)).ToList();

                foreach (var c in communications)
                {
                    c.ToDate = fromDate.AddSeconds(-1);
                }
                await _communicationSetup.UpdateListAsync(communications);

            }
            if (communicationSetup.ToDate != payload.ToDate)
            {
                communicationSetup.ToDate = payload.ToDate;
            }
            if (communicationSetup.FromDate != payload.FromDate)
            {
                communicationSetup.FromDate = payload.FromDate;
            }
            if (communicationSetup.AvCommunicationId != payload.AvCommunicationId) communicationSetup.AvCommunicationId = payload.AvCommunicationId;
            if (communicationSetup.Status != payload.Status) communicationSetup.Status = payload.Status;

            await _communicationSetup.UpdateAsync(communicationSetup);
            return communicationSetup.MapToModel();
        }

        public async Task<List<CommunicationSetupModel>> GetExistingCommunicationSetups(List<CommunicationSetupModel> communicationSetups)
        {
            var list = await _common.GetExistingSetups(_communicationSetup,
                communicationSetups.Cast<BaseSetupModel>().ToList());

            return list.MapToModel();
        }


        public async Task<List<CommunicationSetupMBModel>> GetCommunicationSetupOfTodayByUser(int userId)
        {
            var user = await _cmUser.FindAsync(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.FindAll(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var list = _communicationSetup.FindAllInclude(x => x.Status == Status.Active && sIds.Contains(x.SalesPointId) && x.FromDate <= todayDate && todayDate <= x.ToDate &&
                                                               (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.AvCommunication).ToList();

            var brandIds = list.Select(x => x.AvCommunication.BrandId).Distinct().ToList();
            var brands = _brand.FindAll(x => brandIds.Contains(x.Id)).ToList();
            foreach (var l in list)
            {
                l.AvCommunication.Brand = brands.FirstOrDefault(x => x.Id == l.AvCommunication.BrandId);
                
            }
            var data = list.MapToMBModel();
            foreach (var d in data)
            {
               var result  = await GetOrCreateDailyTask(userId, new DailyTaskMBModel {DateTime = todayDate, SalesPointId = d.SalesPointId});
               d.DailyTaskId = result.Id;
            }

            var dailyTaskIds = data.Select(x => x.DailyTaskId).ToList();
            var submittedDailyCommunicationTask = _dailyCommunication.GetAllActive().Include(inc => inc.DailyTask)
                .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

            var submittedDailyTaskIds = submittedDailyCommunicationTask.Select(x => x.DailyTaskId).ToList();

            if (submittedDailyTaskIds.Count > 0)
                data = data.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

            return data;
        }

        public async Task<List<CommunicationSetupMBModel>> GetCommunicationSetupByTask(DailyTask task)
        {
            try
            {
                var user = await _cmUser.FindAsync(x => x.Id == task.CmUserId);

                AssignedUserType userType = (AssignedUserType)user.UserType;
                var list = _communicationSetup.FindAllInclude(x => x.Status == Status.Active && x.SalesPointId == task.SalesPointId && x.FromDate <= task.DateTime && task.DateTime <= x.ToDate &&
                                                                   (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.AvCommunication).ToList();

                var brandIds = list.Select(x => x.AvCommunication.BrandId).Distinct().ToList();
                var brands = _brand.FindAll(x => brandIds.Contains(x.Id)).ToList();
                foreach (var l in list)
                {
                    l.AvCommunication.Brand = brands.FirstOrDefault(x => x.Id == l.AvCommunication.BrandId);

                }
                var data = list.MapToMBModel();
                foreach (var d in data)
                {
                    var result = await GetOrCreateDailyTask(task.CmUserId, new DailyTaskMBModel { DateTime = task.DateTime, SalesPointId = d.SalesPointId });
                    d.DailyTaskId = result.Id;
                }

                var dailyTaskIds = data.Select(x => x.DailyTaskId).ToList();
                var submittedDailyCommunicationTask = _dailyCommunication.GetAllActive().Include(inc => inc.DailyTask)
                    .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

                var submittedDailyTaskIds = submittedDailyCommunicationTask.Select(x => x.DailyTaskId).ToList();

                if (submittedDailyTaskIds.Count > 0)
                    data = data.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

                return data;
            }
            catch (Exception e)
            {
                return new List<CommunicationSetupMBModel>();
            }
        }

        public async Task<bool> IsCommunicationSetUpActive(CommunicationSetupModel model)
        {
            return _communicationSetup.IsExist(comm => comm.Id == model.Id && comm.Code == model.Code && comm.Status == Status.Active);
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


    }
}
