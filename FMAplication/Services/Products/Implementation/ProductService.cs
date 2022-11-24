using System;
using FMAplication.Domain.Products;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Repositories;
using FMAplication.Services.Products.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Audit;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Models.AvCommunications;
using FMAplication.Services.AzureStorageService.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.Products.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _Product;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IRepository<AuditProduct> _auditProducts;

        public ProductService(IRepository<Product> example, IBlobStorageService blobStorageService, IRepository<AuditProduct> auditProducts)
        {
            _Product = example;
            _blobStorageService = blobStorageService;
            _auditProducts = auditProducts;
        }


        public async Task<ProductModel> CreateAsync(ProductModel model)
        {
            var imagePath = await GetFilePath(model);
            var example = model.ToMap<ProductModel, Product>();
            example.ImageUrl =  imagePath;
            var result = await _Product.CreateAsync(example);
            return result.ToMap<Product, ProductModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReference(id);
            var result = await _Product.DeleteAsync(s => s.Id == id);
            return result;

        }

        private async Task CheckReference(int productId)
        {
            var auditProducts = await _auditProducts.GetAll().CountAsync(x => x.ProductId == productId);
            if (auditProducts > 0) throw new AppException($"The product has been used in audit setup");
        }

        public async Task<bool> IsCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _Product.IsExistAsync(s => s.Code == code)
                : await _Product.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }
        public async Task<ProductModel> GetProductAsync(int id)
        {
            var result = await _Product.FindAsync(s => s.Id == id);
            return result.ToMap<Product, ProductModel>();
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            var result = await _Product.GetAllAsync();
            return result.ToMap<Product, ProductModel>();
        }

        public async Task<IPagedList<ProductModel>> GetPagedProductsAsync(int pageNumber, int pageSize)
        {
            var result = await _Product.GetAll().OrderByDescending(s => s.CreatedTime).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<Product, ProductModel>();

        }

        public async Task<IEnumerable<ProductModel>> GetQueryProductsAsync()
        {
            var result = await _Product.ExecuteQueryAsyc<ProductModel>("SELECT * FROM Products");
            return result;
        }

        public async Task<ProductModel> SaveAsync(ProductModel model)
        {
            var imagePath = await GetFilePath(model);
            var example = model.ToMap<ProductModel, Product>();
            example.ImageUrl = imagePath;

            var result = await _Product.CreateOrUpdateAsync(example);
            return result.ToMap<Product, ProductModel>();
        }

        public async Task<ProductModel> UpdateAsync(ProductModel model)
        {
            var product = await _Product.FindAsync(x => x.Id == model.Id);
            if (product == null) throw new AppException("Product not found");

            product.Code = model.Code;
            product.BrandId = model.BrandId;
            product.SubBrandId = model.SubBrandId;
            product.IsJTIProduct = model.IsJTIProduct;
            product.Name = model.Name;
            product.Type = model.Type;
            if (product.ImageUrl != model.ImageUrl && model.ImageFile != null)
            {
                product.ImageUrl = await GetFilePath(model);
            }
            var result = await _Product.UpdateAsync(product);
            return result.ToMap<Product, ProductModel>();
        }


        #region Private methods 


        private async Task<string> GetFilePath(ProductModel model)
        {
            if (model.ImageFile != null)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                string mimeType = "application/octet-stream";//model.File.ContentType;


                byte[] fileData;
                await using (var target = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(target);
                    fileData = target.ToArray();
                }

                string folder = "Product";

                var filePath = await _blobStorageService.UploadFileToBlobAsync(fileName,
                    fileData, mimeType, folder);
                return filePath;
            }

            return "";

        }

        #endregion


    }
}
