using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.Guidelines;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels;
using FMAplication.Models.Bases;
using FMAplication.Models.Guidelines;
using FMAplication.Models.Sales;
using FMAplication.Repositories;
using FMAplication.RequestModels;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.Guidelines.Interfaces;
using X.PagedList;

namespace FMAplication.Services.Guidelines.Implementation
{
    public class GuidelineSetupService : IGuidelineSetupService
    {
        private readonly IRepository<SalesPoint> _salesPointRepository;
        private readonly IRepository<GuidelineSetup> _guidelineSetupRepository;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<GuidelineSetup> _guidelineSetup;
        private readonly ICommonService _common;


        public GuidelineSetupService(IRepository<SalesPoint> salesPointRepository, IRepository<GuidelineSetup> guidelineSetupRepository, 
            IRepository<CMUser> cmUser, IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping, IRepository<GuidelineSetup> guidelineSetup, ICommonService common)
        {
            _salesPointRepository = salesPointRepository;
            _guidelineSetupRepository = guidelineSetupRepository;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _guidelineSetup = guidelineSetup;
            _common = common;
        }

        public async Task<List<GuidelineSetupModel>> Create(List<GuidelineSetupModel> payload)
        {
            if (payload is null || payload.Count == 0)
                throw new AppException("No Guideline Setup Provided");

            var fromDate = payload[0].FromDate;
            var toDate = payload[0].ToDate;
            _common.ValidateDateRange(fromDate, toDate);

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();
            var salesPoints = (await _salesPointRepository.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            var guidelineSetups = new List<GuidelineSetup>();
            var list = new List<GuidelineSetupModel>();

            foreach (var guideline in payload)
            {
                var salesPoint = salesPoints.Find(x => x.SalesPointId == guideline.SalesPointId);
                var guidelineSetup = new GuidelineSetup()
                {
                    Code = $"G_{salesPoint.Code}",
                    CreatedBy = AppIdentity.AppUser.UserId,
                    FromDate = guideline.FromDate,
                    ToDate = guideline.ToDate,
                    SalesPointId = salesPoint.SalesPointId,
                    UserType = guideline.UserType,
                    GuidelineText = guideline.GuidelineText
                };
                guidelineSetups.Add(guidelineSetup);
            }

            var existingModels = await GetExistingGuidelineSetups(payload);
            var existingModelIds = existingModels.Select(x => x.Id).ToList();
            var existingSetups = _guidelineSetupRepository.FindAll(x => existingModelIds.Contains(x.Id)).ToList();

            if (existingSetups.Count > 0)
            {
                var dateRangeViolatedSurveys = existingSetups.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var conflictingSps = salesPoints.FindAll(x => dateRangeViolatedSurveys.Any(v => x.SalesPointId == v.SalesPointId)).ToList();
                    var spNames = conflictingSps.Select(x => x.Name).ToList();
                    throw new AppException($"The date range conflicts with existing survey in {string.Join(",", spNames)}");
                }

                foreach (var srv in existingSetups)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _guidelineSetupRepository.UpdateListAsync(existingSetups);
            }

            await _guidelineSetupRepository.CreateListAsync(guidelineSetups);
            list = guidelineSetups.MapToModel();
            return list;
        }

        public async Task<Pagination<GuidelineSetupModel>> GetAll(GetGuidelineSetupsRequestModel model)
        {
            if (model.FromDateTime > model.ToDateTime) (model.FromDateTime, model.ToDateTime) = (model.ToDateTime, model.FromDateTime);

            var query = _guidelineSetupRepository.FindAllInclude(x => 
                x.FromDate >= model.FromDateTime && x.ToDate <= model.ToDateTime);

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                var sIds = _salesPointRepository.GetAll().Where(x => x.Code.Contains(model.Search) || x.Name.Contains(model.Search))
                    .Select(x => x.SalesPointId).ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId) || x.GuidelineText.Contains(model.Search));
            }

            if (model.SalesPointId != 0)
                query = query.Where(x => x.SalesPointId == model.SalesPointId);

            query = query.OrderByDescending(x => x.CreatedTime);
            var list = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
            var guidelineModels = list.ToList().MapToModel();
            _common.InsertSalesPoints(guidelineModels);
            var paginatedList = new Pagination<GuidelineSetupModel>(model.PageIndex, model.PageSize, list.TotalItemCount, guidelineModels);

            return paginatedList;
        }

        public async Task<GuidelineSetupModel> GetById(int id)
        {
            var result = await _guidelineSetupRepository.FindIncludeAsync(x => x.Id == id);
            if(result is null) { throw new AppException("Result not found");}
            var model = result.MapToModel();
            model.SalesPoint = _salesPointRepository.GetAllActive().FirstOrDefault(x => x.SalesPointId == model.SalesPointId)
                ?.ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<GuidelineSetupModel> Update(GuidelineSetupModel payload)
        {
            var guidelineSetup = await _guidelineSetupRepository.FindAsync(x => x.Id == payload.Id);
            if (guidelineSetup is null) throw new AppException("Guideline setup not found");

            var existingGuidelineSetups = await GetExistingGuidelineSetups(new List<GuidelineSetupModel>() { payload });
            existingGuidelineSetups = existingGuidelineSetups.FindAll(x => x.Id != payload.Id);

            var fromDate = payload.FromDate;
            var toDate = payload.ToDate;
            if (existingGuidelineSetups.Count > 0)
            {
                var dateRangeViolatedSurveys = existingGuidelineSetups.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var spName = "";
                    if (payload.SalesPoint is object) spName = payload.SalesPoint.Name;
                    else spName = _salesPointRepository.Find(x => x.SalesPointId == payload.SalesPointId).Name;
                    throw new AppException($"The date range conflicts with existing Guideline setup in {spName}");
                }

                var srvIds = existingGuidelineSetups.Select(x => x.Id).ToList();
                var serveys = _guidelineSetupRepository.FindAll(x => srvIds.Contains(x.Id)).ToList();

                foreach (var srv in serveys)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _guidelineSetupRepository.UpdateListAsync(serveys);
            }
            if (guidelineSetup.ToDate != payload.ToDate)
            {
                guidelineSetup.ToDate = payload.ToDate;
            }
            if (guidelineSetup.FromDate != payload.FromDate)
            {
                guidelineSetup.FromDate = payload.FromDate;
            }

            guidelineSetup.Status = payload.Status;

            guidelineSetup.GuidelineText = payload.GuidelineText;
            await _guidelineSetupRepository.UpdateAsync(guidelineSetup);
            return guidelineSetup.MapToModel();
        }

        public async Task<List<GuidelineSetupModel>> GetExistingGuidelineSetups(List<GuidelineSetupModel> guidelineSetups)
        {
            var list = await _common.GetExistingSetups(_guidelineSetupRepository, guidelineSetups.Cast<BaseSetupModel>().ToList());
            return list.MapToModel();
        }

        public async Task<List<GuidelineSetupMBModel>> GetGuidelineSetupOfTodayByUser(int userId)
        {
            var user = await _cmUser.FindAsync(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.FindAll(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow;
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var guideline = await _guidelineSetup.FindAllAsync(x => x.Status == Status.Active && sIds.Contains(x.SalesPointId) && x.FromDate <= todayDate && todayDate <= x.ToDate &&
                                                                 (x.UserType == AssignedUserType.BOTH || x.UserType == userType));

            var data = guideline.ToMap<GuidelineSetup, GuidelineSetupMBModel>();
            return data;
        }

        public async Task<bool> IsGuidelineActive(GuidelineSetupModel model)
        {
            return _guidelineSetup.IsExist(g => g.Id == model.Id && g.Code == model.Code && g.Status == Status.Active);
        }
    }
}
