using FMAplication.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Controllers.Common
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppController : BaseController
    {
        [HttpGet("Enums")]
        public async Task<ActionResult<object>> GetEnums()
        {            
            EnumList model = new EnumList();
            var result = await Task.FromResult(model);
            return Ok(result);         
        }
    }
}
