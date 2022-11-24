using FMAplication.common;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Domain.Products;
using FMAplication.Models.Common;
using FMAplication.RequestModels.Sales;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.FileUtility.Implementation;

namespace FMAplication.Services.Sales.Interfaces
{
    public interface ISalesPointService
    {
        Task<IEnumerable<SalesPointModel>> GetAllSalesPointAsync();
        Task<SalesPointModel> GetSalesPointAsync(int id);
        Task<IEnumerable<SalesPointModel>> GetQuerySalesPointAsync();
        
        Task<SalesPointModel> SaveAsync(SalesPointModel model);
       
        Task<SalesPointModel> CreateAsync(SalesPointModel model);
       
        Task<SalesPointModel> UpdateAsync(SalesPointModel model);
        Task<int> DeleteAsync(int id);
        Task<Pagination<SalesPointStockModel>> GetStocks(GetSalesPointStockModel payload);
        Task<List<SalesPointTransferModel>> AddSalesPointTransfers(List<SalesPointTransferModel> model);
        Task<Pagination<SalesPointTransferModel>> GetSalesPointTransactions(GetSalesPointTransactions data);
        Task<SalesPointTransferModel> ConfirmSalesPointTransfer(int transactionId);
        Task ApproveAwaitingReceiveSalesPointTransfer(int transactionId);
        Task<List<SalesPointModel>> GetByCodes(List<string> codes);
        Task<List<SalesPointTransferModel>> GetReceivableTransfersByCurrentUser();
        Task<SalesPointReceivedTransferModel> RecieveTransfer(SalesPointReceivedTransferModel transaction);

        Task<Pagination<SalesPointReceivedTransferModel>> GetReceivedTransfers(
            GetSalesPointReceivedTransfers queryParams);
        FileData DownloadExcelForSalesPointDistribution(DownloadExcelForSalesPointDistribution payload);
        Task AddPosmToSalesPointStockAndPosmLedger(POSMProduct posmProduct);
        Task<SalesPointTransferModel> GetSalesPointTransferById(int id);
    }
}

