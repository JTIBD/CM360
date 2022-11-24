using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Common;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.Warehouses.interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMAplication.Controllers.WareHouses
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class WareHouseController:BaseController
    {
        private readonly IWareHouseService _wareHouse;
        public WareHouseController(IWareHouseService wareHouse)
        {
            _wareHouse = wareHouse;
        }

        [JwtAuthorize]
        [HttpPost("GetByCodes")]
        public async Task<ActionResult<List<WareHouseModel>>> GetByCodes([FromBody]CustomObject<List<string>> codes)
        {
            var result = await _wareHouse.GetByCodes(codes.Data);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("getStocks")]
        public async Task<ActionResult<Pagination<WareHouseStockModel>>> GetStocks([FromBody] GetWareHouseStockModel model)
        {
            Pagination<WareHouseStockModel> data = await _wareHouse.GetStocks(model.PageSize, model.PageIndex, model.Search,model.WarehouseIds);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpPost("AddWareHouseTransfer")]
        public async Task<ActionResult<WareHouseTransferModel>> AddWareHouseTransaction(WareHouseTransferModel model)
        {
            WareHouseTransferModel result = await _wareHouse.AddDistributionTransaction(model);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("DownloadExcelForWareHouseDistribution")]
        public async Task<FileContentResult> DownloadExcelForWareHouseDistribution([FromBody] DownloadExcelForWareHouseDistribution payload)
        {
            FileData fileData = await _wareHouse.DownloadExcelForWareHouseDistribution(payload);

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
        [HttpGet("GetWareHouseTransfers")]
        public async Task<ActionResult<Pagination<WareHouseTransferModel>>> GetWareHouseTransactions([FromQuery] GetWareHouseTransactions queryParams)
        {
            Pagination<WareHouseTransferModel> data = await _wareHouse.GetWareHouseTransactions(queryParams);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpPut("ConfirmWareHouseTransfer")]
        public async Task<ActionResult<WareHouseTransferModel>> ConfirmWareHouseTransfer([FromQuery] int transactionId)
        {
            WareHouseTransferModel result = await _wareHouse.ConfirmWareHouseTransfer(transactionId);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpGet("GetReceivableTransfers")]
        public async Task<ActionResult<List<WareHouseTransferModel>>> GetReceivableTransfers()
        {            
            List<WareHouseTransferModel> res = await _wareHouse.GetReceivableTransfersByCurrentUser();
            return Ok(res);         
        }

        [JwtAuthorize]
        [HttpPost("RecieveTransfer")]
        public async Task<ActionResult<WareHouseReceivedTransferModel>> RecieveTransfer([FromBody] WareHouseReceivedTransferModel transaction)
        {
            WareHouseReceivedTransferModel result = await _wareHouse.RecieveTransfer(transaction);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpGet("GetReceivedTransfers")]
        public async Task<ActionResult<Pagination<WareHouseReceivedTransferModel>>> GetReceivedTransfers([FromQuery] GetWareHouseReceivedTransfers queryParams)
        {            
            Pagination<WareHouseReceivedTransferModel> data = await _wareHouse.GetReceivedTransfers(queryParams);
            return Ok(data);            
        }
    }
}
