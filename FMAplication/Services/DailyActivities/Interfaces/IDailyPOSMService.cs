using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
namespace FMAplication.Services.Interfaces
{
   public interface IDailyPOSMService
    {
        Task<IEnumerable<DailyPOSMModel>> GetDailyPOSMAsync();
        Task<IPagedList<DailyPOSMModel>> GetPagedDailyPOSMAsync(int pageNumber, int pageSize);
        Task<IEnumerable<DailyPOSMModel>> GetQueryDailyPOSMAsync();
        Task<DailyPOSMModel> GetDailyPOSMAsync(int id);
        Task<DailyPOSMModel> SaveAsync(DailyPOSMModel model);
        Task<DailyPOSMModel> CreateAsync(DailyPOSMModel model);
        Task<DailyPOSMModel> UpdateAsync(DailyPOSMModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(int activityId, int id);
        Task<IEnumerable<DropdownOptions>> GetDropdownValueAsync();
    }
}
