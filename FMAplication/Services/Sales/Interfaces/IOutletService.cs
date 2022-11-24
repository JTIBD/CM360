using FMAplication.Domain.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Sales.Interfaces
{
    public interface IOutletService
    {
        // Task<IEnumerable<ExampleModel>> GetExamplesAsync();
        // Task<IPagedList<ExampleModel>> GetPagedExamplesAsync(int pageNumber, int pageSize);
        // Task<IEnumerable<ExampleModel>> GetQueryExamplesAsync();
        // Task<ExampleModel> GetExampleAsync(int id);
        // Task<ExampleModel> SaveAsync(ExampleModel model);
        // Task<ExampleModel> CreateAsync(ExampleModel model);
        // Task<ExampleModel> UpdateAsync(ExampleModel model);
        // Task<int> DeleteAsync(int id);
        Task<IEnumerable<Outlet>> GetOutletsByRouteId(int id);
        Task<IEnumerable<Route>> GetRoutesBySalesPointId(int id);
        Task<IEnumerable<Route>> GetRoutesByFMId(int id);
        Task<IEnumerable<SalesPoint>> GetSalesPointByFMId(int id);
        Task<IEnumerable<Outlet>> GetOutletsByChannelId(int id);
        Task<IEnumerable<Outlet>> GetOutletsByChannelIdandSalespointId(int id, int sid);
        Task<Outlet> GetOutletsByOutletId(int outletId);
    }
}
