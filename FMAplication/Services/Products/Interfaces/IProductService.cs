using FMAplication.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Products.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProductsAsync();
        Task<IPagedList<ProductModel>> GetPagedProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductModel>> GetQueryProductsAsync();
        Task<ProductModel> GetProductAsync(int id);
        Task<ProductModel> SaveAsync(ProductModel model);
        Task<ProductModel> CreateAsync(ProductModel model);
        Task<ProductModel> UpdateAsync(ProductModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(string code, int id);
    }
}

