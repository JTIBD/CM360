using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Brand;
using FMAplication.Models.Products;
using X.PagedList;

namespace FMAplication.Services.Brand.Interfaces
{
    public interface ISubBrandService
    {
        Task<SubBrandModel> GetSubBrandAsync(int id); 
        Task<IEnumerable<SubBrandModel>> GetSubBrandsAsync();
        Task<IPagedList<SubBrandModel>> GetPagedSubBrandsAsync(int pageNumber, int pageSize);
        Task<SubBrandModel> CreateAsync(SubBrandModel model);
        Task<SubBrandModel> UpdateAsync(SubBrandModel model);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<SubBrandModel>> GetAllForSelectAsync(int brandId);
    }
}
