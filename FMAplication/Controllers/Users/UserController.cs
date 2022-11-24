using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using FMAplication.Models.Users;
using FMAplication.Services.Users.Interfaces;
using FMAplication.Filters;
using FMAplication.Controllers.Common;
using Microsoft.AspNetCore.Http;
using FMAplication.Services.FileImports.Interfaces;
using FMAplication.Models.Sales;
using System.Collections.Generic;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels.Sales;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace FMAplication.Controllers.Users
{
    [ApiController]
   
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;
        private readonly IFileImportService _FileImportService;
        private readonly ICMUserService _User;
        public UserController(ICMUserService userService, ILogger<UserController> logger,
            IFileImportService fileImportService)
        {
            this.logger = logger;
            _FileImportService = fileImportService;
            _User = userService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _User.GetAllUserAsync();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [JwtAuthorize]
        [HttpGet("Me")]
        public async Task<ActionResult<CMUserModel>> Me()
        {
            var loggedInUser = AppIdentity.AppUser;
            var user = await _User.GetUserAsync(loggedInUser.UserId);
            if (user is null) throw new AppException("CM user not found");
            return Ok(user.ToMap<CMUserRegisterModel, CMUserModel>());
        }

        [HttpGet("GetUserTypeCode")]
        public async Task<IActionResult> GetUserTypeCode([FromQuery] CMUserType userTypeId)
        {
            try
            {
                var result = await _User.GetUserTypeCode(userTypeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


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

        [JwtAuthorize]
        [HttpGet("outletsByCMUser")]
        public async Task<ActionResult<List<OutletMBModel>>> GetOutletsByLoggedCMInUser()
        {

            var loggedinUser = AppIdentity.AppUser;
            try
            {
                List<OutletMBModel> result = await _User.GetOutletByCMUser(loggedinUser.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [JwtAuthorize]
        [HttpGet("outletsBySalesPoint/{salesPointId}")]
        public async Task<ActionResult<List<OutletMBModel>>> GetOutletsBySalesPoint(int salesPointId)
        {

            var loggedinUser = AppIdentity.AppUser;
            try
            {
                List<OutletMBModel> result = await _User.GetOutletsBySalesPoint(loggedinUser.UserId, salesPointId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CMUserRegisterModel model)
        {
            var isExistP = await _User.IsUserPhoneNumberExistAsync(model.PhoneNumber, model.Id);
            if (isExistP) throw new AppException($"{model.PhoneNumber} Phone Number Already Exist");
            
            var isExistC = await _User.IsUserCodeExistAsync(model.Code, model.Id);
            if (isExistC) throw new AppException($"{model.Code} Code Already Exist");
           
            var result = await _User.CreateUserAsync(model);
            return OkResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody]CMUserRegisterModel model)
        {
            var isExistP = await _User.IsUserPhoneNumberExistAsync(model.PhoneNumber, model.Id);
            if (isExistP) throw new AppException($"{model.PhoneNumber} Phone Number Already Exist");

            var isExistC = await _User.IsUserCodeExistAsync(model.Code, model.Id);
            if (isExistC) throw new AppException($"{model.Code} Code Already Exist");
           
            var result = await _User.UpdateAsync(model);
            return OkResult(result);  
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
                var result = await _User.DeleteUser(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost("excelImport")]
        public async Task<IActionResult> ExcelImportUser([FromForm]IFormFile excelFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _FileImportService.ExcelImportCAUserAsync(excelFile);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}