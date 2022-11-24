using FMAplication.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.POSMProducts.Interfaces
{
    public interface IPOSMProductService
    {
        Task<IEnumerable<POSMProductModel>> GetPOSMProductsAsync();
        Task<IPagedList<POSMProductModel>> GetPagedPOSMProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<POSMProductModel>> GetQueryPOSMProductsAsync();
        Task<POSMProductModel> GetPOSMProductAsync(int id);
        Task<POSMProductModel> SaveAsync(POSMProductModel model);
        Task<POSMProductModel> CreateAsync(POSMProductModel model);
        Task<POSMProductModel> UpdateAsync(POSMProductModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(string code, int id);
        Task<IPagedList<POSMProductModel>> GetApprovedPOSMProductsAsync(int pageNumber, int pageSize);
        Task<List<POSMProductModel>> GetAllPOSMProductsAsync();
        Task<List<POSMProductModel>> GetAllJtiPosmProducts();
    }
}

