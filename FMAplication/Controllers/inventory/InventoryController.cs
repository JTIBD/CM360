using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.inventory.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Core.Params;
using FMAplication.Models.Transaction;
using FMAplication.Services.inventory.implementation;
using FMAplication.Models.wareHouse;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using FMAplication.Filters;
using FMAplication.Domain.Transaction;
using X.PagedList;
using FMAplication.Models.Common;
using FMAplication.RequestModels;
using FMAplication.RequestModels.Bases;

namespace FMAplication.Controllers.inventory
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class InventoryController : BaseController
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventory;

        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventory)
        {
            _logger = logger;
            _inventory = inventory;
        }

        [JwtAuthorize]
        [HttpGet("DownloadExcelForStockAdd/{wareHouseId}")]
        public async Task<FileContentResult> DownloadExcelForStockAdd(int wareHouseId)
        {
           
            FileData fileData = _inventory.GetFileDataForStockAdd(wareHouseId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(fileData.Data, fileData.ContentType);
                     
        }

        [JwtAuthorize]
        [HttpPost("DownloadExcelForWStockDistribution")]
        public async Task<FileContentResult> DownloadExcelForWStockDistribution([FromBody] DownloadExcelForStockDistributions payload)
        {

            FileData fileData = await _inventory.GetFileDataForWStockDistribution(payload);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileData.Name,
                Inline = true,
            };

            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            var r = File(fileData.Data, fileData.ContentType);
            return await Task.FromResult(r);

        }

        [HttpGet("GetProductStocks/{wareHouseId}")]
        public async Task<ActionResult<POSMProductStockModel>> GetProductStocks(int warehouseId)
        {
            var data = await _inventory.GetProductStock(warehouseId);
            return Ok(data);
        }
        [HttpGet("GetAdjustmentTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> GetAdjustmentTransaction()
        {
            var data = await _inventory.GetStockAdjustmentTransaction();
            return Ok(data);
        }        

        [JwtAuthorize]
        [HttpGet("GetStockDistributionTransactions")]
        public async Task<ActionResult<Pagination<TransactionModel>>> GetStockDistributionTransactions([FromQuery] PaginationParams queryParams)
        {

            Pagination<TransactionModel> data =
                await _inventory.GetStockDistributionTransactions(queryParams.PageSize, queryParams.PageIndex, queryParams.FromDateTime, queryParams.ToDateTime,
                    queryParams.Search);
            return Ok(data);
        
        }

        [JwtAuthorize]
        [HttpGet("GetStockAddTransactions2")]
        public async Task<ActionResult<Pagination<TransactionModel>>> GetStockAddTransactions([FromQuery] PaginationParams queryParams)
        {
            Pagination<TransactionModel> data = await _inventory.GetStockAddTransactions(queryParams.PageSize, queryParams.PageIndex, queryParams.FromDateTime, queryParams.ToDateTime, queryParams.Search);
            return Ok(data);
         
        }

        [JwtAuthorize]
        [HttpGet("GetReceivedTransactions")]
        public async Task<ActionResult<Pagination<TransactionModel>>> GetReceivedTransactions([FromQuery] PaginationParams queryParams)
        {
        
            Pagination<TransactionModel> data = await _inventory.GetReceivedTransactions(queryParams.PageSize, queryParams.PageIndex, queryParams.FromDateTime, queryParams.ToDateTime, queryParams.Search);                
            return Ok(data);
        
        }

        [JwtAuthorize]
        [HttpGet("GetReceivablePOSMDistributionByCurrentUser")]
        public async Task<ActionResult<List<TransactionModel>>> GetReceivablePOSMDistributionByCurrentUser()
        {
            
            List<TransactionModel> res = await _inventory.GetReceivablePOSMDistributionByCurrentUser();
            return Ok(res);

        }

        [JwtAuthorize]
        [HttpGet("GetStockDistributionTotalTransactionsCount")]
        public async Task<ActionResult<int>> GetStockDistributionTotalTransactionsCount()
        {
        
            int totalCount = await _inventory.GetStockDistributionTotalTransactionsCount();
            return Ok(totalCount);
        
        }

        [HttpGet("GetAdjustmentTransactionById/{transactionId}")]
        public async Task<ActionResult<TransactionAdjustmentModel>> GetAdjustmentTransactionById(int transactionId)
        {
            var data = await _inventory.GetStockAdjustmentTransaction(transactionId);
            return Ok(data);
        }

        [HttpGet("GetSalesPointAdjustmentTransactionById/{transactionId}")]
        public async Task<ActionResult<TransactionModel>> GetSalesPointAdjustmentTransactionById(int transactionId)
        {
            TransactionModel data = await _inventory.GetSalesPointStockAdjustmentTransaction(transactionId);
            return Ok(data);
        }

        [HttpPost("SaveAdjustmentTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> SaveAdjustmentTransaction(TransactionAdjustmentModel model)
        {
            await _inventory.SaveStockAdjustmentTransaction(model);
            return Ok();
        }

        [HttpPost("SaveSalesPointAdjustmentTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> SaveSalesPointAdjustmentTransaction(TransactionModel model)
        {            
            await _inventory.SaveSalesPointStockAdjustmentTransaction(model);
            return Ok();
        }

        [HttpPost("UpdateAdjustmentTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> UpdateAdjustmentTransaction(TransactionAdjustmentModel model)
        {
        
            await _inventory.UpdateStockAdjustmentTransaction(model);
            return Ok();
        }

        [JwtAuthorize]
        [HttpPost("UpdateSalesPointAdjustmentTransaction")]
        public async Task<ActionResult<TransactionModel>> UpdateSalesPointAdjustmentTransaction(TransactionModel model)
        {           
            TransactionModel result = await _inventory.UpdateSalePointStockAdjustmentTransaction(model);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("AddStockTransaction")]
        public async Task<ActionResult<TransactionModel>> AddStockTransaction([FromBody] CreateStockAddTransaction model)
        {
        
            TransactionModel result = await _inventory.CreateStockAddTransaction(model);
            return Ok(result);

        }

        [JwtAuthorize]
        [HttpPost("AddWPOSM_DistributionTransaction")]
        public async Task<ActionResult<List<TransactionModel>>> AddWPOSM_DistributionTransaction(CreateWPOSM_DistributionTransaction model)
        {
        
            List<TransactionModel> result = await _inventory.CreateWPOSM_DistributionTransaction(model);
            return Ok(result);
        }


        [JwtAuthorize]
        [HttpPut("UpdateStockAddTransaction")]
        public async Task<ActionResult<TransactionModel>> UpdateStockAddTransaction([FromBody] UpdateStockAddTransaction model)
        {
            
            TransactionModel result = await _inventory.UpdateStockAddTransaction(model);
            return Ok(result);

        }
        [JwtAuthorize]
        [HttpPut("UpdateTransaction")]
        public async Task<ActionResult<TransactionModel>> UpdateTransaction([FromBody] TransactionModel model)
        {
        
            TransactionModel result = await _inventory.UpdateTransaction(model);
            return Ok(result);

        }
        
        [JwtAuthorize]
        [HttpPut("ConfirmStockAddTransaction")]
        public async Task<ActionResult<TransactionModel>> ConfirmStockAddTransaction([FromQuery] int transactionId)
        {
        
            TransactionModel result = await _inventory.ConfirmStockAddTransaction(transactionId);
            return Ok(result);

        }

        [JwtAuthorize]
        [HttpPut("ConfirmWDistributionTransaction")]
        public async Task<ActionResult<TransactionModel>> ConfirmWDistributionTransaction([FromQuery] int transactionId)
        {
        
            TransactionModel result = await _inventory.ConfirmWDistributionTransaction(transactionId);
            return Ok(result);

        }

        [JwtAuthorize]
        [HttpPost("RecievePOSM_Stock")]
        public async Task<ActionResult<TransactionModel>> RecievePOSMStock([FromBody] TransactionModel transaction)
        {
        
            TransactionModel result = await _inventory.ReceivePOSMStock(transaction);
            return Ok(result);
        }


        [JwtAuthorize]
        [HttpGet("GetWareHouses")]
        public async Task<ActionResult<List<WareHouseModel>>> GetWareHouses()
        {
            
            List<WareHouseModel> wareHouseModels = _inventory.GetWareHouses();
            return Ok(wareHouseModels);
        }

        [JwtAuthorize]
        [HttpGet("GetTransactions")]
        public async Task<ActionResult<List<TransactionModel>>> GetTransactions([FromQuery] TransactionType transactionType)
        {
        
            List<TransactionModel> transactionModels = _inventory.GetTransactions(transactionType);
            return Ok(transactionModels);
        
        }
        
        [HttpGet("GetStockAdjustTransactions")]
        public async Task<ActionResult<Pagination<TransactionAdjustmentModel>>> GetStockAdjustmentTransactions([FromQuery]AdjustmentTransactionParams transactionParams)
        {
        
            var data = await _inventory.GetStockAdjustTransactions(transactionParams);
            return Ok(data);
        }

        [JwtAuthorize]
        [HttpGet("GetSalesPointStockAdjustTransactions")]
        public async Task<ActionResult<Pagination<TransactionModel>>> GetSalesPointStockAdjustTransactions([FromQuery] GetSalesPointAdjustmentTransactions transactionParams)
        {            
            Pagination<TransactionModel> data = await _inventory.GetSalesPointStockAdjustTransactions(transactionParams);
            return Ok(data);
        }

        [HttpPut("ConfirmStockAdjustTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> ConfirmStockAdjustTransaction([FromQuery] int transactionId)
        {

            TransactionAdjustmentModel transaction = await _inventory.ConfirmCWStockAdjustmentTransaction(transactionId);
            return Ok(transaction);

        }

        [HttpPut("ConfirmSalesPointStockAdjustTransaction")]
        public async Task<ActionResult<TransactionAdjustmentModel>> ConfirmSalesPointStockAdjustTransaction([FromQuery] int transactionId)
        {            
            TransactionModel transaction = await _inventory.ConfirmSalesPointStockAdjustmentTransaction(transactionId);
            return Ok(transaction);            
        }

        [HttpPut("ConfirmRecievedPOSM")]
        public async Task<ActionResult<TransactionModel>> ConfirmRecievedPOSM([FromQuery] int transactionId)
        {
            
            TransactionModel transaction = await _inventory.ConfirmRecievedPOSM(transactionId);
            return Ok(transaction);
        }

    }
}
