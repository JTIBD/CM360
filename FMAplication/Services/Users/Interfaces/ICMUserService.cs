using FMAplication.Domain.Users;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FMAplication.Models.Users;
using X.PagedList;
using System.Data;
using FMAplication.Domain.Sales;
using FMAplication.Models;
using FMAplication.Models.Sales;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Sales;

namespace FMAplication.Services.Users.Interfaces
{
    public interface ICMUserService
    {
        Task<bool> LoginCMUser(LoginModel model);
        Task<SalesPoint> GetSalesPointByCmUser(int userId);
        Task<CMUserRegisterModel> GetCMUserByLogin(LoginModel model);
        Task<IEnumerable<CMUserRegisterModel>> GetAllUserAsync();
        Task<CMUserRegisterModel> GetUserAsync(int id);
        Task<CMUserRegisterModel> SaveAsync(CMUserRegisterModel model);

        Task<CMUserRegisterModel> CreateUserAsync(CMUserRegisterModel model);

        Task<CMUserRegisterModel> UpdateAsync(CMUserRegisterModel model);
        Task<int> DeleteAsync(int id);

        Task<bool> IsUserExistAsync(string email, int id);
        Task<bool> IsUserPhoneNumberExistAsync(string phoneNumber, int id);
        Task<bool> IsUserCodeExistAsync(string code, int id);
        //Task<IEnumerable<CMUserRegisterModel>> GetCMUserByFMIdAsync(int id);
        Task<(IEnumerable<CMUserRegisterModel> Data, List<UserDataErrorViewModel> Errors)> ExcelSaveToDatabaseAsync(
            DataTable datatable);
        // Task<IEnumerable<CMUserRegisterModel>> GetAllCMUsersByCurrentUserIdAsync();
        Task<List<OutletMBModel>> GetOutletByCMUser(int userId);

        Task<string> GetUserTypeCode(CMUserType userType);

        Task<CMUserRegisterModel> DeleteUser(int id);
        Task<List<OutletMBModel>> GetOutletsBySalesPoint(int loggedinUserUserId, int salesPointId);
        Task<List<SalesPointMBModel>> GetSalesPointsByHhtUser(int userId);
    }
}

