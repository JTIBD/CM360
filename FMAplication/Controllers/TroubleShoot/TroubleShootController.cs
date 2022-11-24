using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Survey;
using FMAplication.Services.TroubleShoot.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FMAplication.Controllers.TroubleShoot
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TroubleShootController : BaseController
    {
        private readonly ITroubleShootService _troubleShoot;

        public TroubleShootController(ITroubleShootService troubleShoot)
        {
            _troubleShoot = troubleShoot;
        }

        [JwtAuthorize]
        [HttpGet("fixFileUrls")]
        public async Task<ActionResult<bool>> FixFileUrls()
        {
            await _troubleShoot.FixFileUrls();
            return Ok(true);
        }
    }
}