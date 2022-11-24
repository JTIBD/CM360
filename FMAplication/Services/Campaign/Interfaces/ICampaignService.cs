using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Campaign;
using FMAplication.Models.Products;
using X.PagedList;

namespace FMAplication.Services.Campaign.Interfaces
{
    public interface ICampaignService
    {
        Task<CampaignModel> GetCampaignAsync(int id); 
        Task<IEnumerable<CampaignModel>> GetCampaignsAsync();
        Task<IPagedList<CampaignModel>> GetPagedCampaignsAsync(int pageNumber, int pageSize);
        Task<CampaignModel> CreateAsync(CampaignModel model);
        Task<CampaignModel> UpdateAsync(CampaignModel model);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<CampaignModel>> GetAllForSelectAsync();
    }
}
