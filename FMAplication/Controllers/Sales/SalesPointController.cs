using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Services.Sales.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Models.Nodes;
using FMAplication.common;
using FMAplication.Models.Common;
using FMAplication.Models.Sales;
using FMAplication.RequestModels.Sales;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.FileUtility.Implementation;

namespace FMAplication.Controllers.Sales
{
    [ApiController]
    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SalesPointController:BaseController
    {
        private readonly ILogger<SalesPointController> _logger;
        private readonly ISalesPointService _salesPoint;
        private readonly ICommonService _commonService;

        public SalesPointController(ILogger<SalesPointController> logger, ISalesPointService salesPoint, 
            ICommonService commonService)
        {
            _logger = logger;
            _salesPoint = salesPoint;
            _commonService = commonService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllSalesPoint()
        {
            try
            {
                var result = await _salesPoint.GetAllSalesPointAsync();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [JwtAuthorize]
        [HttpGet("GetAllSalesPointByCurrentFmUser")]
        public  IActionResult GetAllSalesPointByCurrentUser()
        {
            try
            {
                var result =  _commonService.GetSalesPointsByFMUser(AppIdentity.AppUser.UserId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [JwtAuthorize]
        [HttpPost("GetStocks")]
        public async Task<ActionResult<Pagination<SalesPointStockModel>>> GetStocks([FromBody] GetSalesPointStockModel payload)
        {
            Pagination<SalesPointStockModel> data = await _salesPoint.GetStocks(payload);
            return Ok(data);
        }


        [JwtAuthorize]
        [HttpPost("AddSalesPointTransfer")]
        public async Task<ActionResult<List<SalesPointTransferModel>>> AddSalesPointTransaction(CustomObject<List<SalesPointTransferModel>> model)
        {

            var result = await _salesPoint.AddSalesPointTransfers(model.Data);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("DownloadExcelForSalesPointTransfer")]
        public async Task<FileContentResult> DownloadExcelForSalesPointTransfer([FromBody] DownloadExcelForSalesPointDistribution payload)
        {
            FileData fileData = _salesPoint.DownloadExcelForSalesPointDistribution(payload);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            var r = File(fileData.Data, fileData.ContentType);
            return await Task.FromResult(r);

        }

        [JwtAuthorize]
        [HttpGet("GetSalesPointTransfers")]
        public async Task<ActionResult<Pagination<SalesPointTransferModel>>> GetSalesPointTransactions([FromQuery] GetSalesPointTransactions queryParams)
        {
            Pagination<SalesPointTransferModel> data = await _salesPoint.GetSalesPointTransactions(queryParams);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpGet("GetSalesPointTransferById/{id}")]
        public async Task<ActionResult<SalesPointTransferModel>> GetSalesPointTransferById([FromRoute] int id)
        {
            SalesPointTransferModel data = await _salesPoint.GetSalesPointTransferById(id);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpPut("ConfirmSalesPointTransfer")]
        public async Task<ActionResult<SalesPointTransferModel>> ConfirmSalesPointTransfer([FromQuery] int transactionId)
        {
            SalesPointTransferModel result = await _salesPoint.ConfirmSalesPointTransfer(transactionId);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpGet("GetReceivableTransfers")]
        public async Task<ActionResult<List<SalesPointTransferModel>>> GetReceivableTransfers()
        {
            List<SalesPointTransferModel> res = await _salesPoint.GetReceivableTransfersByCurrentUser();
            return Ok(res);
        }

        [JwtAuthorize]
        [HttpPost("RecieveTransfer")]
        public async Task<ActionResult<SalesPointReceivedTransferModel>> RecieveTransfer([FromBody] SalesPointReceivedTransferModel transaction)
        {
            SalesPointReceivedTransferModel result = await _salesPoint.RecieveTransfer(transaction);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpGet("GetReceivedTransfers")]
        public async Task<ActionResult<Pagination<SalesPointReceivedTransferModel>>> GetReceivedTransfers([FromQuery] GetSalesPointReceivedTransfers queryParams)
        {
            Pagination<SalesPointReceivedTransferModel> data = await _salesPoint.GetReceivedTransfers(queryParams);
            return Ok(data);
        }
    }
}
