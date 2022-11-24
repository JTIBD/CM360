using FMAplication.common;
using FMAplication.Domain.Audit;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels.Audits;
using FMAplication.Models.Audits;
using FMAplication.Models.DailyAudit;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Products;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.DailyAudits.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using X.PagedList;

namespace FMAplication.Services.DailyAudits.Implementation
{
    public class DailyAuditService : IDailyAuditService
    {
        private readonly IRepository<DailyAudit> _dailyposm;
        private readonly IRepository<AuditSetup> _auditSetup;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<Product> _product;
        private readonly IRepository<AuditProduct> _auditProduct;
        private readonly IRepository<AuditPOSMProduct> _auditPOSMProduct;
        private readonly ICommonService _common;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailyAuditTask> _dailyAuditTask;

        public DailyAuditService(IRepository<DailyAudit> dailyposm, IRepository<AuditSetup> auditSetup,
            IRepository<SalesPoint> salesPoint, IRepository<CMUser> cmUser,
            IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping,ICommonService common,
            IRepository<POSMProduct> posmProduct, IRepository<Product> product, IRepository<AuditProduct> auditProduct,
            IRepository<AuditPOSMProduct> auditPOSMProduct, IRepository<DailyTask> dailyTask, 
            IRepository<DailyAuditTask> dailyAuditTask)
        {
            _dailyposm = dailyposm;
            _auditSetup = auditSetup;
            _common = common;
            _salesPoint = salesPoint;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _posmProduct = posmProduct;
            _product = product;
            _auditProduct = auditProduct;
            _auditPOSMProduct = auditPOSMProduct;
            _dailyTask = dailyTask;
            _dailyAuditTask = dailyAuditTask;
        }


        public async Task<DailyAuditModel> CreateAsync(DailyAuditModel model)
        {
            var example = model.ToMap<DailyAuditModel, DailyAudit>();
            var result = await _dailyposm.CreateAsync(example);
            return result.ToMap<DailyAudit, DailyAuditModel>();


        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _dailyposm.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync(int activityId, int id)
        {
            var result = id <= 0
                ? await _dailyposm.IsExistAsync(s => s.DailyCMActivityId == activityId)
                : await _dailyposm.IsExistAsync(s => s.DailyCMActivityId == activityId && s.Id != id);

            return result;
        }

        public async Task<bool> IsAuditSetupActive(int id, string code)
        {
            return await _auditSetup.IsExistAsync(s => s.Id == id && s.Code == code && s.Status == Status.Active);
        }

        public async Task<DailyAuditModel> GetDailyAuditAsync(int id)
        {
            var result = await _dailyposm.FindAsync(s => s.Id == id);
            return result.ToMap<DailyAudit, DailyAuditModel>();
        }

        public async Task<IEnumerable<DailyAuditModel>> GetDailyAuditAsync()
        {
            var result = await _dailyposm.GetAllAsync();
            return result.ToMap<DailyAudit, DailyAuditModel>();
        }

        public async Task<IPagedList<DailyAuditModel>> GetPagedDailyAuditAsync(int pageNumber, int pageSize)
        {
            var result = await _dailyposm.GetAll().OrderBy(s => s.Id).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<DailyAudit, DailyAuditModel>();

        }

        public async Task<IEnumerable<DailyAuditModel>> GetQueryDailyAuditAsync()
        {
            var result = await _dailyposm.ExecuteQueryAsyc<DailyAuditModel>("SELECT * FROM Examples");
            return result;
        }

        public async Task<DailyAuditModel> SaveAsync(DailyAuditModel model)
        {
            var example = model.ToMap<DailyAuditModel, DailyAudit>();
            var result = await _dailyposm.CreateOrUpdateAsync(example);
            return result.ToMap<DailyAudit, DailyAuditModel>();
        }

        public async Task<DailyAuditModel> UpdateAsync(DailyAuditModel model)
        {
            var example = model.ToMap<DailyAuditModel, DailyAudit>();
            var result = await _dailyposm.UpdateAsync(example);
            return result.ToMap<DailyAudit, DailyAuditModel>();
        }

        public async Task<IEnumerable<DropdownOptions>> GetDropdownValueAsync()
        {
            var userId = AppIdentity.AppUser.UserId;

            userId = 1;

            SqlParameter param = new SqlParameter("@id", userId);

            var result = await _dailyposm.ExecuteQueryAsyc<DropdownOptions>("exec [dbo].[GetCMActivityId] @id", param);



            return result;
        }

        public async Task<List<AuditSetupModel>> Create(List<AuditSetupModel> payload)
        {
            if (payload is null || payload.Count == 0) throw new AppException("No auditSetup provided");
            List<AuditSetupModel> list = new List<AuditSetupModel>();

            DateTime fromDate = payload[0].FromDate;
            DateTime toDate = payload[0].ToDate;

            _common.ValidateDateRange(fromDate, toDate);

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();

            var salespoints = (await _salesPoint.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            List<AuditSetup> auditSetups = new List<AuditSetup>();

            var user = AppIdentity.AppUser;

            foreach (var s in payload)
            {
                var sp = salespoints.Find(x => x.SalesPointId == s.SalesPointId);

                AuditSetup sv = new AuditSetup()
                {
                    Code = $"ADT_{sp.Code}",
                    CreatedBy = user.UserId,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate,
                    SalesPointId = sp.SalesPointId,
                    UserType = s.UserType,
                };
                auditSetups.Add(sv);
            }
            
            var existingAuditSetupModels = await GetExistingAuditSetups(payload);
            var existingAuditSetupIds = existingAuditSetupModels.Select(x => x.Id).ToList();
            var existingAuditSetups = _auditSetup.FindAll(x => existingAuditSetupIds.Contains(x.Id)).ToList();

            if (existingAuditSetups.Count > 0)
            {
                var dateRangeViolatedAuditSetups = existingAuditSetups.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedAuditSetups.Count > 0)
                {
                    var conflictingSps = salespoints.FindAll(x => dateRangeViolatedAuditSetups.Any(v => x.SalesPointId == v.SalesPointId)).ToList();
                    var spNames = conflictingSps.Select(x => x.Name).ToList();
                    throw new AppException($"The date range conflicts with existing auditSetup in {string.Join(",", spNames)}");
                }

                foreach (var srv in existingAuditSetups)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _auditSetup.UpdateListAsync(existingAuditSetups);
            }

            await _auditSetup.CreateListAsync(auditSetups);

            await CreateAuditProducts(auditSetups, payload);

            list = auditSetups.MapToModel();

            return list;
        }

        private async Task UpdateAuditProducts(AuditSetupModel setUpModel)
        {
            if (setUpModel.AuditProducts is object)
            {
                var existingAuditProducts = await _auditProduct.FindAll(x => x.AuditSetupId == setUpModel.Id).ToListAsync();
                var existingAuditProductIds = existingAuditProducts.Select(x => x.Id).ToList();
                var removedAuditProducts = existingAuditProducts.FindAll(existing => !setUpModel.AuditProducts.Any(ap => ap.Id == existing.Id));
                if (removedAuditProducts.Any()) await _auditProduct.DeleteListAsync(removedAuditProducts);
                var newAuditProducts = setUpModel.AuditProducts.FindAll(newP => newP.Id == 0).ToMap<AuditProductModel,AuditProduct>();
                await _auditProduct.CreateListAsync(newAuditProducts);
            }
            if (setUpModel.AuditPOSMProducts is object)
            {
                var existingAuditPOSMProducts = await _auditPOSMProduct.FindAll(x => x.AuditSetupId == setUpModel.Id).ToListAsync();
                var removedAuditPOSMProducts = existingAuditPOSMProducts.FindAll(existing => !setUpModel.AuditPOSMProducts.Any(ap => ap.Id == existing.Id));
                if (removedAuditPOSMProducts.Any()) await _auditPOSMProduct.DeleteListAsync(removedAuditPOSMProducts);
                var newPOSMAuditProducts = setUpModel.AuditPOSMProducts.FindAll(existing => existing.Id == 0).ToMap<AuditPOSMProductModel,AuditPOSMProduct>();
                await _auditPOSMProduct.CreateListAsync(newPOSMAuditProducts);
            }

        }
        private async Task CreateAuditProducts(List<AuditSetup> setups,List<AuditSetupModel> setUpModels)
        {
            List<AuditProduct> auditProducts = new List<AuditProduct>();
            List<AuditPOSMProduct> auditPOSMProducts = new List<AuditPOSMProduct>();
            foreach(var setup in setUpModels)
            {
                var setUpId = setups.Find(x => x.SalesPointId == setup.SalesPointId).Id;
                foreach(var product in setup.AuditProducts)
                {
                    AuditProduct auditProduct = new AuditProduct()
                    {
                        AuditSetupId = setUpId,
                        ProductId = product.ProductId,
                        ActionType = product.ActionType
                    };
                    auditProducts.Add(auditProduct);
                    product.AuditSetupId = setUpId;
                }
                foreach (var posm in setup.AuditPOSMProducts)
                {
                    AuditPOSMProduct auditPOSMProduct = new AuditPOSMProduct()
                    {
                        AuditSetupId = setUpId,
                        POSMProductId = posm.POSMProductId,
                        ActionType = posm.ActionType,
                    };
                    auditPOSMProducts.Add(auditPOSMProduct);
                    posm.AuditSetupId = setUpId;
                }

            }
            await _auditProduct.CreateListAsync(auditProducts);
            await _auditPOSMProduct.CreateListAsync(auditPOSMProducts);
        }

        public async Task<Pagination<AuditSetupModel>> GetAuditSetups(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salesPointId)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _auditSetup.FindAllInclude(x => x.FromDate >= fromDateTime && x.ToDate <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var sIds = _salesPoint.GetAll().Where(x => x.Code.Contains(search) || x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId));
            }
            if (salesPointId != 0) query = query.Where(x => x.SalesPointId == salesPointId);

            query = query.OrderByDescending(x => x.CreatedTime);
            var auditSetups = await query.ToPagedListAsync(pageIndex, pageSize);
            var auditSetupModels = auditSetups.ToList().MapToModel();
            var auditSetupIds = auditSetups.Select(x => x.Id).ToList();

            var auditProducts = _auditProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x=>x.Product).ToList().MapToModel();
            var auditPOSMProducts = _auditPOSMProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x=>x.POSMProduct).ToList().MapToModel();
            auditSetupModels.ForEach(x =>
            {
                x.AuditProducts = auditProducts.FindAll(ap => ap.AuditSetupId == x.Id);
                x.AuditPOSMProducts= auditPOSMProducts.FindAll(ap => ap.AuditSetupId == x.Id);
            });
            _common.InsertSalesPoints(auditSetupModels);

            Pagination<AuditSetupModel> paginatedList = new Pagination<AuditSetupModel>(pageIndex, pageSize, auditSetups.TotalItemCount, auditSetupModels);

            return paginatedList;
        }

        public async Task<AuditSetupModel> GetAuditSetupById(int id)
        {
            var auditSetup = await _auditSetup.FindIncludeAsync(x => x.Id == id);
            if (auditSetup is null) throw new AppException("AuditSetup not found");
            var model = auditSetup.MapToModel();            
            model.AuditPOSMProducts = _auditPOSMProduct.FindAll(x => x.AuditSetupId == model.Id).Include(x=>x.POSMProduct).ToList().MapToModel();
            model.AuditProducts= _auditProduct.FindAll(x => x.AuditSetupId == model.Id).Include(x=>x.Product).ToList().MapToModel();
            model.SalesPoint = _salesPoint.GetAllActive().FirstOrDefault(x => x.SalesPointId == model.SalesPointId)
                .ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<AuditSetupModel> UpdateAuditSetup(AuditSetupModel payload)
        {
            AuditSetup auditSetup = await _auditSetup.FindAsync(x => x.Id == payload.Id);
            if (auditSetup is null) throw new AppException("AuditSetup not found");
            var existingAuditSetups = await GetExistingAuditSetups(new List<AuditSetupModel>() { payload });
            existingAuditSetups = existingAuditSetups.FindAll(x => x.Id != payload.Id);
            var fromDate = payload.FromDate;
            var toDate = payload.ToDate;
            if (existingAuditSetups.Count > 0)
            {
                var dateRangeViolatedAuditSetups = existingAuditSetups.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedAuditSetups.Count > 0)
                {
                    var spName = "";
                    if (payload.SalesPoint is object) spName = payload.SalesPoint.Name;
                    else spName = _salesPoint.Find(x => x.SalesPointId == payload.SalesPointId).Name;
                    throw new AppException($"The date range conflicts with existing auditSetup in {spName}");
                }

                var auditIds = existingAuditSetups.Select(x => x.Id).ToList();
                var auditSetups = _auditSetup.FindAll(x => auditIds.Contains(x.Id)).ToList();

                foreach (var srv in auditSetups)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _auditSetup.UpdateListAsync(auditSetups);
            }
            if (auditSetup.ToDate != payload.ToDate)
            {
                auditSetup.ToDate = payload.ToDate;
            }
            if (auditSetup.FromDate != payload.FromDate)
            {
                auditSetup.FromDate = payload.FromDate;
            }
            if (auditSetup.Status != payload.Status)
            {
                auditSetup.Status = payload.Status;
            }
            await _auditSetup.UpdateAsync(auditSetup);
            await UpdateAuditProducts(payload);
            return auditSetup.MapToModel();
        }

        public async Task<List<AuditSetupModel>> GetExistingAuditSetups(List<AuditSetupModel> auditSetups)
        {
            var existingAuditSetups =
                await _common.GetExistingSetups(_auditSetup, auditSetups.Cast<BaseSetupModel>().ToList());

            return existingAuditSetups.MapToModel();
        }

        public async Task<List<AuditSetupMBModel>> GetAuditSetupsOfTodayByUser(int userId)
        {
            var user = await _cmUser.FindAsync(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.FindAll(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var auditSetups = _auditSetup.FindAllInclude(x => x.Status == Status.Active && sIds.Contains(x.SalesPointId) && x.FromDate <= todayDate && todayDate <= x.ToDate &&
                (x.UserType == AssignedUserType.BOTH || x.UserType == userType)).ToList();

            var auditSetupModels = auditSetups.MapToMBModel();
            var auditSetupIds = auditSetups.Select(x => x.Id).ToList();

            var auditProducts = _auditProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x => x.Product).ToList().MapToModel();
            var auditPOSMProducts = _auditPOSMProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x => x.POSMProduct).ToList().MapToModel();
            auditSetupModels.ForEach(x =>
            {
                x.AuditProducts = auditProducts.FindAll(ap => ap.AuditSetupId == x.Id);
                x.AuditPOSMProducts = auditPOSMProducts.FindAll(ap => ap.AuditSetupId == x.Id);
            });
            await CreateDailyTasks(auditSetupModels, userId, todayDate);


            var dailyTaskIds = auditSetupModels.Select(x => x.DailyTaskId).ToList();
            var submittedDailyAduitTask = _dailyAuditTask.GetAllActive().Include(inc => inc.DailyTask)
                .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

            var submittedDailyTaskIds = submittedDailyAduitTask.Select(x => x.DailyTaskId).ToList();

            if (submittedDailyTaskIds.Count > 0)
                auditSetupModels = auditSetupModels.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

            return auditSetupModels;
        }

        public async Task<List<AuditSetupMBModel>> GetAuditSetupsByTask(DailyTask task)
        {
            try
            {
                var user = await _cmUser.FindAsync(x => x.Id == task.CmUserId);
           
                AssignedUserType userType = (AssignedUserType)user.UserType;
                var auditSetups = _auditSetup.FindAllInclude(x => x.Status == Status.Active && x.SalesPointId == task.SalesPointId && x.FromDate <= task.DateTime && task.DateTime <= x.ToDate &&
                                                                  (x.UserType == AssignedUserType.BOTH || x.UserType == userType)).ToList();

                var auditSetupModels = auditSetups.MapToMBModel();
                var auditSetupIds = auditSetups.Select(x => x.Id).ToList();

                var auditProducts = _auditProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x => x.Product).ToList().MapToModel();
                var auditPOSMProducts = _auditPOSMProduct.FindAll(x => auditSetupIds.Contains(x.AuditSetupId)).Include(x => x.POSMProduct).ToList().MapToModel();
                auditSetupModels.ForEach(x =>
                {
                    x.AuditProducts = auditProducts.FindAll(ap => ap.AuditSetupId == x.Id);
                    x.AuditPOSMProducts = auditPOSMProducts.FindAll(ap => ap.AuditSetupId == x.Id);
                });
          
                var dailyTaskIds = auditSetupModels.Select(x => x.DailyTaskId).ToList();
                var submittedDailyAduitTask = _dailyAuditTask.GetAllActive().Include(inc => inc.DailyTask)
                    .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

                var submittedDailyTaskIds = submittedDailyAduitTask.Select(x => x.DailyTaskId).ToList();

                if (submittedDailyTaskIds.Count > 0)
                    auditSetupModels = auditSetupModels.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

                return auditSetupModels;
            }
            catch (Exception e)
            {
                return new List<AuditSetupMBModel>();
            }
        }

        private async Task CreateDailyTasks(List<AuditSetupMBModel> setups,int userId, DateTime todayDate)
        {
            var sPIds = setups.Select(x => x.SalesPointId).ToList();
            var existingTasks = _dailyTask.FindAll(x => sPIds.Contains(x.SalesPointId) && x.DateTime.Date == todayDate.Date && x.CmUserId == userId).ToList();
            if(existingTasks.Any()) existingTasks.ForEach(t => setups.Find(x => x.SalesPointId == t.SalesPointId).DailyTaskId = t.Id);
            var setupsWithoutTaks = setups.FindAll(x => x.DailyTaskId == 0);
            List<DailyTask> newTaks = new List<DailyTask>();
            setupsWithoutTaks.ForEach(st =>
            {
                DailyTask dailyTask = new DailyTask()
                {
                    CmUserId = userId,
                    SalesPointId = st.SalesPointId,
                    DateTime = todayDate
                };
                newTaks.Add(dailyTask);
            });
            await _dailyTask.CreateListAsync(newTaks);
            newTaks.ForEach(nt => setupsWithoutTaks.Find(x => x.SalesPointId == nt.SalesPointId).DailyTaskId = nt.Id);
        }
    }
}
