using FMAplication.common;
using FMAplication.Domain.Products;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;
using FMAplication.Repositories;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.Warehouses.interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Warehouses.implementations
{
    public class WareHouseService : IWareHouseService
    {
        private readonly IRepository<WareHouseStock> _wStock;
        private readonly IRepository<WareHouseTransfer> _wareHouseTransaction;
        private readonly IRepository<WareHouseReceivedTransfer> _wareHouseReceivedTransfer;
        private readonly IRepository<WareHouseTransferItem> _wareHouseTransactionItem;
        private readonly IRepository<WareHouseReceivedTransferItem> _wareHouseReceivedTransferItem;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<WDistributionTransaction> _cwDistributions;
        private readonly IRepository<StockAdjustmentItems> _cwStockAdjustmentItems;

        private readonly IRepository<WareHouse> _wareHouse;
        private readonly IRepository<POSMProduct> _posmProduct;
        public readonly IFileService _file;
        public readonly ICommonService _common;

        public WareHouseService(IRepository<WareHouseStock> wStock, IRepository<WareHouse> wareHouse,
            IRepository<POSMProduct> posmProduct, IFileService file, IRepository<WareHouseTransfer> wareHouseTransaction,
            ICommonService common, IRepository<WareHouseTransferItem> wareHouseTransactionItem,
            IRepository<WareHouseReceivedTransfer> wareHouseReceivedTransfer, IRepository<WareHouseReceivedTransferItem> wareHouseReceivedTransferItem, 
            IRepository<Transaction> transaction, IRepository<WDistributionTransaction> cwDistributions, 
            IRepository<StockAdjustmentItems> cwStockAdjustmentItems)
        {
            _wStock = wStock;
            _wareHouse = wareHouse;
            _posmProduct = posmProduct;
            _file = file;
            _wareHouseTransaction = wareHouseTransaction;
            _common = common;
            _wareHouseTransactionItem = wareHouseTransactionItem;
            _wareHouseReceivedTransfer = wareHouseReceivedTransfer;
            _wareHouseReceivedTransferItem = wareHouseReceivedTransferItem;
            _transaction = transaction;
            _cwDistributions = cwDistributions;
            _cwStockAdjustmentItems = cwStockAdjustmentItems;
        }

        public async Task<FileData> DownloadExcelForWareHouseDistribution(DownloadExcelForWareHouseDistribution payload)
        {
            WareHouseDistributionData taskCreationData = await GetWStockDistributionData(payload.FromWareHouseId, payload.ToWareHouseId);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = GetWStockDistributionExcelHeaders(taskCreationData);

            SetTableStyle(workSheet, headers.Count);
            SetHeaderStyle(workSheet, headers.Count);
            InsertHeaders(headers, workSheet);
            Insert_WareHousePOSM_DistributionExcelRows(taskCreationData, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        public async Task<Pagination<WareHouseStockModel>> GetStocks(int pageSize, int pageIndex, string search,
            List<int> wareHouseIds)
        {
            
            var query = _wStock.GetAllActive().Where(x => x.POSMProduct.Status == Status.Active && x.POSMProduct.IsJTIProduct && wareHouseIds.Contains(x.WareHouseId));
            
            if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.WareHouse.Code.Contains(search)
                                                                             || x.POSMProduct.Name.Contains(search)
                                                                             || x.POSMProduct.Code.Contains(search));
            query = query.Include(x => x.POSMProduct).Include(x => x.WareHouse);
            query = query.OrderByDescending(x => x.CreatedTime);
            var stocks = await query.ToPagedListAsync(pageIndex, pageSize);
            var stockModel = stocks.ToList().MapToModel();

            await InsertWarehouseProductAvailableAmount(stockModel);
           
            Pagination<WareHouseStockModel> paginatedList = new Pagination<WareHouseStockModel>(pageIndex, pageSize, stocks.TotalItemCount, stockModel);

            return paginatedList;
        }

        private async Task<WareHouseDistributionData> GetWStockDistributionData(int fromWareHouseId, int toWareHouseId)
        {
            WareHouseDistributionData distributionData = new WareHouseDistributionData();

            distributionData.FromWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == fromWareHouseId).ToMap<WareHouse, WareHouseModel>();
            distributionData.ToWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == toWareHouseId).ToMap<WareHouse, WareHouseModel>();

            var warehouseStocks = _wStock.GetAllActive().Where(x => x.WareHouseId == fromWareHouseId).ToList();
            var warehouseStockDataModel = warehouseStocks.MapToModel();

            await InsertWarehouseProductAvailableAmount(warehouseStockDataModel);
            var availablePosmIds = warehouseStockDataModel.Where(x => x.AvailableQuantity > 0).Select(x => x.PosmProductId).ToList();

            var posmProducts = _posmProduct.GetAllActive().Where(x => x.IsJTIProduct && availablePosmIds.Contains(x.Id)).ToList().ToMap<POSMProduct, POSMProductModel>();
            distributionData.PosmProducts = posmProducts;

            return distributionData;
        }

        private List<string> GetWStockDistributionExcelHeaders(WareHouseDistributionData taskCreationData)
        {
            var headers = new List<string>() { "From Warehouse Code", "From Warehouse Name", "To Warehouse Code","To Warehouse Name", };
            var posmProductsNames = taskCreationData.PosmProducts.Select(x => x.Name).ToList();
            headers.AddRange(posmProductsNames);
            return headers;
        }

        private void SetTableStyle(ExcelWorksheet workSheet, int columnCount)
        {
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
        }
        public void SetHeaderStyle(ExcelWorksheet workSheet, int headCount)
        {
            var header = workSheet.Cells[1, 1, 1, headCount];
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(194, 194, 194));
            header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }
        public async Task<WareHouseTransferModel> AddDistributionTransaction(WareHouseTransferModel model)
        {
            var user = AppIdentity.AppUser;
            if (!model.Items.Any()) throw new AppException("No distribution data provided");
            WareHouseTransfer wareHouseTransaction = model.ToMap<WareHouseTransferModel, WareHouseTransfer>();
            wareHouseTransaction.CreatedBy = user.UserId;
            wareHouseTransaction.TransactionDate = DateTime.UtcNow;
            var fromWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == model.FromWarehouseId);
            if (fromWareHouse is null) throw new AppException("Source warehouse not found.");
            var toWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == model.ToWarehouseId);
            if (toWareHouse is null) throw new AppException("Destination warehouse not found.");
            var serial = _common.GetTransactionCount(TransactionType.CW_Transfer)+1;
            wareHouseTransaction.TransactionNumber = await _common.GetTransactionNumber(TransactionType.CW_Transfer,fromWareHouse.Code,toWareHouse.Code,serial);
            await _wareHouseTransaction.CreateAsync(wareHouseTransaction);

            var items = model.Items.ToMap<WareHouseTransactionItemModel, WareHouseTransferItem>();
            items.ForEach(x => x.TransactionId = wareHouseTransaction.Id);
            await _wareHouseTransactionItem.CreateListAsync(items);
            var result = wareHouseTransaction.MapToModel();
            result.Items = items.MapToModel();
            return result;                      
        }

        public async Task<Pagination<WareHouseTransferModel>> GetWareHouseTransactions(GetWareHouseTransactions data)
        {
            if (data.FromDateTime > data.ToDateTime) (data.FromDateTime,data.ToDateTime) = (data.ToDateTime, data.FromDateTime);
            var query = _wareHouseTransaction.GetAllActive().Where(x => x.TransactionDate >= data.FromDateTime&& x.TransactionDate <= data.ToDateTime);
            if (!string.IsNullOrWhiteSpace(data.Search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(data.Search));
            }
            query = query.Include(x=>x.FromWarehouse).OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(data.PageIndex,data.PageSize);
            var transactionModels = transactions.ToList().MapToModel();
            var tIds = transactions.Select(x => x.Id).ToList();

            var toWareHouseIds = transactions.Select(x => x.ToWarehouseId).ToList();
            var toWareHouses = _wareHouse.GetAllActive().Where(x => toWareHouseIds.Contains(x.Id)).ToList().ToMap<WareHouse, WareHouseModel>();

            var items = _wareHouseTransactionItem.GetAllActive().Where(x => tIds.Contains(x.TransactionId)).Include(x=>x.POSMProduct).ToList();
            transactionModels.ForEach(x => {
                x.ToWarehouse = toWareHouses.Find(w => w.Id == x.ToWarehouseId);
                x.Items = items.FindAll(i => i.TransactionId == x.Id).MapToModel();
            });
            var result = new Pagination<WareHouseTransferModel>(data.PageIndex, data.PageSize,transactions.TotalItemCount, transactionModels);
            return result;
        }

        public async Task<WareHouseTransferModel> ConfirmWareHouseTransfer(int transactionId)
        {
            var transaction = _wareHouseTransaction.GetAllActive().FirstOrDefault(x => x.Id == transactionId);
            if (transaction is null) throw new AppException("Transaction not found");

            var posms = _wareHouseTransactionItem.GetAllActive().Where(x => x.TransactionId == transactionId).Include(x=>x.POSMProduct).ToList();
            var posmIds = posms.Select(x => x.POSMProductId).ToList();

            var stocks = _wStock.GetAllActive()
                .Where(x => posmIds.Contains(x.PosmProductId) && transaction.FromWarehouseId == x.WareHouseId)
                .Include(x => x.POSMProduct).ToList().MapToModel();

            await InsertWarehouseProductAvailableAmount(stocks);

            var stocksViolated = stocks.Where(st => posms.Any(posm => posm.POSMProductId == st.PosmProductId && posm.Quantity > st.AvailableQuantity)).ToList();
            var violatedProductNames = stocksViolated.Select(x => x.POSMProduct.Name).ToList();

            if (stocksViolated.Count > 0) throw new AppException($"{string.Join(",",violatedProductNames)} exceed stock available quantity");

            transaction.IsConfirmed = true;
            await _wareHouseTransaction.UpdateAsync(transaction);
            //await CreateRecieveTransaction(transaction);
            return transaction.MapToModel();
        }
        public async Task<List<WareHouseModel>> GetByCodes(List<string> codes)
        {
            var r = await _wareHouse.GetAllActive().Where(x => codes.Contains(x.Code)).ToListAsync();
            return r.ToMap<WareHouse, WareHouseModel>();
        }

        public async Task<List<WareHouseTransferModel>> GetReceivableTransfersByCurrentUser()
        {
            
            var transactions = await _wareHouseTransaction.GetAllActive().Where(x => x.IsConfirmed
                && x.TransactionStatus < TransactionStatus.Completed).Include(x=>x.FromWarehouse).ToListAsync();

            var tIds = transactions.Select(x => x.Id).ToList();
            var toCWIds = transactions.Select(x => x.ToWarehouseId).ToList();
            var toWareHouses = _wareHouse.GetAllActive().Where(x => toCWIds.Contains(x.Id)).ToList().ToMap<WareHouse, WareHouseModel>();
            var items = _wareHouseTransactionItem.GetAllActive().Where(x => tIds.Contains(x.TransactionId)).Include(x=>x.POSMProduct).ToList().MapToModel();

            var trModels = transactions.MapToModel();
            trModels.ForEach(model =>
            {
                model.Items = items.FindAll(x => x.TransactionId == model.Id);
                model.ToWarehouse= toWareHouses.Find(x => x.Id == model.ToWarehouseId);
            });
            return trModels;
        }

        public async Task<WareHouseReceivedTransferModel> RecieveTransfer(WareHouseReceivedTransferModel transaction)
        {
            var user = AppIdentity.AppUser;
            if (transaction is null) throw new AppException("Transaction not found");
            if (transaction.Items.Count == 0) throw new AppException("No products provided");
            var referredDistribution = _wareHouseTransaction.GetAllActive().FirstOrDefault(x => x.Id == transaction.SourceTransferId);
            if (referredDistribution is null) throw new AppException("Source transaction not found");
            var posmIds = transaction.Items.Select(x => x.POSMProductId).ToList();
            var stocks = _wStock.GetAllActive()
                .Where(x => x.WareHouseId == transaction.FromWarehouseId && posmIds.Contains(x.PosmProductId)).Include(x=>x.POSMProduct).ToList()
                .MapToModel();
            await InsertWarehouseProductAvailableAmount(stocks);

            var violatedPosms = stocks.Where(st => transaction.Items.Any(item =>
                    item.POSMProductId == st.PosmProductId &&
                    item.ReceivedQuantity > st.AvailableQuantity + item.Quantity))
                .ToList();

            if (violatedPosms.Any())
            {
                var posmNames = string.Join(",", violatedPosms.Select(x => x.POSMProduct.Name));
                throw new AppException($"Received quantity exceeds stock available quantity for {posmNames}");
            }

            WareHouse toWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == transaction.ToWarehouseId);
            WareHouse fromWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == transaction.FromWarehouseId);
            var newReceivedTransfer = transaction.ToMap<WareHouseReceivedTransferModel, WareHouseReceivedTransfer>();
            newReceivedTransfer.CreatedBy = user.UserId;
            newReceivedTransfer.TransactionDate = DateTime.UtcNow;
            var serial = _common.GetTransactionCount(TransactionType.CW_Receive) + 1;
            newReceivedTransfer.TransactionNumber = await _common.GetTransactionNumber(TransactionType.CW_Receive,fromWareHouse.Code, toWareHouse.Code,serial);
            await _wareHouseReceivedTransfer.CreateAsync(newReceivedTransfer);

            var items = transaction.Items.ToMap<WareHouseReceivedTransferItemModel, WareHouseReceivedTransferItem>();
            items.ForEach(x => x.TransferId = newReceivedTransfer.Id);

            await _wareHouseReceivedTransferItem.CreateListAsync(items);            

            referredDistribution.TransactionStatus = TransactionStatus.Completed;
            await _wareHouseTransaction.UpdateAsync(referredDistribution);

            newReceivedTransfer.IsConfirmed = true;
            newReceivedTransfer.TransactionStatus = TransactionStatus.Completed;
            await _wareHouseReceivedTransfer.UpdateAsync(newReceivedTransfer);

            transaction = newReceivedTransfer.MapToModel();
            transaction.Items = items.MapToModel();

            await updatePOSMStockOnRecieve(transaction);


            return transaction;
        }

        public async Task<Pagination<WareHouseReceivedTransferModel>> GetReceivedTransfers(GetWareHouseReceivedTransfers queryParams)
        {
            if (queryParams.FromDateTime> queryParams.ToDateTime) (queryParams.FromDateTime, queryParams.ToDateTime) = (queryParams.ToDateTime, queryParams.FromDateTime);
            var query = _wareHouseReceivedTransfer.GetAllActive().Where(x => x.TransactionStatus >= TransactionStatus.Completed && x.TransactionDate >= queryParams.FromDateTime && x.TransactionDate <= queryParams.ToDateTime);
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(queryParams.Search));
            }
            query = query.Include(x=>x.ToWarehouse).OrderByDescending(x => x.TransactionDate);
            var transfers = await query.ToPagedListAsync(queryParams.PageIndex, queryParams.PageSize);
            var transactionModels = transfers.ToList().MapToModel();

            var tIds = transactionModels.Select(x => x.Id).ToList();
            var sourceTransferIds = transactionModels.Select(x => x.SourceTransferId).ToList();
            var sourceWareHouseIds = transactionModels.Select(x => x.FromWarehouseId).ToList();

            var referenceDistributions = _wareHouseTransaction.GetAllActive().Where(x => sourceTransferIds.Contains(x.Id)).ToList().MapToModel();
            var sourceWareHouses = _wareHouse.GetAllActive().Where(x => sourceWareHouseIds.Contains(x.Id)).ToList().ToMap<WareHouse,WareHouseModel>();

            var receivedItems = _wareHouseReceivedTransferItem.GetAllActive().Where(x => tIds.Contains(x.TransferId)).Include(x=>x.POSMProduct).ToList();

            transactionModels.ForEach(m =>
            {
                m.SourceTransfer = referenceDistributions.Find(x => x.Id == m.SourceTransferId);
                m.FromWarehouse = sourceWareHouses.Find(x => x.Id == m.FromWarehouseId);
                m.Items = receivedItems.FindAll(x => x.TransferId == m.Id).MapToModel();
            });

            Pagination<WareHouseReceivedTransferModel> paginatedList = new Pagination<WareHouseReceivedTransferModel>(queryParams.PageIndex, queryParams.PageSize, transfers.TotalItemCount, transactionModels);

            return paginatedList;
        }

        private async Task updatePOSMStockOnRecieve(WareHouseReceivedTransferModel transactionModel)
        {
            var user = AppIdentity.AppUser;
            var posmIds = transactionModel.Items.Select(x => x.POSMProductId).ToList();
            var sourceWareHouseStocks = _wStock.GetAllActive().Where(x => x.WareHouseId == transactionModel.FromWarehouseId && posmIds.Contains(x.PosmProductId)).ToList();
            var destinationWareHouseStocks = _wStock.GetAllActive().Where(x => x.WareHouseId == transactionModel.ToWarehouseId && posmIds.Contains(x.PosmProductId)).ToList();
            var newStocks = new List<WareHouseStock>();
            foreach (var posmId in posmIds)
            {
                var item = transactionModel.Items.Find(x => x.POSMProductId == posmId);
                var srouceWStock = sourceWareHouseStocks.Find(x => x.PosmProductId == posmId);
                srouceWStock.Quantity -= item.ReceivedQuantity;
                if (srouceWStock.Quantity < 0) srouceWStock.Quantity = 0;
                var destinationWStock = destinationWareHouseStocks.Find(x => x.PosmProductId== posmId);

                if (destinationWStock is null)
                {
                    WareHouseStock newStock = new WareHouseStock()
                    {
                        CreatedBy = user.UserId,
                        PosmProductId = posmId,
                        Quantity = item.ReceivedQuantity,
                        WareHouseId = transactionModel.ToWarehouseId,
                    };
                    newStocks.Add(newStock);
                }
                else destinationWStock.Quantity += item.ReceivedQuantity;
            }
            await _wStock.UpdateListAsync(sourceWareHouseStocks);
            await _wStock.UpdateListAsync(destinationWareHouseStocks);
            if (newStocks.Any()) await _wStock.CreateListAsync(newStocks);
        }

        private static void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }

        private void Insert_WareHousePOSM_DistributionExcelRows(WareHouseDistributionData data, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;            
            workSheet.Cells[currentRowNumberForData, 1].Value = $"{data.FromWareHouse.Code}";
            workSheet.Cells[currentRowNumberForData, 2].Value = $"{data.FromWareHouse.Name}";
            workSheet.Cells[currentRowNumberForData, 3].Value = $"{data.ToWareHouse.Code}";
            workSheet.Cells[currentRowNumberForData, 4].Value = $"{data.ToWareHouse.Name}";            
        }


        public async Task InsertWarehouseProductAvailableAmount(List<WareHouseStockModel> stockData)
        {
          
            var wareHouseIds = stockData.Select(x => x.WareHouseId).Distinct().ToList();

            var distributions = await _transaction.GetAllActive().Where(x =>
                x.TransactionType == TransactionType.Distribute && x.WarehouseId != null &&  wareHouseIds.Contains(x.WarehouseId.Value) &&
                x.IsConfirmed && x.TransactionStatus != TransactionStatus.Completed).
                Include(x=>x.WDistributionTransactions).ToListAsync();

            var warehouseTransfers = _wareHouseTransaction.GetAllActive().Where(x => 
                    x.IsConfirmed  &&  wareHouseIds.Contains(x.FromWarehouseId) && 
                    x.TransactionStatus != TransactionStatus.Completed )
                    .Include(x=>x.WareHouseTransferItems).ToList();


            foreach (var item in stockData)
            {
                var bookedDistributionItemAmount = distributions.Where(x => x.WarehouseId == item.WareHouseId)
                    .SelectMany(x => x.WDistributionTransactions).Where(x => x.POSMProductId == item.PosmProductId)
                    .Select(x => x.Quantity).Sum();
                
                var warehouseTransferItemAmount = warehouseTransfers.Where(x => x.FromWarehouseId == item.WareHouseId)
                    .SelectMany(x => x.WareHouseTransferItems).Where(x => x.POSMProductId == item.PosmProductId)
                    .Select(x => x.Quantity).Sum();

                var totalBookedAmount = bookedDistributionItemAmount  + warehouseTransferItemAmount;
                item.AvailableQuantity = item.Quantity - totalBookedAmount;
                if (item.AvailableQuantity < 0) item.AvailableQuantity = 0;
            }
        }
    }
}
