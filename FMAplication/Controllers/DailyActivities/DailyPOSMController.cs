using System;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Examples;
using FMAplication.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class DailyPOSMController : BaseController
    {
        private readonly ILogger<DailyPOSMController> _logger;
        private readonly IDailyPOSMService _dailyposm;

        public DailyPOSMController(ILogger<DailyPOSMController> logger, IDailyPOSMService dailyposm)
        {
            _logger = logger;
            _dailyposm = dailyposm;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetDailyPOSM()
        {

            try
            {
                var result = await _dailyposm.GetDailyPOSMAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdownValue()
        {

            try
            {
                var result = await _dailyposm.GetDropdownValueAsync();
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
        public async Task<IActionResult> GetDailyPOSM(int id)
        {
            try
            {

                var result = await _dailyposm.GetDailyPOSMAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
        /// <summary>
        /// create or update Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [ValidationFilter]
        public async Task<IActionResult> SaveDailyPOSM([FromBody]DailyPOSMModel model)
        {

            try
            {
                var isExist = await _dailyposm.IsCodeExistAsync(model.DailyCMActivityId, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.DailyCMActivityId), "Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _dailyposm.SaveAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ValidationFilter]
        public async Task<IActionResult> CreateDailyPOSM([FromBody]DailyPOSMModel model)
        {
            try
            {
                var isExist = await _dailyposm.IsCodeExistAsync( model.DailyCMActivityId, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.DailyCMActivityId), "Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _dailyposm.CreateAsync(model);
                    return OkResult(result);
                }


            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }
        /// <summary>
        /// update Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model">DailyPOSMModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ValidationFilter]
        public async Task<IActionResult> UpdateDailyPOSM([FromBody]DailyPOSMModel model)
        {

            try
            {
                var isExist = await _dailyposm.IsCodeExistAsync(model.DailyCMActivityId, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.DailyCMActivityId), " Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _dailyposm.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteDailyPOSM(int id)
        {
            try
            {
                var result = await _dailyposm.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}