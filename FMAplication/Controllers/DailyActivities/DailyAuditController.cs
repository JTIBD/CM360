using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Enumerations;
using FMAplication.Filters;
using FMAplication.Models.Audits;
using FMAplication.Models.Common;
using FMAplication.Models.DailyAudit;
using FMAplication.Services.DailyAudits.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class DailyAuditController : BaseController
    {

        private readonly ILogger<DailyAuditController> _logger;
        private readonly IDailyAuditService _dailyaudit;

        public DailyAuditController(ILogger<DailyAuditController> logger, IDailyAuditService dailyaudit)
        {
            _logger = logger;
            _dailyaudit = dailyaudit;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetDailyAudit()
        {

            try
            {
                var result = await _dailyaudit.GetDailyAuditAsync();
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
                var result = await _dailyaudit.GetDropdownValueAsync();
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
        public async Task<IActionResult> GetDailyAudit(int id)
        {
            try
            {

                var result = await _dailyaudit.GetDailyAuditAsync(id);
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
        public async Task<IActionResult> SaveDailyAudit([FromBody]DailyAuditModel model)
        {

            try
            {
                var isExist = await _dailyaudit.IsCodeExistAsync(model.DailyCMActivityId, model.Id);
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
                    var result = await _dailyaudit.SaveAsync(model);
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
        public async Task<IActionResult> CreateDailyAudit([FromBody]DailyAuditModel model)
        {
            try
            {
                var isExist = await _dailyaudit.IsCodeExistAsync(model.DailyCMActivityId, model.Id);
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
                    var result = await _dailyaudit.CreateAsync(model);
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
        /// <param name="model">DailyAuditModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ValidationFilter]
        public async Task<IActionResult> UpdateDailyAudit([FromBody]DailyAuditModel model)
        {

            try
            {
                var isExist = await _dailyaudit.IsCodeExistAsync(model.DailyCMActivityId, model.Id);
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
                    var result = await _dailyaudit.UpdateAsync(model);
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
        public async Task<IActionResult> DeleteDailyAudit(int id)
        {
            try
            {
                var result = await _dailyaudit.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [JwtAuthorize]
        [HttpPost]
        public async Task<ActionResult<List<AuditSetupModel>>> CreateAuditSetup(CustomObject<List<AuditSetupModel>> payload)
        {
            var audits = await _dailyaudit.Create(payload.Data);
            return Ok(audits);
        }

        [JwtAuthorize]
        [HttpPut]
        public async Task<ActionResult<List<AuditSetupModel>>> UpdateAudit(AuditSetupModel payload)
        {
            if (! await _dailyaudit.IsAuditSetupActive(payload.Id, payload.Code) )
                throw new ApplicationException("Audit Setup Already InActive");

            AuditSetupModel survey = await _dailyaudit.UpdateAuditSetup(payload);
            return Ok(survey);
        }

        [JwtAuthorize]
        [HttpGet("getAuditSetups")]
        public async Task<ActionResult<Pagination<AuditSetupModel>>> GetSurveys([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string search, [FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime, int salesPointId)
        {
            fromDateTime = fromDateTime.ToUniversalTime();
            toDateTime = toDateTime.ToUniversalTime();
            Pagination<AuditSetupModel> data = await _dailyaudit.GetAuditSetups(pageSize, pageIndex, fromDateTime, toDateTime, search, salesPointId);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpGet("getAuditSetup/{id}")]
        public async Task<ActionResult<AuditSetupModel>> GetSurvey([FromRoute] int id)
        {
            AuditSetupModel data = await _dailyaudit.GetAuditSetupById(id);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpPost("getExistingAuditSeups")]
        public async Task<ActionResult<List<AuditSetupModel>>> GetExistingSurveys(CustomObject<List<AuditSetupModel>> payload)
        {
            List<AuditSetupModel> data = await _dailyaudit.GetExistingAuditSetups(payload.Data);
            return Ok(data);
        }
    }
}