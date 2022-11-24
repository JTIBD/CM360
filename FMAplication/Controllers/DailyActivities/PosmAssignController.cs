using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Core.Params;
using FMAplication.Filters;
using FMAplication.Models.PosmAssign;
using FMAplication.RequestModels;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.FileImports.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using Microsoft.AspNetCore.Http;

namespace FMAplication.Controllers.DailyActivities
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    
    public class PosmAssignController : BaseController
    {
        private readonly IPosmAssignService _posmAssignService;
        private readonly IFileImportService _importService;

        public PosmAssignController(IPosmAssignService posmAssignService, IFileImportService importService)
        {
            _posmAssignService = posmAssignService;
            _importService = importService;
        }
        [JwtAuthorize]
        [HttpPost("DownloadPosmAssignFile")]
        public async Task<FileContentResult> DownloadPosmAssignFile([FromBody] ExportPosmAssignViewModel model)
        {

            FileData fileData = await _posmAssignService.GetExcelFileForPosmAssign(model);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }
        [JwtAuthorize]
        [HttpPost("ExcelImportPosmAssign")]
        public async Task<IActionResult> ExcelImportPosmAssign([FromForm] IFormFile excelFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _importService.ExcelImportPosmAssignAsync(excelFile);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPosmAssigns")]
        public async Task<IActionResult> GetPosmAssigns([FromQuery] PomsAssignParams searchParams)
        {
            try
            {
                var data = await _posmAssignService.GetPosmAssigns(searchParams);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPosmAssignDetails")]
        public async Task<IActionResult> GetPosmAssignDetails([FromQuery]GetPosmAssignModel model)
        {
            try
            {
                var data = await _posmAssignService.GetPosmTasks(model.CmUserId, model.SalesPointId, model.Date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }



    }
}
