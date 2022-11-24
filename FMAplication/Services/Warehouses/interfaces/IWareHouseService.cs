using FMAplication.common;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.FileUtility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.Warehouses.interfaces
{
    public interface IWareHouseService
    {
        Task<Pagination<WareHouseStockModel>> GetStocks(int pageSize, int pageIndex, string search,
            List<int> wareHouseIds);

        Task<FileData> DownloadExcelForWareHouseDistribution(DownloadExcelForWareHouseDistribution payload);
        Task<WareHouseTransferModel> AddDistributionTransaction(WareHouseTransferModel model);
        Task<Pagination<WareHouseTransferModel>> GetWareHouseTransactions(GetWareHouseTransactions queryParams);
        Task<WareHouseTransferModel> ConfirmWareHouseTransfer(int transactionId);
        Task<List<WareHouseModel>> GetByCodes(List<string> codes);
        Task<List<WareHouseTransferModel>> GetReceivableTransfersByCurrentUser();
        Task<WareHouseReceivedTransferModel> RecieveTransfer(WareHouseReceivedTransferModel transaction);
        Task<Pagination<WareHouseReceivedTransferModel>> GetReceivedTransfers(GetWareHouseReceivedTransfers queryParams);
        Task InsertWarehouseProductAvailableAmount(List<WareHouseStockModel> stockData);
    }
}
