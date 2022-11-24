using FMAplication.Controllers.Common;
using FMAplication.Domain.Products;
using FMAplication.Extensions;
using FMAplication.Filters;
using FMAplication.Models.Common;
using FMAplication.Models.Products;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.POSMProducts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboard;
        public DashboardController(ILogger<DashboardController> logger,
            IDashboardService dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }


        /// <summary>
        /// Return a list of POSMProduct Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllDashboardData()
        {
            try
            {
                DashboardModel model = new DashboardModel();

                model = await _dashboard.GetAllDashboardDataAsync();

                return OkResult(model);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}