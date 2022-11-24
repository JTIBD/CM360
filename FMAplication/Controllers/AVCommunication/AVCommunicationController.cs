using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Services.AvCommunication.Interfaces;
using FMAplication.Models.AvCommunications;

namespace FMAplication.Controllers.AVCommunication
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AvCommunicationController : BaseController
    {
        private readonly IAvCommunicationService _avCommunicationService;

        public AvCommunicationController(IAvCommunicationService avCommunicationService)
        {
            _avCommunicationService = avCommunicationService;
        }

        [HttpGet("Get")]
        
        public async Task<ActionResult<List<AvCommunicationViewModel>>> Get()
        {
            var data = await _avCommunicationService.GetAvsCommunications();
            return Ok(data);
        }

        [HttpGet("Get/{id}")]

        public async Task<ActionResult<List<AvCommunicationViewModel>>> GetById(int id)
        {
            
            var data = await _avCommunicationService.GetAvsCommunication(id);
            return Ok(data);

        }

        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromForm] AvCommunicationViewModel model)
        {
            
            await _avCommunicationService.SaveAvCommunication(model);
            return Ok();
        
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromForm] AvCommunicationViewModel model)
        {

            await _avCommunicationService.UpdateAvCommunication(model);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _avCommunicationService.RemoveAvCommunication(id);
            return Ok();
        }
    }
}
