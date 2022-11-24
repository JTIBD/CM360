using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Exceptions;
using FMAplication.Filters;
using FMAplication.Models.Common;
using FMAplication.Models.Guidelines;
using FMAplication.RequestModels;
using FMAplication.Services.Guidelines.Interfaces;

namespace FMAplication.Controllers.Guideline
{
    [JwtAuthorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]

    public class GuidelineSetupController : BaseController
    {
        private readonly IGuidelineSetupService _guidelineSetupService;

        public GuidelineSetupController(IGuidelineSetupService guidelineSetupService)
        {
            _guidelineSetupService = guidelineSetupService;
        }

        [JwtAuthorize]
        [HttpPost("Create")]    
        public async Task<IActionResult> Create(CustomObject<List<GuidelineSetupModel>> model)
        {
            var data = await _guidelineSetupService.Create(model.Data);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(GuidelineSetupModel model)
        {
            if (! await _guidelineSetupService.IsGuidelineActive(model))
                throw new AppException("Guideline Setup Already InActive");

            var data = await _guidelineSetupService.Update(model);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpPost("GetExistingGuidelineSetups")]
        public async Task<IActionResult> GetExistingGuidelineSetups(CustomObject<List<GuidelineSetupModel>> payload)
        {

            var data = await _guidelineSetupService.GetExistingGuidelineSetups(payload.Data);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetGuidelineSetupsRequestModel model)
        {
            var data = await _guidelineSetupService.GetAll(model);
            return OkResult(data);
        }

        [JwtAuthorize]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await _guidelineSetupService.GetById(id);
            return OkResult(data);
        }
    }
}
