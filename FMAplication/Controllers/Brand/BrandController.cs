using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Controllers.Products;
using FMAplication.Domain.Products;
using FMAplication.Filters;
using FMAplication.Models.Brand;
using FMAplication.Models.Products;
using FMAplication.Services.Brand.Interfaces;
using FMAplication.Services.Products.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers.Brand
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class BrandController : BaseController
    {
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandService _brand;

        public BrandController(ILogger<BrandController> logger, IBrandService brandService)
        {
            _logger = logger;
            _brand = brandService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                var result = await _brand.GetPagedBrandsAsync(1, 20000);
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
                var result = await _brand.GetAllForSelectAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            try
            {
                var result = await _brand.GetBrandAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _brand.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _brand.UpdateAsync(model);
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
        
            var result = await _brand.DeleteAsync(id);
            return OkResult(result);
        
        }
    }
}
