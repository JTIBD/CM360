using FMAplication.Models.FileUtility;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Controllers.FileUtility
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class FileUtilityController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileUtilityController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("ParseExcel")]
        public ActionResult<ParsedExcelData> ParseExcel([FromForm] FileUpload payload)
        {
            try {
                ParsedExcelData parsedExcelData = _fileService.ParseExcel(payload.File);
                return Ok(parsedExcelData);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
