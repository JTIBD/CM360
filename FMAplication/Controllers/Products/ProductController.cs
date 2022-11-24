using FMAplication.Core;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Services.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Filters;
using FMAplication.Controllers.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMAplication.Controllers.Products
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _Product;

        public ProductController(ILogger<ProductController> logger, IProductService Product)
        {
            _logger = logger;
            _Product = Product;
        }

        /// <summary>
        /// Return a list of Product Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _Product.GetPagedProductsAsync(1, 200000);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single product object by productId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var result = await _Product.GetProductAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// create or update Product object and Return a single of Product Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveProduct([FromForm]ProductModel model)
        {
            try
            {
                var isExist = await _Product.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "This Product Code is already exist, please try another Product Code.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Product.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }    

        /// <summary>
        /// create Product object and Return a single of Product Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody]ProductModel model)
        {
            try
            {
                var isExist = await _Product.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "This Product Code is already exist, please try another Product Code.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Product.CreateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        /// <summary>
        /// update Product object and Return a single of Product Model objects
        /// </summary>
        /// <param name="model">ProductModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm]ProductModel model)
        {
            try
            {
                var isExist = await _Product.IsCodeExistAsync(model.Code, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Code), "This Product Code is already exist, please try another Product Code.");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _Product.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// delete a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
        
            var result = await _Product.DeleteAsync(id);
            return OkResult(result);
        
        }
    }
}