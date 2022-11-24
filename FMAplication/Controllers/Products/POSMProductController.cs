using FMAplication.Core;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Services.POSMProducts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Filters;
using FMAplication.Controllers.Common;
using FMAplication.Services.FileUploads.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMAplication.Controllers.Products
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class POSMProductController : BaseController
    {
        private readonly ILogger<POSMProductController> _logger;
        private readonly IPOSMProductService _POSMProduct;

        public POSMProductController(ILogger<POSMProductController> logger, IPOSMProductService POSMProduct)
        {
            _logger = logger;
            _POSMProduct = POSMProduct;
        }

        /// <summary>
        /// Return a list of POSMProduct Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetPOSMProducts()
        {
            try
            {
                var result = await _POSMProduct.GetPagedPOSMProductsAsync(1, 20000);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPOSMProducts()
        {
            try
            {
                List<POSMProductModel> result = await _POSMProduct.GetAllPOSMProductsAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("allJTIProducts")]
        public async Task<ActionResult<POSMProductModel>> GetAllJTIProducts()
        {
            List<POSMProductModel> result = await _POSMProduct.GetAllJtiPosmProducts();
            return Ok(result);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedPOSMProducts()
        {
            try
            {
                var result = await _POSMProduct.GetApprovedPOSMProductsAsync(1, 20000);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPOSMProduct(int id)
        {
            try
            {
                var result = await _POSMProduct.GetPOSMProductAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create or update POSMProduct object and Return a single of POSMProduct Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SavePOSMProduct([FromForm]POSMProductModel model)
        {
            var isExist = await _POSMProduct.IsCodeExistAsync(model.Code, model.Id);
            if (isExist)
            {
                ModelState.AddModelError(nameof(model.Code), "This POSM Product Code is already exist, please try another POSM Product Code.");
            }
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }
            else
            {
                var result = await _POSMProduct.SaveAsync(model);
                return OkResult(result);
            }
        }
        /// <summary>
        /// create POSMProduct object and Return a single of POSMProduct Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePOSMProduct([FromBody]POSMProductModel model)
        {
            try
            {
                var isExist = await _POSMProduct.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "This POSM Product Code is already exist, please try another POSM Product Code.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _POSMProduct.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// update POSMProduct object and Return a single of POSMProduct Model objects
        /// </summary>
        /// <param name="model">POSMProductModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePOSMProduct([FromForm]POSMProductModel model)
        {
           
            var isExist = await _POSMProduct.IsCodeExistAsync(model.Code, model.Id);
            if (isExist)
            {
                ModelState.AddModelError(nameof(model.Code), "This POSM Product Code is already exist, please try another POSM Product Code.");
            }
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }
            else
            {
                var result = await _POSMProduct.UpdateAsync(model);
                return OkResult(result);
            }
            
        }

        /// <summary>
        /// delete a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePOSMProduct(int id)
        {
        
            var result = await _POSMProduct.DeleteAsync(id);
            return OkResult(result);
        
        }
    }
}