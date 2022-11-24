using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.MobileModels;
using FMAplication.Models.Guidelines;
using FMAplication.RequestModels;

namespace FMAplication.Services.Guidelines.Interfaces
{
    public interface IGuidelineSetupService
    {
        Task<List<GuidelineSetupModel>> Create(List<GuidelineSetupModel> payload);
        Task<List<GuidelineSetupModel>> GetExistingGuidelineSetups(List<GuidelineSetupModel> guidelineSetups);
        Task<Pagination<GuidelineSetupModel>> GetAll(GetGuidelineSetupsRequestModel model);
        Task<GuidelineSetupModel> GetById(int id);
        Task<GuidelineSetupModel> Update(GuidelineSetupModel payload);
        Task<List<GuidelineSetupMBModel>> GetGuidelineSetupOfTodayByUser(int userId);
        Task<bool> IsGuidelineActive(GuidelineSetupModel model);
    }
}
