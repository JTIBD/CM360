using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Exceptions;
using FMAplication.Filters;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Common;
using FMAplication.Services.AvCommunication.Interfaces;

namespace FMAplication.Controllers.AVCommunication
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CommunicationSetupController : BaseController
    {
        private readonly ICommunicationSetupService _communicationSetup;

        public CommunicationSetupController(ICommunicationSetupService communicationSetup)
        {
            _communicationSetup = communicationSetup;
        }

        [JwtAuthorize]
        [HttpPost("Save")]
        public async Task<ActionResult<List<CommunicationSetupModel>>> Save(CustomObject<List<CommunicationSetupModel>> payload)
        {
           
            var avSetups = await _communicationSetup.Save(payload.Data);
            return Ok(avSetups);
        }

        [JwtAuthorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSetup(CommunicationSetupModel payload)
        {
            if (! await _communicationSetup.IsCommunicationSetUpActive(payload))
                throw new AppException("Communication Setup Already InActive");

            CommunicationSetupModel avSetup = await _communicationSetup.Update(payload);
            return Ok(avSetup);
        
        }

        [JwtAuthorize]
        [HttpGet("Get")]
        public async Task<ActionResult<Pagination<CommunicationSetupModel>>> GetAll([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string search, [FromQuery] DateTime fromDateTime, [FromQuery] DateTime toDateTime, [FromQuery] int salesPointId)
        {
        
            fromDateTime = fromDateTime.ToUniversalTime();
            toDateTime = toDateTime.ToUniversalTime();
            Pagination<CommunicationSetupModel> data = await _communicationSetup.GetAll(pageSize, pageIndex, fromDateTime, toDateTime, search, salesPointId);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CommunicationSetupModel>> Get([FromRoute] int id)
        {
        
            CommunicationSetupModel data = await _communicationSetup.GetById(id);
            return Ok(data);
        
        }

        [JwtAuthorize]
        [HttpPost("getExistingCommunicationSetups")]
        public async Task<ActionResult<List<CommunicationSetupModel>>> GetExistingAvSetups(CustomObject<List<CommunicationSetupModel>> payload)
        {
        
            List<CommunicationSetupModel> data = await _communicationSetup.GetExistingCommunicationSetups(payload.Data);
            return Ok(data);
        
        }

    }
}
