using FMAplication.Models.ExecutionLimits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.MobileModels.ExecutionLimits;
using X.PagedList;

namespace FMAplication.Services.ExecutionLimit.Interfaces
{
    public interface IMinimumExecutionLimitService
    {
        Task<Pagination<MinimumExecutionLimitModel>> GetAll(GetMinimumExecutionLimitModel model);
        Task<List<MinimumExecutionLimitMBModel>> GetAll(int userId);
        Task<List<MinimumExecutionLimitModel>> Create(List<MinimumExecutionLimitModel> payload);
        Task<List<MinimumExecutionLimitModel>> GetExistingMinimumExecutionLimit(List<MinimumExecutionLimitModel> models);
        Task<MinimumExecutionLimitModel> GetById(int id);
        Task<MinimumExecutionLimitModel> Update(MinimumExecutionLimitModel payload);
        Task<int> Delete(int id); 
    }
}
