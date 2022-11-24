using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Reasons;
using FMAplication.MobileModels;
using FMAplication.MobileModels.Reasons;

namespace FMAplication.Services.Reasons.Interfaces
{
    public interface IReasonService
    {
        Task<List<ReasonModel>> GetAllReasonsAsync();
        Task<List<ReasonsMBModel>> GetAllReasonForMobileAsync();
        Task<ReasonModel> GetReasonAsync(int id);
        Task<ReasonModel> CreateAsync(ReasonModel model);
        Task<ReasonModel> UpdateAsync(ReasonModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(string name, int id);
        Task<List<ReasonsWithType>> GetAllReasonsWithTypes();
        Task<List<ReasonTypeModel>> GetAllReasonTypes();
    }
}
