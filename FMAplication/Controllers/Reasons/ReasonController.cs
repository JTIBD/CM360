using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Enumerations;
using FMAplication.Filters;
using FMAplication.Models.Reasons;
using FMAplication.Services.Reasons.Interfaces;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers.Reasons
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ReasonController : BaseController
    {
        private readonly ILogger<ReasonController> _logger;
        private readonly IReasonService _reasonService;

        public ReasonController(ILogger<ReasonController> logger, IReasonService service)
        {
            _logger = logger;
            _reasonService = service;
        }


        [HttpGet("GetAllReasons")]
        public async Task<IActionResult> GetAllReasons()
        {
            try
            {
                List<ReasonModel> response = await _reasonService.GetAllReasonsAsync();
                return OkResult(response);
            }
            catch(Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReason(int id)
        {
            try
            {
                var result = await _reasonService.GetReasonAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetAllReasonTypes")]
        public async Task<ActionResult<List<ReasonTypeModel>>> GetAllReasonTypes()
        {
            var response = await _reasonService.GetAllReasonTypes();
            return Ok(response);
        }


        [HttpPost("CreateReason")]
        public async Task<IActionResult> CreateReason([FromBody] ReasonModel reason)
        {
            try
            {
                var isExist = await _reasonService.IsCodeExistAsync(reason.Name, reason.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(reason.Name), "This Reason Name is already exist, please try another Name.");
                }
                if (reason.Status != Status.Active && reason.Status != Status.InActive)
                {
                    ModelState.AddModelError(nameof(reason.Status), "Reason can't have any other status except active and inactive");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _reasonService.CreateAsync(reason);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateReason")]
        public async Task<IActionResult> UpdateReason([FromBody] ReasonModel reason)
        {
            try
            {
                var isExist = await _reasonService.IsCodeExistAsync(reason.Name, reason.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(reason.Name), "This Reason Name is already exist, please try another Name.");
                }
                if (reason.Status != Status.Active && reason.Status != Status.InActive)
                {
                    ModelState.AddModelError(nameof(reason.Status), "Reason can't have any other status except active and inactive");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _reasonService.UpdateAsync(reason);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("DeleteReason/{id}")]
        public async Task<IActionResult> DeleteReason(int id)
        {
            var result = await _reasonService.DeleteAsync(id);
            return OkResult(result);
        }
    }
}
