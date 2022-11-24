using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Filters;
using FMAplication.Models.Transaction;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.inventory.implementation;
using FMAplication.Services.inventory.interfaces;
using FMAplication.Services.TransactionWorkflow;

namespace FMAplication.Controllers.TransactionWorkflow
{
    [ApiController]
    //    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TransactionWorkflowController :  BaseController
    {
        private readonly ITransactionWorkflowService _transactionWorkflow;
        private readonly IInventoryService _inventoryService;
        private readonly ICommonService _commonService;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<SalesPointTransfer> _salesPointTransfer;


        public TransactionWorkflowController(ITransactionWorkflowService transactionWorkflow, 
            IInventoryService inventoryService, ICommonService commonService, IRepository<Transaction> transaction, 
            IRepository<SalesPointTransfer> salesPointTransfer)
        {
            _transactionWorkflow = transactionWorkflow;
            _inventoryService = inventoryService;
            _commonService = commonService;
            _transaction = transaction;
            _salesPointTransfer = salesPointTransfer;
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptWorkflow(TransactionNotification model)
        {
            var transaction = await GetTransaction(model);
            var appIdentity = AppIdentity.AppUser;

            await CheckTransactionStockForAvailableAmount(transaction);

            await _transactionWorkflow.AcceptWorkflow(appIdentity.UserId, 
                model.TransactionWorkFlowId, transaction);

            if (await _transactionWorkflow.IsAllAccepted(transaction))
                await _inventoryService.ApproveTransaction(transaction);
            else 
                await _transactionWorkflow.SendNotification(transaction);

            return Ok();
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectWorkflow(TransactionNotification model)
        {

            var transaction = await GetTransaction(model);
            var appIdentity = AppIdentity.AppUser;

            await _transactionWorkflow.RejectWorkflow(appIdentity.UserId,
                model.TransactionWorkFlowId,transaction);

            return Ok();
        }


        #region private methods

        private async Task<Transaction> GetTransaction(TransactionNotification model)
        {
            if (model.TransactionType == TransactionType.SP_Transfer)
            {
                var salesPointTransfer = await _salesPointTransfer.FindAsync(x => x.Id == model.TransactionId);
                if (salesPointTransfer == null) throw new AppException("Transaction not found");
                var result = salesPointTransfer.ToMap<SalesPointTransfer, Transaction>();
                result.TransactionType = TransactionType.SP_Transfer;
                result.SalesPointId = salesPointTransfer.FromSalesPointId;
                return result;
            }
            var transaction = _transaction.GetAllActive().FirstOrDefault(x=>x.Id == model.TransactionId);
            if (transaction == null) throw new AppException("Transaction not found");
            return transaction;
        }

        private async Task CheckTransactionStockForAvailableAmount(Transaction model)
        {
            if (model.TransactionType == TransactionType.StockAdjustment)
                await _inventoryService.CheckWarehouseStockAvailableAmount(model);
            else if (model.TransactionType == TransactionType.SalesPointStockAdjustment)
                 _commonService.CheckSpAvailableQuantityViolationForSPAdjustment(model.Id);
            else if (model.TransactionType == TransactionType.SP_Transfer)
                _commonService.CheckSpTransferAvailableQuantityViolation(model.Id);
        }

        #endregion
    }
}
