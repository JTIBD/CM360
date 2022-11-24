using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Brand;
using FMAplication.Models.Products;
using X.PagedList;

namespace FMAplication.Services.Brand.Interfaces
{
    public interface IBrandService
    {
        Task<BrandModel> GetBrandAsync(int id); 
        Task<IEnumerable<BrandModel>> GetBrandsAsync();
        Task<IPagedList<BrandModel>> GetPagedBrandsAsync(int pageNumber, int pageSize);
        Task<BrandModel> CreateAsync(BrandModel model);
        Task<BrandModel> UpdateAsync(BrandModel model);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<BrandModel>> GetAllForSelectAsync();
    }
}
