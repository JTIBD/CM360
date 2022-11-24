using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Nodes;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.Sales.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FMAplication.Controllers.Sales
{

    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class NodeController : BaseController
    {
        private readonly ILogger<NodeController> _logger;
        private readonly INodeService _node;
        private readonly ICommonService _commonService;
        public NodeController(ILogger<NodeController> logger, INodeService node, ICommonService commonService)
        {
            _logger = logger;
            _node = node;
            _commonService = commonService;
        }

        /// <summary>
        /// Return a list of Node Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllNodes()
        {
            try
            {
                var result = await _node.GetAllNodesAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// return a single node object by nodeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNode(int id)
        {
            try
            {
                var result = await _node.GetNodeAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [JwtAuthorize]
        [HttpGet("GetNodeTreeByCurrentFmUser")]
        public async Task<ActionResult<List<NodeHierarchy>>> GetSalesPointWithHierarchyByCurrentFmUser()
        {
            try
            {
                var result = _commonService.GetNodeTreeByFmUser(AppIdentity.AppUser.UserId);
                var r = await Task.FromResult(result);
                return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}