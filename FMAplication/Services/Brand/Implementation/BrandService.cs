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
    public class BrandService : IBrandService
    {
        private readonly IRepository<Domain.Brand.Brand> _brand;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<Product> _product;
        private readonly IRepository<SubBrand> _subBrand;

        public BrandService(IRepository<Domain.Brand.Brand> brand, IRepository<POSMProduct> posmProduct, IRepository<Product> product, IRepository<SubBrand> subBrand)
        {
            _brand = brand;
            _posmProduct = posmProduct;
            _product = product;
            _subBrand = subBrand;
        }

        public async Task<BrandModel> GetBrandAsync(int id)
        {
            var result = await _brand.FindAsync(s => s.Id == id);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.Brand, BrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<BrandModel>(result);
        }

        public async Task<IEnumerable<BrandModel>> GetBrandsAsync()
        {
            var result = await _brand.GetAllAsync();
            return result.ToMap<Domain.Brand.Brand, BrandModel>();
        }

        public async Task<IEnumerable<BrandModel>> GetAllForSelectAsync()
        {
            var result = await _brand.FindAllAsync(x => x.Status == Status.ActiveBrand || x.Status == Status.Active);
            return result.ToMap<Domain.Brand.Brand, BrandModel>();
        }

        public async Task<IPagedList<BrandModel>> GetPagedBrandsAsync(int pageNumber, int pageSize)
        {
            var result = await _brand.GetAll().OrderByDescending(s => s.CreatedTime).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<Domain.Brand.Brand, BrandModel>();
        }

        public async Task<BrandModel> CreateAsync(BrandModel model)
        {

            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandModel, Domain.Brand.Brand>();
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Brand.Brand>(model);

            var result = await _brand.CreateAsync(entity);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.Brand, BrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<BrandModel>(entity);
        }

        public async Task<BrandModel> UpdateAsync(BrandModel model)
        {
            var mapperToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandModel, Domain.Brand.Brand>();
            }).CreateMapper();

            var entity = mapperToEntity.Map<Domain.Brand.Brand>(model);

            var result = await _brand.UpdateAsync(entity); 
            
            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Brand.Brand, BrandModel>();
            }).CreateMapper();

            return mapperToModel.Map<BrandModel>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReference(id);
            var result = await _brand.DeleteAsync(s => s.Id == id);
            return result;

        }

        private async Task CheckReference(int brandId)
        {
            var posms = await _posmProduct.GetAll().CountAsync(x => x.BrandId == brandId);
            var products = await _product.GetAll().CountAsync(x => x.BrandId == brandId);
            var subBrands = await _subBrand.GetAll().CountAsync(x => x.BrandId == brandId);
            var references = new List<string>();
            if(posms > 0) references.Add("POSM product");
            if(products > 0) references.Add("Product");
            if(subBrands > 0) references.Add("SubBrand");
            if (references.Any()) throw new AppException($"The brand has been used in {string.Join(", ", references)}");
        }
    }
}
