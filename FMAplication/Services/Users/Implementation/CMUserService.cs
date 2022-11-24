
using FMAplication.Extensions;
using FMAplication.Repositories;
using FMAplication.Services.Users.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Domain.Users;
using FMAplication.Models.Users;
using X.PagedList;
using System.Data;
using FMAplication.Enumerations;
using AutoMapper;
using FMAplication.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using FMAplication.Models.Sales;
using System;
using System.Threading;
using FMAplication.Core;
using FMAplication.Domain.Sales;
using FMAplication.Exceptions;
using FMAplication.MobileModels.Sales;
using FMAplication.Services.Common.Interfaces;

namespace FMAplication.Services.Users.Implementation
{
    public class CMUserService : ICMUserService
    {
        private readonly IRepository<CMUser> _user;
        private readonly IUserInfoService _userInfoService;
        private readonly IRepository<UserInfo> _fmUser;
        private readonly IRepository<UserTerritoryMapping> _userTerritoryMapping;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNodeMapping;
        private readonly IRepository<Outlet> _outlet;
        private readonly IRepository<Route> _route;
        private readonly IRepository<Node> _node;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<Channel> _channel;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsSalesPoint;
        private readonly ICommonService _commonService;


        public CMUserService(IRepository<CMUser> user, IRepository<UserInfo> fmUser, IUserInfoService userInfoService,
            IRepository<UserTerritoryMapping> userTerritoryMapping, IRepository<Outlet> outlet, IRepository<Route> route, IRepository<Node> node,
            IRepository<SalesPoint> salesPoint, IRepository<SalesPointNodeMapping> salesPointNodeMapping, IRepository<Channel> channel,
            IRepository<CmsUserSalesPointMapping> cmsSalesPoint,
            ICommonService commonService)
        {
            _user = user;
            _userInfoService = userInfoService;
            _fmUser = fmUser;
            _userTerritoryMapping = userTerritoryMapping;
            _salesPointNodeMapping = salesPointNodeMapping;
            _outlet = outlet;
            _route = route;
            _node = node;
            _salesPoint = salesPoint;
            _channel = channel;
            _cmsSalesPoint = cmsSalesPoint;
            _commonService = commonService;
        }

        public async Task<CMUserRegisterModel> CreateUserAsync(CMUserRegisterModel model)
        {
           
            var user = model.ToMap<CMUserRegisterModel, CMUser>();

            if (model.SalesPointIds.Count > 0)
            {
                if (user.UserType == CMUserType.CMR) model.SalesPointIds = model.SalesPointIds.Take(1).ToList();
                foreach (var modelSalesPointId in model.SalesPointIds)
                    user.CmsUserSalesPointMappings.Add(new CmsUserSalesPointMapping { SalesPointId = modelSalesPointId });
            }

            var result = await _user.CreateAsync(user);
            return result.ToMap<CMUser, CMUserRegisterModel>();
        }

        public async Task<bool> LoginCMUser(LoginModel model)
        {
            var isLoggedIn = await _user.AnyAsync(a => a.PhoneNumber == model.MobileNumber && a.Password == model.Password && a.Status == Status.Active);
            return isLoggedIn;
        }

        public async Task<CMUserRegisterModel> GetCMUserByLogin(LoginModel model)
        {
            var result = await _user.FindAsync(a => a.PhoneNumber == model.MobileNumber && a.Password == model.Password && a.Status == Status.Active);
            return result.ToMap<CMUser, CMUserRegisterModel>();
        }

        public async Task<IEnumerable<CMUserRegisterModel>> GetAllUserAsync()
        {
           var result = await _user.GetAll().ToListAsync(); 
            return result.ToMap<CMUser, CMUserRegisterModel>();
        }

        //public async Task<IEnumerable<CMUserRegisterModel>> GetAllCMUsersByCurrentUserIdAsync()
        //{


        //    var appUserId = AppIdentity.AppUser.UserId;

        //    var result = new List<CMUser>();
            
            
        //    var userIds = _user.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
        //    result = _user.FindAll(x => userIds.Contains(x.FMUserId ?? 0)).ToList();
            

        //    return result.ToMap<CMUser, CMUserRegisterModel>();
            
        //}

        public async Task<CMUserRegisterModel> GetUserAsync(int id)
        {
            var result = await _user.FindAsync(x => x.Id == id);
            result.CmsUserSalesPointMappings = _cmsSalesPoint.FindAll(x => x.CmUserId == result.Id).ToList();
            var data = result.ToMap<CMUser, CMUserRegisterModel>();
            data.SalesPointIds = result.CmsUserSalesPointMappings.Select(x => x.SalesPointId).ToList();
            return data;
        }

        public async Task<CMUserRegisterModel> UpdateAsync(CMUserRegisterModel model)
        {
            var user = model.ToMap<CMUserRegisterModel, CMUser>();

            await RemoveExistingSalesPointMapping(user);

            await AddSalesPointMappingToCMuser(model, user);

            var result = await _user.UpdateAsync(user);
            return result.ToMap<CMUser, CMUserRegisterModel>();
        }

        private async System.Threading.Tasks.Task AddSalesPointMappingToCMuser(CMUserRegisterModel model, CMUser user)
        {
            
            if (model.SalesPointIds.Count > 0)
            {
                if (user.UserType == CMUserType.CMR) model.SalesPointIds = model.SalesPointIds.Take(1).ToList();
                foreach (var modelSalesPointId in model.SalesPointIds)
                    await _cmsSalesPoint.CreateAsync(new CmsUserSalesPointMapping
                        { CmUserId = user.Id, SalesPointId = modelSalesPointId });
            }
               
        }

        private async System.Threading.Tasks.Task RemoveExistingSalesPointMapping(CMUser user)
        {
            var existingMapping = _cmsSalesPoint.FindAll(x => x.CmUserId == user.Id).ToList();
            await _cmsSalesPoint.DeleteListAsync(existingMapping);
        }

        public async Task<CMUserRegisterModel> SaveAsync(CMUserRegisterModel model)
        {
            var example = model.ToMap<CMUserRegisterModel, CMUser>();
            var result = await _user.CreateOrUpdateAsync(example);
            return result.ToMap<CMUser, CMUserRegisterModel>();

        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _user.DeleteAsync(s => s.Id == id);
            return result;
        }

        public async Task<CMUserRegisterModel> DeleteUser(int id)
        {
            var user = await _user.FindAsync(x => x.Id == id);
            if (user == null) throw new AppException("User not found");

            user.Status = Status.InActive;
            var result = await _user.UpdateAsync(user);
            return result.ToMap<CMUser, CMUserRegisterModel>();
        }

        public async Task<List<OutletMBModel>> GetOutletsBySalesPoint(int loggedinUserUserId, int salesPointId)
        {
            var cmUser = _user.Find(x => x.Id == loggedinUserUserId);
            if (cmUser is null) throw new Exception("User not found");
            var salesPoint = await _cmsSalesPoint.FirstOrDefaultAsync(x => x.CmUserId == loggedinUserUserId && x.SalesPointId == salesPointId);
            if (salesPoint is null) throw new Exception("Salespoint not connect with current user.");
            var outlets = GetOutletsBySalesPoints(new List<int>{salesPointId});
            SetOutletTypes(outlets);
            return await Task.FromResult(outlets);
        }

        public async Task<List<SalesPointMBModel>> GetSalesPointsByHhtUser(int userId)
        {
            var salesPointMappings = await _cmsSalesPoint.FindAll(x => x.CmUserId == userId).ToListAsync();
            if(salesPointMappings == null || !salesPointMappings.Any())
                throw new Exception("No SalesPoint attached to this user.");
            var salesPointIds = salesPointMappings.Select(x =>  x.SalesPointId).ToList();
            var salesPoints = await _salesPoint.GetAllActive().Where(x => salesPointIds.Contains(x.SalesPointId)).ToListAsync();
            List<SalesPointMBModel> salesPointModels = salesPoints.ToMap<SalesPoint, SalesPointMBModel>();
            return salesPointModels;
        }


        public async Task<bool> IsUserExistAsync(string email, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.Email == email)
                : await _user.IsExistAsync(s => s.Email == email && s.Id != id);

            return result;
        }

        public async Task<bool> IsUserPhoneNumberExistAsync(string phoneNumber, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.PhoneNumber == phoneNumber)
                : await _user.IsExistAsync(s => s.PhoneNumber == phoneNumber && s.Id != id);

            return result;
        }

        public async Task<bool> IsUserCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _user.IsExistAsync(s => s.Code == code)
                : await _user.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }


        //TODO: Need to fix later
        //public async Task<IEnumerable<CMUserRegisterModel>> GetCMUserByFMIdAsync(int id)
        //{
        //    var result = await _user.FindAllAsync(cm => cm.FMUserId == id);
        //    return result.ToMap<CMUser, CMUserRegisterModel>();

        //}

        public async Task<(IEnumerable<CMUserRegisterModel> Data, List<UserDataErrorViewModel> Errors)> ExcelSaveToDatabaseAsync(DataTable datatable)
        {
            var items = new List<CMUser>();
            var errorsData = new List<UserDataErrorViewModel>();
            //var existCount = 0;
            //var invalidCount = 0;
            int index = 1;
            foreach (DataRow row in datatable.Rows)
            {
                index++;
                var item = new CMUser();
                var userType = row["USERTYPE"].ObjectToString("NULL");
                item.Name = row["NAME"].ObjectToString("NULL");
                item.Email = row["EMAIL"].ObjectToString("NULL");
                item.NIdBirthCertificate = row["NidBirthCertificate"].ObjectToString("NULL");
                item.JoiningDate = DateTime.Parse(row["JoiningDate"].ObjectToString("NULL"));
                item.Designation = row["DESIGNATION"].ObjectToString("NULL");

                item.AltCode = row["AlternativeCode"].ObjectToString("NULL");
                item.Password = row["PASSWORD"].ObjectToString("NULL");
                item.PhoneNumber = row["MOBILENUMBER"].ObjectToString("NULL");
                item.FamilyContactNo = row["FAMILYCONTACTNO"].ObjectToString("NULL");
                item.Address = row["ADDRESS"].ObjectToString("NULL");
                item.Status = Status.Active;

                var salesPoints = row["SalesPointCode"].ObjectToString("NULL");


                var errors =await ValidateUsersData(item, index, salesPoints, userType);
                var errorLength = errors.Count;
                if (errorLength > 0)
                {
                    errorsData.AddRange(errors);
                    continue;
                }
                   


                item.UserType = (CMUserType)Enum.Parse(typeof(CMUserType), userType, true);
                item.Code = await GetUserTypeCode(item.UserType);
                await AddSalesPointToUser(salesPoints, item);
                items.Add(item);
            }

            if (errorsData.Count > 0) return (null, errorsData);

            var result = await _user.CreateListAsync(items);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CMUser, CMUserRegisterModel>()
                    .ForMember(src => src.Password, opt => opt.Ignore());
            }).CreateMapper();

            var data = mapper.Map<List<CMUserRegisterModel>>(result);
            return (data, null);
        }

        private async Task<List<UserDataErrorViewModel>> ValidateUsersData(CMUser user, int row, string salesPointCodes, string userType)
        {
            var errors = new List<UserDataErrorViewModel>();


            if(string.IsNullOrWhiteSpace(user.Name))
                errors.Add(new UserDataErrorViewModel { ColumnName = "Name", ErrorMessage = "Name can't be empty", Row = row });

            if (string.IsNullOrWhiteSpace(user.Password))
                errors.Add(new UserDataErrorViewModel { ColumnName = "Password", ErrorMessage = "Password can't be empty", Row = row });
           
            if (String.IsNullOrWhiteSpace(user.Address))
                errors.Add(new UserDataErrorViewModel { ColumnName = "Address", ErrorMessage = "Address can't be empty", Row = row });

            if (string.IsNullOrWhiteSpace(user.Designation))
                errors.Add(new UserDataErrorViewModel { ColumnName = "Designation", ErrorMessage = "Designation can't be empty", Row = row });


            await GetPhoneNumberValidation(user, row, errors);
            GetUserTypeValidation(row, userType, errors);

           
            await GetSalesPointValidation(row, salesPointCodes, errors, userType);

            return errors;
        }

        private async Task GetSalesPointValidation(int row, string salesPointCodes, List<UserDataErrorViewModel> errors,string userType)
        {
            if (userType.ToLower() != CMUserType.TMS.ToString().ToLower() && userType.ToLower() != CMUserType.CMR.ToString().ToLower()) return;


            if (string.IsNullOrWhiteSpace(salesPointCodes))
            {
                errors.Add(new UserDataErrorViewModel
                {
                    ColumnName = "SalesPointCode", ErrorMessage = $"Invalid Sales Point code can't be empty ", Row = row
                });
                return;
            }

           

            var type = (CMUserType)Enum.Parse(typeof(CMUserType), userType, true);
            var codes = salesPointCodes.Split(",");

            if (type == CMUserType.CMR && codes.Length > 1)
            {
                errors.Add(new UserDataErrorViewModel
                {
                    ColumnName = "SalesPointCode",
                    ErrorMessage = $"CMR user can't have more than one sales point",
                    Row = row
                });
                return;
            }

            foreach (var code in codes)
            {
                var isExist = await _salesPoint.IsExistAsync(x => x.Code.ToLower() == code.Trim().ToLower());
                if (!isExist)
                    errors.Add(new UserDataErrorViewModel
                        {ColumnName = "SalesPointCode", ErrorMessage = $"Invalid Sales Point code - {code} ", Row = row});
                return;
            }
        }

        private void GetUserTypeValidation(int row, string userType, List<UserDataErrorViewModel> errors)
        {
            if (userType.ToLower() != CMUserType.TMS.ToString().ToLower() && userType.ToLower() != CMUserType.CMR.ToString().ToLower())
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = "User Type", ErrorMessage = "User Type must be CMR or TMS", Row = row});
        }

        private async Task GetPhoneNumberValidation(CMUser user, int row, List<UserDataErrorViewModel> errors)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = "PhoneNumber", ErrorMessage = "Phone Number can't be empty ", Row = row});
            else if (user.PhoneNumber.Length != 11)
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = "PhoneNumber", ErrorMessage = "Phone Number must be 11 characters ", Row = row});
            else if (await _user.IsExistAsync(x => x.PhoneNumber == user.PhoneNumber))
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = "PhoneNumber", ErrorMessage = "Phone Number already exists", Row = row});
           
        }

        private async Task AddSalesPointToUser(string salesPoints, CMUser item)
        {
            var salesPointCodes = salesPoints.Split(",");
            foreach (var salesPointCode in salesPointCodes)
            {
                var salesPoint =  await _salesPoint.FirstOrDefaultAsync(x => x.Code.ToLower() == salesPointCode.Trim().ToLower());
                if (salesPoint == null) throw new Exception("Sales Point not found");
                item.CmsUserSalesPointMappings.Add(new CmsUserSalesPointMapping {SalesPointId = salesPoint.SalesPointId});
            }
        }

        public async Task<SalesPoint> GetSalesPointByCmUser(int userId)
        {
            var cmUser = await _user.FindAsync(x => x.Id == userId);
            if (cmUser is null) throw new Exception("User not found");
            var salesPointMapping =  await _cmsSalesPoint.FirstOrDefaultAsync(x => x.CmUserId == userId);
            var salespoint = await _salesPoint.FindAsync(x => x.SalesPointId == salesPointMapping.SalesPointId);
            return salespoint;
        }


        public async Task<List<OutletMBModel>> GetOutletByCMUser(int userId)
        {
            var cmUser = _user.Find(x => x.Id == userId);
            if (cmUser is null) throw new Exception("User not found");

            var salesPoints = _cmsSalesPoint.FindAll(x => x.CmUserId == userId).ToList();
            var outlets = GetOutletsBySalesPoints(salesPoints.Select(x => x.SalesPointId).ToList());
            SetOutletTypes(outlets);
            return await Task.FromResult(outlets);
        }

        private static void SetOutletTypes(List<OutletMBModel> outlets)
        {
            foreach (var outlet in outlets)
            {
                if (outlet.OutletType is null)
                {
                    outlet.OutletType = OutletType.CASH;
                }
            }
        }

        public async Task<string> GetUserTypeCode(CMUserType userType)
        {
            var preFix = userType == CMUserType.CMR ? "CMR" : "TMS";

            int id = 0; 
            var data = await _user.GetAll().Where(x => x.UserType == userType).ToListAsync();
            id = data.Count + 1;
            return  $"{preFix}{id:D4}";
        }

        private List<int> GetNodeIdsByFmUser(int userId)
        {
            var nodeIds = _userTerritoryMapping.FindAll(x => x.UserInfoId == userId).Select(x => x.NodeId).ToList();
            var desc = getDescendantNodeIds(nodeIds);
            nodeIds.AddRange(desc);
            nodeIds = nodeIds.Distinct().ToList();
            return nodeIds;
        }

        private List<int> getDescendantNodeIds(List<int> nodeIds)
        {
            List<int> allDescendantsNodeIds = new List<int>();
            List<int> childNodeIds = _node.FindAll(n => n.ParentId != null && nodeIds.Any(id => id == n.ParentId.Value)).Select(x => x.NodeId).ToList();
            if (childNodeIds.Count > 0) 
            {
                allDescendantsNodeIds.AddRange(childNodeIds);
                var descendants = getDescendantNodeIds(childNodeIds);
                allDescendantsNodeIds.AddRange(descendants);
            }
            return allDescendantsNodeIds;
        }

        private List<OutletMBModel> getOutletsByNodes(List<int> nodeIds)
        {            
            var salesPoints = _salesPointNodeMapping.FindAll(x => nodeIds.Any(id=> id == x.NodeId)).ToList();
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var outlets = GetOutletsBySalesPoints(salesPointIds);
            return outlets;            
        }

        private List<OutletMBModel> GetOutletsBySalesPoints(List<int> salesPointIds)
        {
            List<SalesPoint> salesPoints = _salesPoint.FindAll(x => salesPointIds.Any(sId => sId == x.SalesPointId)).ToList();
            List<SalesPointMBModel> salesPointModels = salesPoints.ToMap<SalesPoint, SalesPointMBModel>();
            var outlets = _outlet.FindAll(x => salesPointIds.Any(o => o == x.SalesPointId)).ToList();
            var outletModels = outlets.ToMap<Outlet, OutletMBModel>();
            var channelIds = outlets.Select(x => x.ChannelID).ToList();
            var routes = _route.FindAll(x => salesPointIds.Any(s => s == x.SalesPointId)).ToList();
            var routeModels = routes.ToMap<Route, RouteMBModel>();

            var channels = _channel.FindAll(x => channelIds.Any(cn => cn == x.ChannelID)).ToList();
            var channelModels = channels.ToMap<Channel, ChannelMBModel>();

            foreach(var olet in outletModels)
            {
                olet.SalesPoint = salesPointModels.Find(x => x.SalesPointId == olet.SalesPointId);
                olet.Route = routeModels.Find(r => r.RouteId == olet.RouteId);
                olet.Channel = channelModels.Find(x => olet.ChannelID == x.ChannelID);
            }
            return outletModels;
        }
    }
}
