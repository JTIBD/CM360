using FMAplication.Core;
using FMAplication.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Controllers.Common
{
    [ApiController]
    [Route("api/errors")]
    public class ErrorController:Controller
    {
       
        public ErrorController()
        {

        }
        [AllowAnonymous]
        [HttpGet("{code}")]
        public async Task<IActionResult> Error(int code)
        
        {
            var apiResult = new ApiResponse<ErrorModel>()
            { };

            if (code == 401)
            {
                apiResult.StatusCode = 401;
                apiResult.Status = "Fail";
                apiResult.Msg = "You are not authorized to see this page";
                return BadRequest(apiResult);

            }
            else {
                apiResult.StatusCode = 500;
                apiResult.Status = "Fail";
                apiResult.Msg = "Unauthenticated User or token expired";
                return BadRequest(apiResult);

            }

          




            //ex.ToWriteLog();
            //    apiResult.StatusCode = 500;
            //    apiResult.Status = "Fail";
            //    apiResult.Msg = ex.Message;
            //    return BadRequest(apiResult);



        }

    }
}
