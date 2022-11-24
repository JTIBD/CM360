using FMAplication.Services.FileUtility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Core.Params;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.WareHouse;
using FMAplication.Models.Transaction;
using FMAplication.Services.inventory.implementation;
using FMAplication.Models.wareHouse;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using FMAplication.Domain.Transaction;
using X.PagedList;
using FMAplication.RequestModels;

namespace FMAplication.Services.inventory.interfaces
{
    public interface IInventoryService
    {
        FileData GetFileDataForStockAdd(int wareHouseId); 
        Task<List<POSMProductStockModel>> GetProductStock(int warehouseId);
        List<WareHouseModel> GetWareHouses();
        Task<TransactionAdjustmentModel> GetStockAdjustmentTransaction();
        Task SaveStockAdjustmentTransaction(TransactionAdjustmentModel model);
        Task<TransactionAdjustmentModel> GetStockAdjustmentTransaction(int transactionId);
        Task<TransactionModel> CreateStockAddTransaction(CreateStockAddTransaction model);
        List<TransactionModel> GetTransactions(TransactionType transactionType);
        Task UpdateStockAdjustmentTransaction(TransactionAdjustmentModel model);
        Task<TransactionModel> UpdateTransaction(TransactionModel model);
        Task<TransactionModel> ConfirmStockAddTransaction(int transactionId);
        Task<TransactionModel> UpdateStockAddTransaction(UpdateStockAddTransaction model);

        Task<Pagination<TransactionAdjustmentModel>> GetStockAdjustTransactions(
            AdjustmentTransactionParams transactionParams);
        Task<TransactionAdjustmentModel> ConfirmCWStockAdjustmentTransaction(int transactionId);
        Task CheckWarehouseStockAvailableAmount(Transaction transaction);
        Task ApproveCWStockAdjustment(int transactionId);
        Task<FileData> GetFileDataForWStockDistribution(DownloadExcelForStockDistributions payload);
        Task<List<TransactionModel>> CreateWPOSM_DistributionTransaction(CreateWPOSM_DistributionTransaction model);
        Task<TransactionModel> ConfirmWDistributionTransaction(int transactionId);
        Task<TransactionModel> ReceivePOSMStock(TransactionModel transaction);
        Task<Pagination<TransactionModel>> GetStockDistributionTransactions(int limit, int skip, DateTime startFrom,
            DateTime toDateTime, string search);
        Task<int> GetStockDistributionTotalTransactionsCount();
        Task<TransactionModel> ConfirmRecievedPOSM(int transactionId);
        Task<Pagination<TransactionModel>> GetReceivedTransactions(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search);
        Task<List<TransactionModel>> GetReceivablePOSMDistributionByCurrentUser();
        Task<Pagination<TransactionModel>> GetStockAddTransactions(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search);
        Task<Pagination<TransactionModel>> GetSalesPointStockAdjustTransactions(GetSalesPointAdjustmentTransactions transactionParams);
        Task SaveSalesPointStockAdjustmentTransaction(TransactionModel model);
        Task<TransactionModel> ConfirmSalesPointStockAdjustmentTransaction(int transactionId);
        Task<TransactionModel> GetSalesPointStockAdjustmentTransaction(int transactionId);
        Task<TransactionModel> UpdateSalePointStockAdjustmentTransaction(TransactionModel model);
        Task ApproveTransaction(Transaction transaction);
        Task SPStockRemove(List<DailyPosmTaskItems> posmTaskItemses, int salesPointId);
    }

}
