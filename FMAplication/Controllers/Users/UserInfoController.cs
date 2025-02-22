﻿using FMAplication.Core;
using FMAplication.Extensions;
using FMAplication.Models.Users;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Filters;
using FMAplication.Controllers.Common;
using FMAplication.Domain.Users;
using FMAplication.Models.Sales;

namespace FMAplication.Controllers.Users
{
    [ApiController]

    [JwtAuthorize]

    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class UserInfoController:BaseController
    {
        private readonly ILogger<UserInfoController> logger;
        private readonly IUserInfoService _User;
        public UserInfoController(IUserInfoService userService, ILogger<UserInfoController> logger)
        {
            this.logger = logger;
            this._User = userService;
        }

        /// <summary>
        /// Return a list of User Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                var result = await _User.GetUsersAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Return a paged list of User Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("paged-users")]
        public async Task<IActionResult> GetPagedUsers()
        {
            try
            {
                var result = await _User.GetPagedUsersAsync(1, 20);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _User.GetUserAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }      



        /// <summary>
        /// create or update User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost("save")]
        //public async Task<IActionResult> SaveUser([FromBody]UserInfoModel model)
        //{
        //    try
        //    {
        //        var isExist = await _User.IsUserExistAsync(model.Code, model.Id);
        //        if (isExist)
        //        {
        //            ModelState.AddModelError(nameof(model.Code), "User Already Exist");
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return ValidationResult(ModelState);
        //        }
        //        else
        //        {
        //            var result = await _User.SaveAsync(model);
        //            return OkResult(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}
        /// <summary>
        /// create User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserInfoModel model)
        {
            try
            {
                var isExist = await _User.IsUserExistAsync(model.AdGuid);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "User Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _User.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// Update User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model">UserInfoModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody]UserInfoModel model)
        {
            try
            {
                var isExist = await _User.IsUserExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "User Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {

                    var result = await _User.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// delete a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _User.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("rolelinkwithuser")]
        public async Task<IActionResult> RoleLinkWithUser([FromBody]UserRoleMappingModel model)
        {
            try
            {
                var isExist = await _User.IsRoleLinkWithUserExistAsync(model.RoleId, model.UserInfoId);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.RoleId), "This User has already in this role");
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _User.SaveRoleLinkWithUserAsync(model);
                    return OkResult(result);

                }
                
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
        
        
        /// <summary>
        /// create User object and Return a single of User Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("territorylinkwithuser")]
        public async Task<IActionResult> TerritoryLinkWithUser([FromBody]UserTerritoryMappingModel model)
        {
            try
            {
                var isExist = await _User.IsTerritoryLinkWithUserExistAsync(model.NodeId, model.UserInfoId);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.NodeId), "This User has already in this territory");
                }
                var result = await _User.SaveTerritoryLinkWithUserAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }


         /// <summary>
        /// return a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("designationcode/{id}")]
        public async Task<IActionResult> GetDesignationCode(int id)
        {
            try
            {
                var result = await _User.GetDesignationCodeAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Return a list of Hierarchy Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("hierarchy")]
        public async Task<IActionResult> GetAllHierarchy()
        {
            try
            {

                var result = await _User.GetAllHierarchyAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}
