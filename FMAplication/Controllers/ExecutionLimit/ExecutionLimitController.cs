using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.ExecutionLimits;
using FMAplication.Services.ExecutionLimit.Interfaces;
using FMAplication.Models.Common;

namespace FMAplication.Controllers.ExecutionLimit
{
    [JwtAuthorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ExecutionLimitController : BaseController
    {
        private readonly IMinimumExecutionLimitService _executionLimitService;

        public ExecutionLimitController(IMinimumExecutionLimitService minimumExecutionLimitService)
        {
            _executionLimitService = minimumExecutionLimitService;
        }

        [JwtAuthorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetMinimumExecutionLimitModel model)
        {
            var data = await _executionLimitService.GetAll(model);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpPost("GetExistingMinimumExecutionLimit")]
        public async Task<IActionResult> GetExistingMinimumExecutionLimit(CustomObject<List<MinimumExecutionLimitModel>> payload)
        {
            var data = await _executionLimitService.GetExistingMinimumExecutionLimit(payload.Data);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomObject<List<MinimumExecutionLimitModel>> model)
        {
            var data = await _executionLimitService.Create(model.Data);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await _executionLimitService.GetById(id);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MinimumExecutionLimitModel model)
        {
            var data = await _executionLimitService.Update(model);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var data = await _executionLimitService.Delete(id);
            return OkResult(data);
        }
    }
}
