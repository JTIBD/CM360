using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Core;
using FMAplication.Domain.Users;
using FMAplication.Extensions;
using FMAplication.Models;
using FMAplication.Models.Users;
using FMAplication.Services.Interfaces;
using FMAplication.Services.Notification;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FMAplication.Controllers.Users
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class LoginController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IAuthService authService;
        private readonly IUserInfoService _userService;
        private readonly ICMUserService _cmuserservice;
        private readonly INotificationHubService _notificationHubService;

        public LoginController(IConfiguration config, IAuthService service, 
            IUserInfoService user, ICMUserService userService, INotificationHubService notificationHubService)
        {
            _config = config;
            authService = service;
            _userService = user;
            _cmuserservice = userService;
            _notificationHubService = notificationHubService;
        }


        [HttpGet("Hello")]
        public async Task<IActionResult> Hello()
        {
            await _notificationHubService.SendNotification("Hello From Controller");
            return Ok();
        } 




        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var apiResult = new ApiResponse<IEnumerable<LoginModel>>
            {
                Data = new List<LoginModel>()
            };
            if (ModelState.IsValid)
            {
                try
                {
                    bool IsLoginSuccessful = await  _cmuserservice.LoginCMUser(model);
                    if (IsLoginSuccessful)
                    {
                        var result = await authService.GetJWTToken(model);
                        return OkResult(result);

                    }
                    else
                    {
                        return BadRequest("Invalid username or password.");
                    }
                   
                }
                catch (Exception ex)
                {
                    ex.ToWriteLog();

                    apiResult.StatusCode = 500;
                    apiResult.Status = "Fail";
                    apiResult.Msg = ex.Message;
                    return BadRequest(apiResult);
                }


            }


            return BadRequest();


        }

        [HttpPost("aduserlogin")]
        public async Task<IActionResult> AdUserLogin([FromBody]AdLoginModel model)
        {
            var apiResult = new ApiResponse<IEnumerable<AdLoginModel>>
            {
                Data = new List<AdLoginModel>()
            };
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSameAdUser = await _userService.CheckAdUserInfo(model);         
                    bool IsAdUserAvailable = await _userService.IsUserExistAsync(model.AdGuid);
                    if (IsAdUserAvailable && isSameAdUser)
                    {
                        var result =  await authService.GetJWTToken(model);
                        return OkResult(result);
                    }
                    else
                    {
                        var err = new { err = "Cannot Verify User" };
                        return BadRequest(err);
                    }

                }
                catch (Exception ex)
                {
                    ex.ToWriteLog();

                    apiResult.StatusCode = 500;
                    apiResult.Status = "Fail";
                    apiResult.Msg = ex.Message;
                    return BadRequest(apiResult);
                }


            }


            return BadRequest();


        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getuser")]
        public IActionResult GetUser()
        {
            var apiResult = new ApiResponse<IEnumerable<UserInfo>>
            {
                Data = new List<UserInfo>()
            };
            try
            {
                UserInfo userInfo = new UserInfo();
                IEnumerable<object> items = User.Claims;
                //foreach (var item in items)
                //{
                //    userInfo.Name = AppIdentity.AppUser;
                //    //userInfo.Roles = item.
                //    return OkResult(userInfo);

                //}

                return OkResult(AppIdentity.AppUser);

            }
            catch (Exception ex)
            {

                ex.ToWriteLog();

                apiResult.StatusCode = 500;
                apiResult.Status = "Fail";
                apiResult.Msg = ex.Message;
                return BadRequest(apiResult);
            }



        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getaduser")]
        public IActionResult GetAdUser()
        {
            var apiResult = new ApiResponse<IEnumerable<UserInfo>>
            {
                Data = new List<UserInfo>()
            };
            try
            {
                UserInfoModel userInfo = new UserInfoModel() {
                    Id = AppIdentity.AppUser.UserId,
                    Name = AppIdentity.AppUser.FullName,
                    RoleId = AppIdentity.AppUser.ActiveRoleId,
                    RoleIds = AppIdentity.AppUser.RoleIdList,
                    NodeId = AppIdentity.AppUser.NodeId,
                    PhoneNumber = AppIdentity.AppUser.Phone,
                    EmployeeId = AppIdentity.AppUser.EmployeeId,
                    Email = AppIdentity.AppUser.Email,
                    RoleName = AppIdentity.AppUser.ActiveRoleName

                };              
               

                return OkResult(userInfo);

            }
            catch (Exception ex)
            {

                ex.ToWriteLog();

                apiResult.StatusCode = 500;
                apiResult.Status = "Fail";
                apiResult.Msg = ex.Message;
                return BadRequest(apiResult);
            }
        }
    }
}
