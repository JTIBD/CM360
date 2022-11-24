using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Enumerations;
using FMAplication.Filters;
using FMAplication.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Services.AvCommunication.Interfaces;
using FMAplication.Models.AvCommunications;

namespace FMAplication.Controllers.AVCommunication
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AvSetupController : BaseController
    {
        private readonly IAvSetupService _avSetup;

        public AvSetupController(IAvSetupService avSetup)
        {
            _avSetup = avSetup;
        }
        [JwtAuthorize]
        [HttpPost]
        public async Task<ActionResult<List<AvSetupModel>>> CreateAvSetup(CustomObject<List<AvSetupModel>> payload)
        {            
            var avSetups = await _avSetup.Create(payload.Data);
            return Ok(avSetups);                    
        }

        [JwtAuthorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAvSetup(AvSetupModel payload)
        {
            if (! await _avSetup.IsAvSetUpActive(payload) ) throw new ApplicationException("AvSetup Already InActive");

            AvSetupModel avSetup = await _avSetup.UpdateAvSetup(payload);
            return Ok(avSetup);                        
        }

        [JwtAuthorize]
        [HttpGet("getAvSetups")]
        public async Task<ActionResult<Pagination<AvSetupModel>>> GetAvSetups([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string search, [FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime,[FromQuery] int salespointId)
        {            
            fromDateTime = fromDateTime.ToUniversalTime();
            toDateTime = toDateTime.ToUniversalTime();
            Pagination<AvSetupModel> data = await _avSetup.GetSetups(pageSize, pageIndex, fromDateTime, toDateTime, search,salespointId);
            return Ok(data);            
        }

        [JwtAuthorize]
        [HttpGet("getAvSetup/{id}")]
        public async Task<ActionResult<AvSetupModel>> GetAvSetup([FromRoute] int id)
        {            
            AvSetupModel data = await _avSetup.GetAvSetupById(id);
            return Ok(data);            
        }

        [JwtAuthorize]
        [HttpPost("getExistingAvSetups")]
        public async Task<ActionResult<List<AvSetupModel>>> GetExistingAvSetups(CustomObject<List<AvSetupModel>> payload)
        {
            
            List<AvSetupModel> data = await _avSetup.GetExistingAvSetups(payload.Data);
            return Ok(data);                        
        }
    }
}
