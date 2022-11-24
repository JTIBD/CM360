using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.MobileModels.Sales;
using FMAplication.Models;
using FMAplication.Models.Sales;
using FMAplication.Models.Users;
using FMAplication.Repositories;
using FMAplication.Services.Interfaces;
using FMAplication.Services.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FMAplication.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly IUserInfoService _userService;
        private readonly ICMUserService _cmUserService;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsSalesPoint;
        private readonly IRepository<SalesPoint> _salesPoint;

        public AuthService(IConfiguration config, IUserInfoService user, 
            ICMUserService cmUser, 
            IRepository<CmsUserSalesPointMapping> cmsSalesPoint, 
            IRepository<SalesPoint> salesPoint)
        {
            configuration = config;
            _userService = user;
            this._cmUserService = cmUser;
            _cmsSalesPoint = cmsSalesPoint;
            _salesPoint = salesPoint;
        }

        public async Task<object> GetJWTToken(LoginModel model)
        {
            try
            {
                var cmUser = await _cmUserService.GetCMUserByLogin(model);
                if(cmUser == null)
                    throw new Exception("Invalid HHT User");

                var user = new AppUserPrincipal("brainstation23")
                {
                    UserId = cmUser.Id,
                    Email = cmUser.Email,
                    ActiveRoleId = 0,
                    RoleIdList = new List<int> { 0 },
                    Avatar = "/img/user.png",
                    FullName = cmUser.Name,
                    EmployeeId = "0",
                    Phone = cmUser.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",

                };
                var appClaimes = user
                                .GetByName()
                                .Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                    {

                            new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                claims.AddRange(appClaimes);
                foreach (var role in user.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Tokens:Issuer"],
                    configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1.00),
                    signingCredentials: cred
                    );
                var salesPointMappings = _cmsSalesPoint.FindAll(x => x.CmUserId == cmUser.Id).ToList();
                var salesPointIds = salesPointMappings.Select(x =>  x.SalesPointId).ToList();
                var salesPoints = _salesPoint.GetAllActive().Where(x => salesPointIds.Contains(x.SalesPointId)).ToList();
                List<SalesPointMBModel> salesPointModels = salesPoints.ToMap<SalesPoint, SalesPointMBModel>();

                var results = new
                {
                    userId = cmUser.Id,
                    fullName = cmUser.Name??"",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userType = DesignationByUserType(cmUser.UserType),
                    designation = cmUser.Designation,
                    salesPoints = salesPoints.Select(t=>t.SalesPointId).ToList(),
                    salesPointModels = salesPointModels
                };

                return results;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private string DesignationByUserType(CMUserType userType)
        {
            if (userType == CMUserType.CMR)
                return CMUserType.CMR.ToString();
            return CMUserType.TMS.ToString();
        }


        public async Task<object> GetJWTToken(AdLoginModel model)
        {
            try
            {
                var userInfo = await _userService.GetCurrentUser(model.AdGuid);                
                

                var user = new AppUserPrincipal("brainstation23")
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    ActiveRoleId = userInfo.RoleId,
                    RoleIdList = userInfo.RoleIds,
                    Avatar = "/img/user.png",
                    FullName = userInfo.Name,
                    EmployeeId = userInfo.EmployeeId,
                    Phone = userInfo.PhoneNumber,
                    UserAgentInfo = "127.0.0.1",
                    NodeId = userInfo.NodeId,
                    ActiveRoleName = userInfo.RoleName

                };
                var appClaimes = user
                                .GetByName()
                                .Select(item => new Claim(item.Key, item.Value));

                var claims = new List<Claim>()
                    {

                            new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                claims.AddRange(appClaimes);
                foreach (var role in user.RoleIdList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

                }


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Tokens:Issuer"],
                    configuration["Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(15.00),
                    signingCredentials: cred

                    );


                var results = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };

                return results;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }



    }
}
