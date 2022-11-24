using System;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Brand;
using FMAplication.Services.Brand.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers.SubBrand
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SubBrandController : BaseController
    {
        private readonly ILogger<SubBrandController> _logger;
        private readonly ISubBrandService _subBrand;

        public SubBrandController(ILogger<SubBrandController> logger, ISubBrandService subBrandService)
        {
            _logger = logger;
            _subBrand = subBrandService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetSubBrands()
        {
            try
            {
                var result = await _subBrand.GetPagedSubBrandsAsync(1, 20000);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("select/{brandId}")]
        public async Task<IActionResult> GetAllForSelect(int brandId)
        {
            try
            {
                var result = await _subBrand.GetAllForSelectAsync(brandId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubBrand(int id)
        {
            try
            {
                var result = await _subBrand.GetSubBrandAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubBrand([FromBody] SubBrandModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _subBrand.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSubBrand([FromBody] SubBrandModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _subBrand.UpdateAsync(model);
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
        
            var result = await _subBrand.DeleteAsync(id);
            return OkResult(result);
        
        }
    }
}
