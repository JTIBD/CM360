using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Brand;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Brand;
using FMAplication.Models.Products;
using FMAplication.Repositories;
using FMAplication.Services.Brand.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.Brand.Implementation
{
    public class SubBrandService : ISubBrandService
    {
        private readonly IRepository<Domain.Brand.SubBrand> _subbrand;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<Product> _product;

        public SubBrandService(IRepository<Domain.Brand.SubBrand> subbrand, IRepository<POSMProduct> posmProduct, IRepository<Product> product)
        {
            _subbrand = subbrand;
            _posmProduct = posmProduct;
            _product = product;
        }

        public async Task<SubBrandModel> GetSubBrandAsync(int id)
        {
            var result = await _subbrand.FindAsync(s => s.Id == id);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.SubBrand, SubBrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<SubBrandModel>(result);
        }

        public async Task<IEnumerable<SubBrandModel>> GetSubBrandsAsync()
        {
            var result = await _subbrand.GetAllAsync();
            return result.ToMap<Domain.Brand.SubBrand, SubBrandModel>();
        }

        public async Task<IEnumerable<SubBrandModel>> GetAllForSelectAsync(int brandId)
        {
            var result = await _subbrand.FindAllAsync(x => x.Status == Status.Active && x.BrandId == brandId);
            return result.ToMap<Domain.Brand.SubBrand, SubBrandModel>();
        }

        public async Task<IPagedList<SubBrandModel>> GetPagedSubBrandsAsync(int pageNumber, int pageSize)
        {
            var result = _subbrand.GetAllIncludeStrFormat(includeProperties: "Brand", orderBy: x => x.OrderByDescending(s => s.CreatedTime), 
                                                                skip: pageNumber-1, take: pageSize);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.SubBrand, SubBrandModel>();
                cfg.CreateMap<Domain.Brand.Brand, BrandModel>();
            }).CreateMapper();

            var mapResult = mapperToModel.Map<IEnumerable<SubBrandModel>>(result);

            var mapResultCount = mapResult.Count() == 0 ? 1 : mapResult.Count();

            return new StaticPagedList<SubBrandModel>(mapResult, 1, mapResultCount, mapResultCount);
        }

        public async Task<SubBrandModel> CreateAsync(SubBrandModel model)
        {

            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubBrandModel, Domain.Brand.SubBrand>()
                    .ForMember(dest => dest.Brand, opt => opt.Ignore());
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Brand.SubBrand>(model);

            var result = await _subbrand.CreateAsync(entity);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.SubBrand, SubBrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<SubBrandModel>(entity);
        }

        public async Task<SubBrandModel> UpdateAsync(SubBrandModel model)
        {
            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubBrandModel, Domain.Brand.SubBrand>()
                    .ForMember(dest => dest.Brand, opt => opt.Ignore());
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Brand.SubBrand>(model);

            var result = await _subbrand.UpdateAsync(entity); 
            
            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.SubBrand, SubBrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<SubBrandModel>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReference(id);
            var result = await _subbrand.DeleteAsync(s => s.Id == id);
            return result;
        }

        private async Task CheckReference(int subBrandId)
        {
            var posms = await _posmProduct.GetAll().CountAsync(x => x.SubBrandId == subBrandId);
            var products = await _product.GetAll().CountAsync(x => x.SubBrandId == subBrandId);
            var references = new List<string>();
            if (posms > 0) references.Add("POSM product");
            if (products > 0) references.Add("Product");
            if (references.Any()) throw new AppException($"The sub brand has been used in {string.Join(", ", references)}");
        }
    }
}
