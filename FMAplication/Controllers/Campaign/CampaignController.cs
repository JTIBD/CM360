using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Controllers.Products;
using FMAplication.Domain.Products;
using FMAplication.Filters;
using FMAplication.Models.Campaign;
using FMAplication.Models.Products;
using FMAplication.Services.Campaign.Interfaces;
using FMAplication.Services.Products.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers.Campaign
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CampaignController : BaseController
    {
        private readonly ILogger<CampaignController> _logger;
        private readonly ICampaignService _campaign;

        public CampaignController(ILogger<CampaignController> logger, ICampaignService campaignService)
        {
            _logger = logger;
            _campaign = campaignService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCampaigns()
        {
            try
            {
                var result = await _campaign.GetPagedCampaignsAsync(1, 20000);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("select")]
        public async Task<IActionResult> GetAllForSelect()
        {
            try
            {
                var result = await _campaign.GetAllForSelectAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaign(int id)
        {
            try
            {
                var result = await _campaign.GetCampaignAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCampaign([FromBody] CampaignModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _campaign.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCampaign([FromBody] CampaignModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _campaign.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePOSMProduct(int id)
        {
        
            var result = await _campaign.DeleteAsync(id);
            return OkResult(result);
        
        }
    }
}
