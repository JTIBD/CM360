using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.ExecutionLimits;
using FMAplication.Domain.Guidelines;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels.ExecutionLimits;
using FMAplication.Models.ExecutionLimits;
using FMAplication.Models.Guidelines;
using FMAplication.Models.Sales;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.ExecutionLimit.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using MinimumExecutionLimitExtensions = FMAplication.Models.ExecutionLimits.MinimumExecutionLimitExtensions;

namespace FMAplication.Services.ExecutionLimit.Implementations
{
    public class MinimumExecutionLimitService : IMinimumExecutionLimitService
    {
        private readonly IRepository<SalesPoint> _salesPointRepository;
        private readonly IRepository<MinimumExecutionLimit> _minimumExecutionRepository;
        private readonly IRepository<CmsUserSalesPointMapping> _cmUserSalesPoint;
        private ICommonService _common;

        public MinimumExecutionLimitService(IRepository<SalesPoint> salesPointRepository, 
            IRepository<MinimumExecutionLimit> minimumExecutionRepository, IRepository<CmsUserSalesPointMapping> cmUserSalesPoint, ICommonService common)
        {
            _salesPointRepository = salesPointRepository;
            _minimumExecutionRepository = minimumExecutionRepository;
            _cmUserSalesPoint = cmUserSalesPoint;
            _common = common;
        }

        public async Task<Pagination<MinimumExecutionLimitModel>> GetAll(GetMinimumExecutionLimitModel model)
        {
            var query = _minimumExecutionRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(model.SearchText))
            {
                var sIds = _salesPointRepository.GetAllActive().Where(x => x.Code.Contains(model.SearchText) || x.Name.Contains(model.SearchText))
                    .Select(x => x.SalesPointId).ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId));
            }
                

            if (model.SalesPointId != 0)
                query = query.Where(x => x.SalesPointId == model.SalesPointId);

            query = query.OrderByDescending(x => x.CreatedTime);
            var list = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
            var minimumExecutionLimitModels= MinimumExecutionLimitExtensions.MapToModel(list.ToList());
            _common.InsertSalesPoints(minimumExecutionLimitModels);
            var paginatedList = new Pagination<MinimumExecutionLimitModel>(model.PageIndex, model.PageSize, list.TotalItemCount, minimumExecutionLimitModels);

            return paginatedList;
        }
        public async Task<List<MinimumExecutionLimitMBModel>> GetAll(int userId)
        {
            var salesPoints = await _cmUserSalesPoint.FindAll(x => x.CmUserId == userId).ToListAsync();
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var result = await _minimumExecutionRepository.GetAll().Where(x=> 
                                                    salesPointIds.Contains(x.SalesPointId)).ToListAsync();

            return result.ToMap<MinimumExecutionLimit, MinimumExecutionLimitMBModel>();
        }

        public async Task<List<MinimumExecutionLimitModel>> Create(List<MinimumExecutionLimitModel> payload)
        {
            if (payload is null || payload.Count == 0)
                throw new AppException("No Target Visited Outlet provided");

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();
            var salesPoints = (await _salesPointRepository.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            var minimumExecutionLimits = new List<MinimumExecutionLimit>();
            var list = new List<MinimumExecutionLimitModel>();

            foreach (var item in payload)
            {
                var salesPoint = salesPoints.Find(x => x.SalesPointId == item.SalesPointId);
                
                var minimumExecutionLimit = new MinimumExecutionLimit()
                {
                    Id = item.Id,
                    Code = $"M_{salesPoint.Code}",
                    CreatedBy = AppIdentity.AppUser.UserId,
                    SalesPointId = salesPoint.SalesPointId,
                    TargetVisitedOutlet = item.TargetVisitedOutlet
                };
                minimumExecutionLimits.Add(minimumExecutionLimit);
            }

            var existingModels = await GetExistingMinimumExecutionLimit(payload);
            var existingModelIds = existingModels.Select(x => x.Id).ToList();
            var existingSetups = _minimumExecutionRepository.FindAll(x => existingModelIds.Contains(x.Id)).ToList();
            
            if (existingSetups.Count > 0)
            {
                existingSetups.ForEach(x => { x.TargetVisitedOutlet = payload[0].TargetVisitedOutlet;});
                await _minimumExecutionRepository.UpdateListAsync(existingSetups);
                var updatedLimits = await minimumExecutionLimits.Where(x => existingSetups.Any(y => y.SalesPointId == x.SalesPointId)).ToListAsync();
                foreach (var item in updatedLimits)
                {
                    minimumExecutionLimits.Remove(item);
                }
            }

            await _minimumExecutionRepository.CreateListAsync(minimumExecutionLimits);
            list = MinimumExecutionLimitExtensions.MapToModel(minimumExecutionLimits);
            return list;
        }

        public async Task<List<MinimumExecutionLimitModel>> GetExistingMinimumExecutionLimit(List<MinimumExecutionLimitModel> models)
        {
            if (models is null || models.Count == 0) throw new AppException("No data provided");

            var salesPointIds = models.Select(sv => sv.SalesPointId).ToList();
            var list = (await _minimumExecutionRepository.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            return MinimumExecutionLimitExtensions.MapToModel(list);
        }

        public async Task<MinimumExecutionLimitModel> GetById(int id)
        {
            var result = await _minimumExecutionRepository.FindIncludeAsync(x => x.Id == id);
            if (result is null) { throw new AppException("Result not found"); }
            var model = MinimumExecutionLimitExtensions.MapToModel(result);
            model.SalesPoint = _salesPointRepository.GetAllActive()
                .FirstOrDefault(x => x.SalesPointId == model.SalesPointId)?.ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<MinimumExecutionLimitModel> Update(MinimumExecutionLimitModel payload)
        {
            var minimumExecutionLimit = await _minimumExecutionRepository.FindAsync(x => x.Id == payload.Id);
            if (minimumExecutionLimit is null) throw new AppException("Target Visited Outlet not found");

            minimumExecutionLimit.TargetVisitedOutlet = payload.TargetVisitedOutlet;
            await _minimumExecutionRepository.UpdateAsync(minimumExecutionLimit);
            return MinimumExecutionLimitExtensions.MapToModel(minimumExecutionLimit);
        }

        public async Task<int> Delete(int id)
        {
            var result = await _minimumExecutionRepository.DeleteAsync(x => x.Id == id);
            return result;
        }
    }
}
