using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Common;
using FMAplication.Models.DailyTasks;
using FMAplication.RequestModels.Bases;
using FMAplication.RequestModels.Reports;
using FMAplication.Services.DailyTasks.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.Reports.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FMAplication.Controllers.Reports
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ReportController: BaseController
    {
        private IDailyTaskService _dailyTask;
        private IReportService _report;

        public ReportController(IDailyTaskService dailyTask, IReportService report)
        {
            _dailyTask = dailyTask;
            _report = report;
        }

        [JwtAuthorize]
        [HttpGet("GetAuditReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetAuditReport([FromQuery] GetAuditReportParams queryParams)
        {
            Pagination < DailyTaskModel >  report = await _report.GetAuditReport(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportAuditReportToExcel")]
        public async Task<FileContentResult> ExportAuditReportToExcel([FromQuery] GetAuditReportParams queryParams)
        {
            FileData fileData = await _report.ExportAuditReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetSurveyReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetSurveyReport([FromQuery] PaginationParams queryParams)
        {
            Pagination<DailyTaskModel> report = await _report.GetSurveyReport(queryParams.PageIndex,
                queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportSurveyReportToExcel")]
        public async Task<FileContentResult> ExportSurveyReportToExcel([FromQuery] GetAuditReportParams queryParams)
        {
            FileData fileData = await _report.ExportSurveyReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetConsumerSurveyReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetConsumerSurveyReport([FromQuery] PaginationParams queryParams)
        {
            Pagination<DailyTaskModel> report = await _report.GetConsumerSurveyReport(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportConsumerSurveyReportToExcel")]
        public async Task<FileContentResult> ExportConsumerSurveyReportToExcel([FromQuery] PaginationParams queryParams)
        {
            FileData fileData = await _report.ExportConsumerSurveyReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetAvReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetAvReport([FromQuery] PaginationParams queParams)
        {
            Pagination<DailyTaskModel> report = await _report.GetAvReport(queParams.PageIndex, queParams.PageSize, queParams.Search, queParams.FromDateTime, queParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportAvReportToExcel")]
        public async Task<FileContentResult> ExportAvReportToExcel([FromQuery] PaginationParams queryParams)
        {
            FileData fileData = await _report.ExportAvReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetCommunicationReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetCommunicationReport([FromQuery] PaginationParams queryParams)
        {
            Pagination<DailyTaskModel> report = await _report.GetCommunicationReport(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportCommunicationReportToExcel")]
        public async Task<FileContentResult> ExportCommunicationReportToExcel([FromQuery] PaginationParams queryParams)
        {
            FileData fileData = await _report.ExportCommunicationReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetInformationReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetInformationReport([FromQuery] PaginationParams queryParams)
        {
            Pagination<DailyTaskModel> report = await _report.GetInformationReport(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(report);
        }

        [JwtAuthorize]
        [HttpGet("ExportInformationReportToExcel")]
        public async Task<FileContentResult> ExportInformationReportToExcel([FromQuery] PaginationParams queryParams)
        {
            FileData fileData = await _report.ExportInformationReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpGet("GetPOSMTaskReport")]
        public async Task<ActionResult<Pagination<DailyTaskModel>>> GetPOSMTaskReport([FromQuery] PaginationParams queryParams)
        {

            Pagination<DailyTaskModel> result = await _report.GetPOSMTaskReport(queryParams.PageIndex,queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpGet("ExportPOSMTaskReportToExcel")]
        public async Task<FileContentResult> ExportPOSMTaskReportToExcel([FromQuery] PaginationParams queryParams)
        {
            FileData fileData = await _report.ExportPOSMTaskReportToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }


        [JwtAuthorize]
        [HttpGet("ExportCWStockUpdateReportToExcel")]
        public async Task<FileContentResult> ExportCwStockUpdateToExcel([FromQuery] CWStockUpdateRequestParams queryParams)
        {
            FileData fileData = await _report.ExportCWStockUpdateToExcel(queryParams.PageIndex, queryParams.PageSize, queryParams.Search, queryParams.FromDateTime, queryParams.ToDateTime, queryParams.CWIds);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpPost("ExportCWDistributionReportToExcel")]
        public async Task<FileContentResult> ExportCWDistributionReportToExcel([FromBody] ExportCWDistributionReportToExcelModel payload)
        {
            FileData fileData = await _report.ExportCWDistributionReportToExcel(payload);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpPost("ExportCWStockReportToExcel")]
        public async Task<FileContentResult> ExportCWStockReportToExcel([FromBody] CustomObject<List<int>> payload)
        {
            FileData fileData = await _report.ExportCWStockReportToExcel(payload.Data);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpPost("ExportSPStockReportToExcel")]
        public async Task<FileContentResult> ExportSPStockReportToExcel([FromBody] CustomObject<List<int>> payload)
        {
            FileData fileData = await _report.ExportSPStockReportToExcel(payload.Data);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }

        [JwtAuthorize]
        [HttpPost("ExportSPWisePosmLedgerReportToExcel")]
        public async Task<FileContentResult> ExportSPWisePosmLedgerReportToExcel([FromBody] ExportSPWisePosmLedgerPayload payload)
        {
            FileData fileData = await _report.ExportSPWisePosmLedgerReportToExcel(payload);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
        }
    }
}