using FMAplication.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Menus.Interfaces
{
    public interface IMenuActivityPermissionService
    {
        Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionAsync();
        Task<IPagedList<MenuActivityPermissionModel>> GetPagedMenuPermissionAsync(int pageNumber, int pageSize);
        Task<MenuActivityPermissionModel> GetMenuActivityPermissionAsync(int id);
        Task<MenuActivityPermissionModel> SaveAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> CreateAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> CreateAndUpdateParentAsync(MenuActivityPermissionModel model);
        Task<MenuActivityPermissionModel> UpdateAsync(MenuActivityPermissionModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsMenuActivityPermissionExistAsync(int roleId, int id);
        Task<IEnumerable<MenuActivityPermissionModel>> GetAllMenusActivityPermissionByRoleIdAsync(int id);
        Task<List<MenuActivityPermissionModel>> CreateOrUpdateAllAsync(List<MenuActivityPermissionVm> modelList);
        Task<List<ActivityPermissionModel>> GetActivityPermissions(int roleId);
    }
}
