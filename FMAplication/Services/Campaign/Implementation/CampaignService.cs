using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Campaign;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Campaign;
using FMAplication.Models.Products;
using FMAplication.Repositories;
using FMAplication.Services.Campaign.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.Campaign.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Domain.Campaign.Campaign> _campaign;
        private readonly IRepository<CampaignHistory> _campaignHistory;
        private readonly IRepository<POSMProduct> _posmProduct;

        public CampaignService(IRepository<Domain.Campaign.Campaign> campaign, IRepository<Domain.Campaign.CampaignHistory> campaignHistory, IRepository<POSMProduct> posmProduct)
        {
            _campaign = campaign;
            _campaignHistory = campaignHistory;
            _posmProduct = posmProduct;
        }

        public async Task<CampaignModel> GetCampaignAsync(int id)
        {
            var result = await _campaign.GetFirstOrDefaultIncludeAsync(x => x, s => s.Id == id, x => x.Include(i => i.CampaignHistories), true);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Campaign.Campaign, CampaignModel>();
                cfg.CreateMap<CampaignHistory, CampaignHistoryModel>();
            }).CreateMapper();

            return mapperToModel.Map<CampaignModel>(result);
        }

        public async Task<IEnumerable<CampaignModel>> GetCampaignsAsync()
        {
            var result = await _campaign.GetAllAsync();
            return result.ToMap<Domain.Campaign.Campaign, CampaignModel>();
        }

        public async Task<IEnumerable<CampaignModel>> GetAllForSelectAsync()
        {
            var result = await _campaign.FindAllAsync(x => x.Status == Status.Active);
            return result.ToMap<Domain.Campaign.Campaign, CampaignModel>();
        }

        public async Task<IPagedList<CampaignModel>> GetPagedCampaignsAsync(int pageNumber, int pageSize)
        {
            var result = await _campaign.GetAll().OrderByDescending(s => s.CreatedTime).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<Domain.Campaign.Campaign, CampaignModel>();
        }

        public async Task<CampaignModel> CreateAsync(CampaignModel model)
        {

            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CampaignModel, Domain.Campaign.Campaign>()
                    .ForMember(dest => dest.CampaignHistories, opt => opt.Ignore());
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Campaign.Campaign>(model);

            var result = await _campaign.CreateAsync(entity);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Campaign.Campaign, CampaignModel>()
                    .ForMember(dest => dest.CampaignHistories, opt => opt.Ignore());
            }).CreateMapper();

            return mapperToModel.Map<CampaignModel>(entity);
        }

        public async Task<CampaignModel> UpdateAsync(CampaignModel model)
        {
            #region Update History
            var camp = await _campaign.FindAsync(x => x.Id == model.Id);
            if(camp != null && (camp.StartDate != model.StartDate || camp.EndDate != model.EndDate))
            {
                var campHistory = new CampaignHistory();
                campHistory.CampaignId = camp.Id;
                campHistory.StartDate = camp.StartDate;
                campHistory.EndDate = camp.EndDate;
                campHistory.Status = camp.Status;

                await _campaignHistory.CreateAsync(campHistory);
            }
            #endregion

            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CampaignModel, Domain.Campaign.Campaign>()
                    .ForMember(dest => dest.CampaignHistories, opt => opt.Ignore());
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Campaign.Campaign>(model);

            var result = await _campaign.UpdateAsync(entity); 
            
            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Campaign.Campaign, CampaignModel>()
                    .ForMember(dest => dest.CampaignHistories, opt => opt.Ignore());
            }).CreateMapper();

            return mapperToModel.Map<CampaignModel>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReference(id);
            var result = await _campaign.DeleteAsync(s => s.Id == id);
            return result;

        }

        private async Task CheckReference(int campaignId)
        {
            var posms = await _posmProduct.GetAll().CountAsync(x => x.CampaignId == campaignId);
            var references = new List<string>();
            if (posms > 0) references.Add("POSM product");
            if (references.Any()) throw new AppException($"The campaign has been used in {string.Join(", ", references)}");
        }
    }
}
