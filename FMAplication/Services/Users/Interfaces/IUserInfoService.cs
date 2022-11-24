using FMAplication.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using FMAplication.Models.Sales;

namespace FMAplication.Services.Users.Interfaces
{
    public interface IUserInfoService
    {
        //Task<IEnumerable<UserInfoViewModel>> GetAllUserAsync();
        //Task<UserInfoViewModel> GetUserAsync(int id);
        //Task<UserInfoViewModel> CreateUserAsync(UserInfoViewModel model);
        //Task<UserInfoViewModel> UpdateAsync(UserInfoViewModel model);
        //Task<int> DeleteAsync(int id);


        Task<IEnumerable<UserInfoModel>> GetUsersAsync();
        Task<IPagedList<UserInfoModel>> GetPagedUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UserInfoModel>> GetQueryUsersAsync();
        Task<UserInfoModel> GetUserAsync(int id);
        Task<UserInfoModel> SaveAsync(UserInfoModel model);
        Task<UserRoleMappingModel> SaveRoleLinkWithUserAsync(UserRoleMappingModel model);
        Task<UserTerritoryMappingModel> SaveTerritoryLinkWithUserAsync(UserTerritoryMappingModel model);
        Task<UserInfoModel> CreateAsync(UserInfoModel model);
        Task<UserRoleMappingModel> CreateRoleLinkWithUserAsync(UserRoleMappingModel model);
        Task<UserTerritoryMappingModel> CreateTerritoryLinkWithUserAsync(UserTerritoryMappingModel model);
        Task<UserInfoModel> UpdateAsync(UserInfoModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsUserExistAsync(string code, int id);
        Task<bool> IsRoleLinkWithUserExistAsync(int roleId, int userInfoId);
        Task<bool> IsRoleMappingExistAsync(int userInfoId);
        Task<bool> IsTerritoryLinkWithUserExistAsync(int nodeId, int userInfoId);
        Task<bool> IsUserExistAsync(string adguid);
        Task<bool> CheckAdUserInfo(AdLoginModel model);

        //Task<IEnumerable< UserInfoModel>> GetFMUsersAsync(int id);
        Task<Nullable<int>> GetFMUserIdByNameAsync(string name);
        Task<Nullable<int>> GetFMUserIdByEmailAsync(string email);
        Task<UserInfoModel> GetCurrentUser(string adguid);
        Task<NodeModel> GetDesignationCodeAsync(int id);
       Task<IEnumerable<UserTerritoryMappingModel>>  GetUserTerritoryMappingListByUserInfoIdAsync(int userinfoId);
        Task<int> DeleteUserTerritoryMappingAsync(int id);

        Task<UserRoleMappingModel> GetUserRoleMappingByUserInfoIdAsync(int userinfoId);
        Task<int> DeleteUserRoleMappingAsync(int id);
        Task<IEnumerable<HierarchyModel>> GetAllHierarchyAsync();
    }
}
