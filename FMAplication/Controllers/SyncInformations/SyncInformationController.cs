using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.SyncInformations;
using FMAplication.Services.SyncInformations.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMAplication.Controllers.SyncInformations
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SyncInformationController: BaseController
    {
        private readonly ISyncInformationService _syncInformation;
        private readonly ILogger<SyncInformationController> _logger;
        public SyncInformationController(ILogger<SyncInformationController> logger, ISyncInformationService syncInformation)
        {
            _logger = logger;
            _syncInformation = syncInformation;
        }

        [HttpGet]
        public async Task<ActionResult<SyncInformationModel>> GetInformations()
        {
            try
            {
                List<SyncInformationModel> result = await _syncInformation.GetAll();
                return Ok(result[0]);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<SyncInformationModel>>> GetAllInformations()
        {
            try
            {
                List<SyncInformationModel> result = await _syncInformation.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SyncInformationModel>> CreateInformations(SyncInformationModel payload)
        {
            try
            {
                payload.LastSyncTime = payload.LastSyncTime.ToUniversalTime();
                SyncInformationModel result = await _syncInformation.Create(payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPut("")]
        public async Task<ActionResult<SyncInformationModel>> UpdateInformations(SyncInformationModel payload)
        {
            try
            {
                payload.LastSyncTime = payload.LastSyncTime.ToUniversalTime();
                SyncInformationModel result = await _syncInformation.Update(payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
