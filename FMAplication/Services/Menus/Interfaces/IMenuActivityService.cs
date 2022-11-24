using FMAplication.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Menus.Interfaces
{
    public interface IMenuActivityService
    {
        Task<IEnumerable<MenuActivityModel>> GetAllMenusActivityAsync();
        Task<IPagedList<MenuActivityModel>> GetPagedMenusAsync(int pageNumber, int pageSize);
        Task<MenuActivityModel> GetMenuActivityAsync(int id);
        Task<MenuActivityModel> SaveAsync(MenuActivityModel model);
        Task<MenuActivityModel> CreateAsync(MenuActivityModel model);
        Task<MenuActivityModel> CreateAndUpdateParentAsync(MenuActivityModel model);
        Task<MenuActivityModel> UpdateAsync(MenuActivityModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsMenuActivityExistAsync(string name, string code, int id);

        Task<IEnumerable<MenuActivityModel>> GetAllMenuActivityById(int id);

        Task<IEnumerable<MenuActivityPermissionVm>> GetAllMenuActivityPermissionByRoleId(int id); /////to get all menu id under any roleid
    }
}
