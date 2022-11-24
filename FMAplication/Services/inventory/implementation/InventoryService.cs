using FMAplication.Domain.Products;
using FMAplication.Domain.WareHouse;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Models.wareHouse;
using FMAplication.Repositories;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.inventory.interfaces;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Core.Params;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;
using FMAplication.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using FMAplication.Domain.Sales;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Models.Sales;
using X.PagedList;
using FMAplication.Models.Common;
using FMAplication.Domain.Users;
using FMAplication.Exceptions;
using FMAplication.Models.Users;
using FMAplication.Exceptions;
using FMAplication.Models.SPWisePOSMLedgers;
using FMAplication.RequestModels;
using FMAplication.Services.Sales.Interfaces;
using FMAplication.Services.TransactionWorkflow;
using FMAplication.Services.Warehouses.interfaces;
using Microsoft.Extensions.Logging;

namespace FMAplication.Services.inventory.implementation
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<WareHouse> _wareHouse;
        private readonly IRepository<WareHouseStock> _warehouseStockRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<SalesPointStock> _salesPointStock;
        private readonly IRepository<StockAddTransaction> _stockAddTransaction;
        private readonly IRepository<SalesPointAdjustmentItem> _salesPointAdjustmentItem;
        private readonly ITransactionWorkflowService _transactionWorkflowService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<StockAdjustmentItems> _stockAdjustmentRepository;
        private readonly IRepository<WDistributionTransaction> _wDistributionTransaction;
        private readonly IRepository<WDistributionRecieveTransaction> _wDistributionRecieveTransaction;
        public readonly IFileService _file;
        public readonly ICommonService _commonService;
        private readonly IRepository<UserInfo> _user;
        private readonly ISPWisePosmLedgerService _spWisePosmLedgerService;
        private readonly ISalesPointService _salesPointService;
        private readonly IWareHouseService _wareHouseService;

        public InventoryService(IRepository<POSMProduct> posmProduct, IRepository<WareHouse> wareHouse, 
            IRepository<WareHouseStock> warehouseStockRepository, IRepository<SalesPoint> salesPoint,
            IRepository<Transaction> transactionRepository, IRepository<StockAdjustmentItems> stockAdjustmentRepository, 
            IFileService file, IRepository<StockAddTransaction> stockAddTransaction, ICommonService commonService,
            IRepository<WDistributionTransaction> wDistributionTransaction, IRepository<WDistributionRecieveTransaction> wDistributionRecieveTransaction,
            IRepository<SalesPointStock> salesPointStock,IRepository<UserInfo> user, 
            IRepository<SalesPointAdjustmentItem> salesPointAdjustmentItem, 
            ITransactionWorkflowService transactionWorkflowService, INotificationService notificationService, 
            ISPWisePosmLedgerService spWisePosmLedgerService, ISalesPointService salesPointService, IWareHouseService wareHouseService)
        {
            _posmProduct = posmProduct;
            _wareHouse = wareHouse;
            _warehouseStockRepository = warehouseStockRepository;
            _transactionRepository = transactionRepository;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _stockAddTransaction = stockAddTransaction;
            _file = file;
            _commonService = commonService;
            _wDistributionTransaction = wDistributionTransaction;
            _wDistributionRecieveTransaction = wDistributionRecieveTransaction;
            _salesPoint = salesPoint;
            _salesPointStock = salesPointStock;
            _user = user;
            _salesPointAdjustmentItem = salesPointAdjustmentItem;
            _transactionWorkflowService = transactionWorkflowService;
            _notificationService = notificationService;
            _spWisePosmLedgerService = spWisePosmLedgerService;
            _salesPointService = salesPointService;
            _wareHouseService = wareHouseService;
        }
        public FileData GetFileDataForStockAdd(int wareHouseId)
        {
            StockCreationData taskCreationData = GetStockCreationData(wareHouseId);

            ExcelPackage excel = new ExcelPackage();
            
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = new List<string>() { "CentralWarehouse", "POSM Code", "POSM Name", "Quantity", "Supplier" };
            SetTableStyle(workSheet, headers.Count);
            SetHeaderStyle(workSheet,headers.Count);
            InsertHeaders(headers, workSheet);
            InsertRows(taskCreationData.WareHouse, taskCreationData.PosmProducts, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void InsertRows(WareHouseModel  wareHouse, List<POSMProductModel> posmProducts, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;        
            
            for (int i = 0; i < posmProducts.Count; i++)
            {
                workSheet.Cells[currentRowNumberForData, 1].Value = $"{wareHouse.Name}({wareHouse.Code})";
                workSheet.Cells[currentRowNumberForData, 2].Value = posmProducts[i].Code;
                workSheet.Cells[currentRowNumberForData, 3].Value = posmProducts[i].Name;
                currentRowNumberForData++;
            }
        }

        private void Insert_WareHousePOSM_DistributionExcelRows(StockDistributionData data,  ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            for (int i = 0; i < data.SalesPointViewModels.Count; i++)
            {
                workSheet.Cells[currentRowNumberForData, 1].Value = $"{data.WareHouse.Code}";
                workSheet.Cells[currentRowNumberForData, 2].Value = data.SalesPointViewModels[i].Code;
                workSheet.Cells[currentRowNumberForData, 3].Value = data.SalesPointViewModels[i].Name;
                currentRowNumberForData++;
            }
        }

        private void SetTableStyle(ExcelWorksheet workSheet, int columnCount)
        {            
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
        }

        public void SetHeaderStyle(ExcelWorksheet workSheet,int headCount)
        {
            var header = workSheet.Cells[1,1,1,headCount];
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(194,194,194));
            header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        private static void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }

        private StockCreationData GetStockCreationData(int wareHouseId)
        {
            StockCreationData stockCreationData = new StockCreationData();
            
            var posmProducts = _posmProduct.GetAllActive().Where(x=>x.IsJTIProduct).ToList().ToMap<POSMProduct,POSMProductModel>();
            stockCreationData.PosmProducts = posmProducts;
            
            var centralWareHouse = _wareHouse.GetAllActive().FirstOrDefault(x => x.Id == wareHouseId);
            if (centralWareHouse is null) throw new AppException("Warehouse not found");
            stockCreationData.WareHouse = centralWareHouse.ToMap<WareHouse,WareHouseModel>();

            return stockCreationData;
        }

        public async Task<List<POSMProductStockModel>> GetProductStock(int warehouseId)
        {
            var warehouseStockData = await _warehouseStockRepository.GetAll().Where(x => x.WareHouseId == warehouseId).ToListAsync();
            var warehouseStockDataModel = warehouseStockData.MapToModel();
            await _wareHouseService.InsertWarehouseProductAvailableAmount(warehouseStockDataModel);

            var posmProducts = _posmProduct.GetAllActive().Where(x => x.IsJTIProduct ).ToList().ToMap<POSMProduct, POSMProductStockModel>();

            foreach (var product in posmProducts)
            {
                var stock = warehouseStockDataModel.FirstOrDefault(x => x.PosmProductId == product.Id);
                if (stock != null)
                {
                    product.Quantity = stock.Quantity;
                    product.AvailableQuantity = stock.AvailableQuantity;
                }
                   
            }
            return posmProducts; ;
        }

        public async Task<TransactionAdjustmentModel> GetStockAdjustmentTransaction()
        {
            var stockAdjustmentTransaction = new TransactionAdjustmentModel
            {
                TransactionNumber = "",
                TransactionDate = DateTime.UtcNow,
                StockAdjustmentItems = new List<StockAdjustmentItemsModel>()
            };
            return stockAdjustmentTransaction;
        }

        public async Task SaveStockAdjustmentTransaction(TransactionAdjustmentModel model)
        {
            var warehouse = await _wareHouse.FindAsync(x => x.Id == model.WarehouseId); 
            if (warehouse == null) throw new AppException("Warehouse not found");


            var result = await CheckProductIsPendingOnAdjustStock(model);
            if (result != "") throw new DefaultException(result);


            var transaction = new Transaction { TransactionType =  TransactionType.StockAdjustment, Remarks = model.Remarks, TransactionDate = DateTime.UtcNow, 
                IsConfirmed = model.IsConfirmed, TransactionNumber = await GetTransactionNumber(TransactionType.StockAdjustment, warehouse.Code), 
                TransactionStatus = TransactionStatus.Pending, WarehouseId = warehouse.Id};
            await _transactionRepository.CreateAsync(transaction);

            var stockAdjustmentItems = new List<StockAdjustmentItems>();
            if (model.StockAdjustmentItems.Any())
            {
                foreach (var item in model.StockAdjustmentItems)
                {
                    stockAdjustmentItems.Add(new StockAdjustmentItems { AdjustedQuantity = item.AdjustedQuantity, PosmProductId = item.PosmProductId,  SystemQuantity = item.SystemQuantity, TransactionId = transaction.Id });
                }
            }
            await _stockAdjustmentRepository.CreateListAsync(stockAdjustmentItems);
        }

        private async Task<string> CheckProductIsPendingOnAdjustStock(TransactionAdjustmentModel model)
        {
            var result = ""; 

            var transactions = await _transactionRepository.FindAllAsync(x =>
                !x.IsConfirmed && x.TransactionType == TransactionType.StockAdjustment && x.WarehouseId == model.WarehouseId &&
                x.TransactionStatus == TransactionStatus.Pending);

            var productIds = await model.StockAdjustmentItems.Select(s => s.PosmProductId).ToListAsync();

            foreach (var trans in transactions)
            {
                var lineItems = await _stockAdjustmentRepository.FindAllAsync(x =>
                    x.TransactionId == trans.Id && productIds.Contains(x.PosmProductId));
                if (lineItems.Count > 0)
                {
                    result = $"Failed to create stock adjustment, there is a pending stock adjustment on transaction number {trans.TransactionNumber}";
                    break;
                }
                          
            }
            return result;
        }

        private async Task<string> CheckProductIsPendingOnSalesPointAdjustStock(TransactionModel model)
        {
            var result = "";

            var transactions = await _transactionRepository.FindAll(x =>
                !x.IsConfirmed && x.TransactionType == TransactionType.SalesPointStockAdjustment && x.SalesPointId == model.SalesPointId &&
                x.TransactionStatus == TransactionStatus.Pending).ToListAsync();
            var tIds = transactions.Select(x => x.Id).ToList();
            var productIds = await model.SalesPointAdjustmentItems.Select(s => s.PosmProductId).ToListAsync();
            var existingItems = _salesPointAdjustmentItem.FindAll(x => tIds.Contains(x.TransactionId) && productIds.Contains(x.PosmProductId)).ToList();

            if (existingItems.Any())
            {
                var existingTransactionNumbers = transactions.FindAll(x => existingItems.Any(item => item.TransactionId == x.Id)).Select(x=>x.TransactionNumber).ToList();
                string transactionNumberStr = string.Join(",",existingTransactionNumbers);
                result = $"Failed to create stock adjustment, there is a pending stock adjustment on transaction number {transactionNumberStr}";
            }            
            return result;
        }

        public async Task UpdateStockAdjustmentTransaction(TransactionAdjustmentModel model)
        {

            var transaction = await _transactionRepository.FindAsync(x => x.Id == model.Id && x.WarehouseId == model.WarehouseId); 
            if (transaction == null) throw  new AppException("Transaction not found");

            transaction.Remarks = model.Remarks;
            await _transactionRepository.UpdateAsync(transaction);

            var stockAdjustmentItems = new List<StockAdjustmentItems>();

            var existingItems = await _stockAdjustmentRepository.Where(x => x.TransactionId == transaction.Id).ToListAsync();
            if (existingItems.Any()) await _stockAdjustmentRepository.DeleteListAsync(existingItems);

            if (model.StockAdjustmentItems.Any())
            {
                foreach (var item in model.StockAdjustmentItems)
                {
                    stockAdjustmentItems.Add(new StockAdjustmentItems { AdjustedQuantity = item.AdjustedQuantity, PosmProductId = item.PosmProductId,  SystemQuantity = item.SystemQuantity, TransactionId = transaction.Id });
                }
            }
            await _stockAdjustmentRepository.CreateListAsync(stockAdjustmentItems);
        }

        public async Task<TransactionModel> UpdateSalePointStockAdjustmentTransaction(TransactionModel model)
        {
            var transaction = _transactionRepository.Find(x => x.Id == model.Id);
            if (transaction is null) throw new AppException("Transaction not found");

            transaction.Remarks = model.Remarks;
            await _transactionRepository.UpdateAsync(transaction);

            var stockAdjustmentItems = new List<SalesPointAdjustmentItem>();

            var updatedItemIds = model.SalesPointAdjustmentItems.Select(x => x.Id).ToList();
            var existingItems = await _salesPointAdjustmentItem.Where(x => x.TransactionId == model.Id).ToListAsync();

            if (model.SalesPointAdjustmentItems.Any())
            {
                model.SalesPointAdjustmentItems.ForEach(x => x.PosmProduct = null);
                var itemsToUpdate = existingItems.FindAll(x=> updatedItemIds.Contains(x.Id));
                itemsToUpdate.ForEach(item=> item.AdjustedQuantity = model.SalesPointAdjustmentItems.Find(x => x.Id == item.Id).AdjustedQuantity);
                await _salesPointAdjustmentItem.UpdateListAsync(itemsToUpdate);
            }

            var existingItemsToDelete = existingItems.Where(x => !updatedItemIds.Contains(x.Id)).ToList();
            if (existingItemsToDelete.Any()) await _salesPointAdjustmentItem.DeleteListAsync(existingItemsToDelete);

            return model;
        }

        public async Task ApproveTransaction(Transaction transaction)
        {
            if (transaction.TransactionType == TransactionType.StockAdjustment)
                await ApproveCWStockAdjustment(transaction.Id);
            else if (transaction.TransactionType == TransactionType.SalesPointStockAdjustment)
                await ApproveSPStockAdjustment(transaction.Id);
            else if (transaction.TransactionType == TransactionType.SP_Transfer)
               await _salesPointService.ApproveAwaitingReceiveSalesPointTransfer(transaction.Id);
        }

        public List<WareHouseModel> GetWareHouses()
        {
            var wareHouses = _wareHouse.GetAll().ToList();
            return wareHouses.ToMap<WareHouse,WareHouseModel>();
        }
      
       
        public async Task<TransactionAdjustmentModel> GetStockAdjustmentTransaction(int transactionId)
        {
            var loggedInUser = AppIdentity.AppUser;

            var transaction = await _transactionRepository.GetAll().FirstOrDefaultAsync(x =>
                x.Id == transactionId && x.TransactionType == TransactionType.StockAdjustment);

            if (transaction == null) throw  new AppException("Transaction not found");
            var items = await _stockAdjustmentRepository.GetAll().Include(x=>x.PosmProduct)
                                                        .Where(x => x.TransactionId == transactionId).ToListAsync();


            var transactionModel = transaction.ToMap<Transaction, TransactionAdjustmentModel>();
            transactionModel.StockAdjustmentItems = items.ToMap<StockAdjustmentItems, StockAdjustmentItemsModel>();
            if (transaction.WarehouseId != null) transactionModel.WarehouseId = (int) transaction.WarehouseId;

            var transactionNotification =  await _notificationService.TransactionNotification(loggedInUser.UserId, transactionId);
            if (transactionNotification != null) transactionModel.TransactionNotification = transactionNotification;
            return transactionModel;
        }

        public async Task<TransactionModel> GetSalesPointStockAdjustmentTransaction(int transactionId)
        {
            var loggedInUser = AppIdentity.AppUser;

            var transaction = await _transactionRepository.FindAsync(x=>x.Id == transactionId);
            if (transaction == null) throw new AppException("Transaction not found");
            var items = _salesPointAdjustmentItem.FindAll(x => x.TransactionId == transaction.Id).Include(x => x.PosmProduct).ToList();
            var transactionAdjustment = transaction.ToMap<Transaction, TransactionModel>();
            var transactionNotification = await _notificationService.TransactionNotification(loggedInUser.UserId, transactionId);
            if (transactionNotification != null) transactionAdjustment.TransactionNotification = transactionNotification;
            transactionAdjustment.SalesPointAdjustmentItems = items.MapToModel();
            return transactionAdjustment;
        }

        public async Task<TransactionModel> CreateStockAddTransaction( CreateStockAddTransaction model)
        {
            var user = AppIdentity.AppUser;
            Transaction transaction = new Transaction();
            transaction.TransactionType = TransactionType.StockAdd;
            transaction.Remarks = model.Remarks;
            var wareHouse = _wareHouse.Find(x => x.Code == model.WareHouseCode);
            transaction.WarehouseId = wareHouse.Id;
            string transactionNumber = await GetTransactionNumber(TransactionType.StockAdd, wareHouse.Code);
            transaction.TransactionNumber = transactionNumber;
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.CreatedBy = user.UserId;
            await _transactionRepository.CreateAsync(transaction);
            List<StockAddTransactionModel> stockAddTransactionModels = new List<StockAddTransactionModel>();
            TransactionModel transactionModel = transaction.ToMap<Transaction,TransactionModel>();
            foreach (var posmProduct in model.POSM_Products)
            {
                StockAddTransaction stockAddTransaction = new StockAddTransaction();
                var posmProductInfo = _posmProduct.Find(x => x.Code == posmProduct.PosmProductCode && x.IsJTIProduct);
                stockAddTransaction.PosmProductId = posmProductInfo.Id;
                stockAddTransaction.Quantity = posmProduct.Quantity;
                stockAddTransaction.TransactionId = transaction.Id;
                stockAddTransaction.CreatedBy = user.UserId;
                stockAddTransaction.Supplier = posmProduct.Supplier;
                await _stockAddTransaction.CreateAsync(stockAddTransaction);

                StockAddTransactionModel stockAddTransactionModel = stockAddTransaction.ToMap<StockAddTransaction, StockAddTransactionModel>();
                stockAddTransactionModels.Add(stockAddTransactionModel);
            }
            transactionModel.StockAddTransactions = stockAddTransactionModels;
            return transactionModel;
        }

        public async Task<TransactionModel> UpdateTransaction(TransactionModel model)
        {
            var user = AppIdentity.AppUser;
            var transaction = _transactionRepository.Find(x => x.Id == model.Id);
            if (transaction is null) throw new AppException("Transaction not found");
            transaction.Remarks = model.Remarks;
            transaction.TransactionStatus = model.TransactionStatus;
            transaction.ChalanNumber = model.ChalanNumber;
            transaction.ModifiedBy = user.UserId;
            await _transactionRepository.UpdateAsync(transaction);
            return transaction.ToMap<Transaction, TransactionModel>();
        }

        

        public List<TransactionModel> GetTransactions(TransactionType transactionType)
        {
            var transactions = _transactionRepository.FindAll(x => x.TransactionType == transactionType).Include(x=>x.WareHouse).ToList();
            var transactionModels = transactions.ToMap<Transaction, TransactionModel>();
            foreach(var transactionModel in transactionModels)
            {
                transactionModel.WareHouseModel = transactions.Find(x => x.Id == transactionModel.Id).WareHouse.ToMap<WareHouse,WareHouseModel>();
            }
            return transactionModels;

        }        

        public async Task<TransactionModel> ConfirmStockAddTransaction(int transactionId)
        {
            var user = AppIdentity.AppUser;
            var transaction = _transactionRepository.Find(x => x.Id == transactionId);
            if (transaction is null) throw new AppException("Transaction not found");
            if (transaction.IsConfirmed) throw new AppException("Already Confirmed");
            var stockAddTransactions = _stockAddTransaction.FindAll(x => x.TransactionId == transaction.Id).Include(x=>x.PosmProduct).ToList();

            ValidateStockAddQuantities(stockAddTransactions);

            var posmProductIds = stockAddTransactions.Select(x => x.PosmProductId).ToList();
            var stocks = _warehouseStockRepository.FindAll(x => x.WareHouseId == transaction.WarehouseId && posmProductIds.Any(id => id == x.PosmProductId)).ToList();
            
            foreach(var addTr in stockAddTransactions)
            {
                var stock = stocks.Find(x => x.PosmProductId == addTr.PosmProductId);
                if (stock is null) {
                    WareHouseStock wareHouseStock = new WareHouseStock()
                    {
                        WareHouseId = (int)transaction.WarehouseId,
                        PosmProductId = addTr.PosmProductId,
                        Quantity = addTr.Quantity,
                        Status = Status.Active,
                        CreatedBy = user.UserId
                    };
                    await _warehouseStockRepository.CreateAsync(wareHouseStock);
                }
                else
                {
                    stock.Quantity += addTr.Quantity;
                    stock.ModifiedBy = user.UserId;
                    await _warehouseStockRepository.UpdateAsync(stock);
                } 
            }            

            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.Completed;
            transaction.ModifiedBy = user.UserId;

            await _transactionRepository.UpdateAsync(transaction);

            

            return transaction.ToMap<Transaction,TransactionModel>();
        }

        private void ValidateStockAddQuantities(List<StockAddTransaction> stockAddItems)
        {
            var itemsWithNegativeValues = stockAddItems.Where(x => x.Quantity < 0);
            if (itemsWithNegativeValues.Any())
            {
                var productNames = itemsWithNegativeValues.Select(x => x.PosmProduct.Name);
                var productNameStr = string.Join(", ", productNames);
                throw new AppException($"Negative quantity found for product {productNameStr}");
            }
        }

        public async Task<TransactionModel> UpdateStockAddTransaction(UpdateStockAddTransaction model)
        {
            if (model.StockAddTransactions.Count == 0) throw new AppException("No stock transactions provided");
            Transaction transaction = _transactionRepository.Find(x => x.Id == model.StockAddTransactions[0].TransactionId);
            if (transaction is null) throw new AppException("Transaction not found");
            if (model.Remarks is object) {
                transaction.Remarks = model.Remarks;
                await _transactionRepository.UpdateAsync(transaction);
            }
            var stockAddTransactions = _stockAddTransaction.FindAll(x => x.TransactionId == transaction.Id).ToList();
            var removedTransactions = stockAddTransactions.FindAll(x => !model.StockAddTransactions.Any(addTr => addTr.Id == x.Id));            
            await _stockAddTransaction.DeleteListAsync(removedTransactions);
            var editableStockTransactions = stockAddTransactions.FindAll(x => model.StockAddTransactions.Any(addTr => addTr.Id == x.Id));
            foreach(var tr in editableStockTransactions)
            {
                tr.Quantity = model.StockAddTransactions.Find(x => x.Id == tr.Id).Quantity;
            }
            await _stockAddTransaction.UpdateListAsync(editableStockTransactions);
            var trModel = transaction.ToMap<Transaction, TransactionModel>();
            trModel.StockAddTransactions = editableStockTransactions.ToMap<StockAddTransaction, StockAddTransactionModel>();
            return trModel;
        }

        public async Task<Pagination<TransactionAdjustmentModel>> GetStockAdjustTransactions(AdjustmentTransactionParams transactionParams)
        {
            var spec = new AdjustmentTransactionWithIncludesSpecification(transactionParams);
            var countSpec = new AdjustmentTransactionCountSpecification(transactionParams);

            var totalItems = await _transactionRepository.CountAsync(countSpec);
            var transactions = await _transactionRepository.ListAsync(spec);
            var data = transactions.ToMap<Transaction, TransactionAdjustmentModel>();
            var transactionIds = data.Where(x=>x.IsConfirmed && x.TransactionStatus == TransactionStatus.WaitingForApproval).Select(x => x.Id).ToList();
            var transactionWorkflows = await _transactionWorkflowService.GetTransactionWorkFlows(transactionIds);
            foreach (var transactionModel in data)
            {
                transactionModel.Lines = await _stockAdjustmentRepository.CountFuncAsync(x => x.TransactionId == transactionModel.Id);
                var result = await CalculateStockAdjustDecreaseIncrease(transactionModel);
                transactionModel.TotalDecrease = result.decrease;
                transactionModel.TotalIncrease = result.increase;
                transactionModel.WareHouseModel = transactions.Find(x => x.Id == transactionModel.Id).WareHouse.ToMap<WareHouse, WareHouseModel>();

                var transactionWorkflow = transactionWorkflows.FirstOrDefault(x => x.TransactionId == transactionModel.Id);
                if (transactionWorkflow != null) transactionModel.TransactionWorkflow = transactionWorkflow;
            }
            return new Pagination<TransactionAdjustmentModel>(transactionParams.PageIndex, transactionParams.PageSize, totalItems, data);
        }

        public async Task<Pagination<TransactionModel>> GetSalesPointStockAdjustTransactions(GetSalesPointAdjustmentTransactions model)
        {            
            if (model.FromDateTime > model.ToDateTime) (model.FromDateTime, model.ToDateTime) = (model.ToDateTime, model.FromDateTime);
            var query = _transactionRepository.FindAllInclude(x => x.TransactionType == TransactionType.SalesPointStockAdjustment && 
                x.TransactionDate >= model.FromDateTime && x.TransactionDate <= model.ToDateTime , i1 => i1.WareHouse);
            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(model.Search));
            }
            if ( model.TransactionStatus != -1) query = query.Where(x => x.TransactionStatus == (TransactionStatus) model.TransactionStatus);
            query = query.OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
            var transactionModels = transactions.ToList().MapToModel();

            var transactionIds = transactionModels.Where(x => x.IsConfirmed && x.TransactionStatus == TransactionStatus.WaitingForApproval).Select(x => x.Id).ToList();
            var transactionWorkflows = await _transactionWorkflowService.GetTransactionWorkFlows(transactionIds);

            var tids = transactionModels.Select(x => x.Id).ToList();
            var adjustedPosms = _salesPointAdjustmentItem.FindAllInclude(x => tids.Contains(x.TransactionId), i1 => i1.PosmProduct).ToList();

            transactionModels.ForEach(m =>
            {
                m.SalesPointAdjustmentItems = adjustedPosms.FindAll(x => x.TransactionId == m.Id).MapToModel();
                var transactionWorkflow = transactionWorkflows.FirstOrDefault(x => x.TransactionId == m.Id);
                if (transactionWorkflow != null) m.TransactionWorkflow = transactionWorkflow;
              
            });
            _commonService.InsertSalesPoints(transactionModels);
            Pagination<TransactionModel> paginatedList = new Pagination<TransactionModel>(model.PageIndex, model.PageSize, transactions.TotalItemCount, transactionModels);


            return paginatedList;
        }

        public async Task<TransactionAdjustmentModel> ConfirmCWStockAdjustmentTransaction(int transactionId)
        {
            var transaction = await _transactionRepository.GetAllActive().Where(x => x.Id == transactionId &&
                                                                                 x.TransactionType == TransactionType.StockAdjustment
                                                                                 && x.TransactionStatus == TransactionStatus.Pending)
                                                                                 .Include(x=>x.StockAdjustmentItems)
                                                                                 .ThenInclude(x=>x.PosmProduct).FirstOrDefaultAsync();

            if (transaction == null) throw new AppException("Transaction not found");



            await CheckWarehouseStockAvailableAmount(transaction);


            await _transactionWorkflowService.CheckValidWorkflowSetup(transaction);

            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.WaitingForApproval;
            await _transactionRepository.UpdateAsync(transaction);


            //await ApproveCWStockAdjustment(transaction.Id);

            var result = await _transactionWorkflowService.CreateTransactionWorkflow(transaction);
            if (result)
                await _transactionWorkflowService.SendNotification(transaction);

            return transaction.ToMap<Transaction, TransactionAdjustmentModel>();
        }

        public async Task CheckWarehouseStockAvailableAmount(Transaction model)
        {

            var transaction = await _transactionRepository.GetAllActive().Where(x => x.Id == model.Id &&
                                                                                     x.TransactionType == TransactionType.StockAdjustment)
                .Include(x => x.StockAdjustmentItems)
                .ThenInclude(x => x.PosmProduct).FirstOrDefaultAsync();

            var posmIds = transaction.StockAdjustmentItems.Select(x => x.PosmProductId).ToList();
            var cwStocks = _warehouseStockRepository.GetAllActive().Where(x => x.WareHouseId == transaction.WarehouseId && posmIds.Contains(x.PosmProductId)).ToList()
                .ToMap<WareHouseStock, WareHouseStockModel>();

            await _wareHouseService.InsertWarehouseProductAvailableAmount(cwStocks);
            var salesPointAdjustmentItems = transaction.StockAdjustmentItems.Where(x =>
                cwStocks.Any(st => st.PosmProductId == x.PosmProductId &&
                                   st.Quantity - x.AdjustedQuantity > st.AvailableQuantity)).ToList();
            if (salesPointAdjustmentItems.Any())
            {
                var productNames = string.Join(",", salesPointAdjustmentItems.Select(x => x.PosmProduct.Name));
                throw new AppException(
                    $"Failed to confirm. Decreased quantity exceeds available quantity for {productNames}");
            }
        }

        public async Task<TransactionModel> ConfirmSalesPointStockAdjustmentTransaction(int transactionId)
        {
            var transaction = await _transactionRepository.GetAllActive().
                Where(x => x.Id == transactionId && x.TransactionType == TransactionType.SalesPointStockAdjustment 
                && x.TransactionStatus == TransactionStatus.Pending)
                .Include(x=>x.SalesPointAdjustmentItems).ThenInclude(x=>x.PosmProduct).FirstOrDefaultAsync();

            if (transaction == null) throw new AppException("Transaction not found");
            _commonService.CheckSpAvailableQuantityViolationForSPAdjustment(transactionId);

            var transactionItems = await _salesPointAdjustmentItem.GetAllActive().Where(x => x.TransactionId == transaction.Id).ToListAsync();
            foreach (var salesPointAdjustmentItem in transactionItems)
            {
                var holdingAmount = await _commonService.GetPosmHoldingAmount(salesPointAdjustmentItem.PosmProductId,transaction.SalesPointId);
                if (holdingAmount > salesPointAdjustmentItem.AdjustedQuantity)
                {
                    var posm = await _posmProduct.FindAsync(x => x.Id == salesPointAdjustmentItem.PosmProductId && x.IsJTIProduct);
                    throw new AppException($"Failed to confirm, Posm Product {posm.Code}- {posm.Name} holding amount more than adjusted quantity");
                }
            }

            await _transactionWorkflowService.CheckValidWorkflowSetup(transaction);

            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.WaitingForApproval;
            await _transactionRepository.UpdateAsync(transaction);

           
            var result = await _transactionWorkflowService.CreateTransactionWorkflow(transaction);
            if (result)
                await _transactionWorkflowService.SendNotification(transaction);

            return transaction.ToMap<Transaction, TransactionModel>();
        }


        public async Task ApproveCWStockAdjustment(int transactionId)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(x => x.Id == transactionId && x.IsConfirmed && x.TransactionType == TransactionType.StockAdjustment); 
            if (transaction == null) throw new AppException("Transaction not found");

            var transactionItems = await _stockAdjustmentRepository.FindAllAsync(x => x.TransactionId == transaction.Id);

            foreach (var item in transactionItems)
            {
                var stockItem = await _warehouseStockRepository.FindAsync(x => x.PosmProductId == item.PosmProductId && x.WareHouseId == transaction.WarehouseId);
                if (stockItem != null)
                {
                    stockItem.Quantity = item.AdjustedQuantity;
                    await _warehouseStockRepository.UpdateAsync(stockItem);
                }
            }

            transaction.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(transaction);

        }

        public async Task ApproveSPStockAdjustment(int transactionId)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(x => x.Id == transactionId && x.IsConfirmed && x.TransactionType == TransactionType.SalesPointStockAdjustment);
            if (transaction == null) throw new AppException("Transaction not found");

            var transactionItems = await _salesPointAdjustmentItem.GetAllActive().Where(x => x.TransactionId == transaction.Id).ToListAsync();

            foreach (var item in transactionItems)
            {
                var stockItem = await _salesPointStock.FindAsync(x => x.POSMProductId == item.PosmProductId && x.SalesPointId== transaction.SalesPointId);
                if (stockItem != null)
                {
                    stockItem.Quantity = item.AdjustedQuantity;
                    await _salesPointStock.UpdateAsync(stockItem);
                }
            }

            transaction.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task SPStockRemove(List<DailyPosmTaskItems> posmTaskItemses, int salesPointId)
        {
            List<SalesPointStock> salesPointStocks = new List<SalesPointStock>();
            foreach (var item in posmTaskItemses)
            {
                var stockItem = await _salesPointStock.FindAsync(x => x.POSMProductId == item.PosmProductId && x.SalesPointId == salesPointId);
                if (stockItem != null)
                {
                    stockItem.Quantity = stockItem.Quantity - item.Quantity;
                    salesPointStocks.Add(stockItem);
                }
            }
            await _salesPointStock.UpdateListAsync(salesPointStocks);
        }


        public async Task<FileData> GetFileDataForWStockDistribution(DownloadExcelForStockDistributions payload)
        {
            StockDistributionData taskCreationData = await GetWStockDistributionData(payload.WareHouseId,payload.SalesPointIds);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = GetWStockDistributionExcelHeaders(taskCreationData);
            
            SetTableStyle(workSheet, headers.Count);
            SetHeaderStyle(workSheet, headers.Count);
            InsertHeaders(headers, workSheet);
            Insert_WareHousePOSM_DistributionExcelRows(taskCreationData,workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        public async Task<List<TransactionModel>> CreateWPOSM_DistributionTransaction(CreateWPOSM_DistributionTransaction model)
        {
            var user = AppIdentity.AppUser;
            var wareHouseCode = model.WareHouseCode;
            WareHouse wareHouse = _wareHouse.Find(x => x.Code == wareHouseCode);
            if (wareHouse is null) throw new AppException("Warehouse not found");
            if (model.WDistributionTransactionProducts.Count == 0) throw new AppException("No distribution data provided");
            List<string> salesPointIds = model.WDistributionTransactionProducts.Select(x => x.SalesPointCode).Distinct().ToList();
            List<SalesPoint> salesPoints = _salesPoint.FindAll(x => salesPointIds.Any(id => id == x.Code)).ToList();
            List<Transaction> transactions = await CreateWDistributionTransactionList(model, wareHouse,salesPoints);
            
            var posmNames = model.WDistributionTransactionProducts.Select(x => x.POSM_Name).ToList();
            var posmProducts = _posmProduct.GetAllActive().Where(x => x.IsJTIProduct && posmNames.Any(name => name == x.Name)).ToList();
            //var posmIds = posmProducts.Select(x => x.Id).ToList();
            //var stocks = _warehouseStockRepository.FindAll(x => posmIds.Contains(x.PosmProductId)).ToList();

            await _transactionRepository.CreateListAsync(transactions);

            List<WDistributionTransaction> wDistributionTransactions = new List<WDistributionTransaction>();


            foreach (var tr in transactions)
            {
                var salesPoint = _salesPoint.Find(x => x.SalesPointId == tr.SalesPointId);
                var WDistributionTransactionProductList = model.WDistributionTransactionProducts.FindAll(x => x.SalesPointCode == salesPoint.Code).ToList();
                foreach(var l in WDistributionTransactionProductList)
                {
                    var posmProduct = posmProducts.Find(x => x.Name == l.POSM_Name);
                    if (posmProduct is null) continue;
                    WDistributionTransaction wDistributionTransaction = new WDistributionTransaction()
                    {
                        CreatedBy = user.UserId,
                        POSMProductId = posmProduct.Id,
                        Quantity = l.Quantity,
                        TransactionId = tr.Id
                    };
                    wDistributionTransactions.Add(wDistributionTransaction);
                }                
            }

            await _wDistributionTransaction.CreateListAsync(wDistributionTransactions);
            
            return transactions.ToMap<Transaction,TransactionModel>();
        }

        public async Task<TransactionModel> ConfirmWDistributionTransaction(int transactionId)
        {
            Transaction transaction = _transactionRepository.Find(x => x.Id == transactionId);
            if (transaction is null) throw new AppException("Transaction not found");

            var posms = _wDistributionTransaction.GetAll().Where(x => x.TransactionId == transactionId)
                .Include(x => x.POSMProduct).ToList();
            var posmIds = posms.Select(x => x.POSMProductId).ToList();

            var stocks = _warehouseStockRepository.FindAllInclude(x => x.WareHouseId == transaction.WarehouseId && posmIds.Contains(x.PosmProductId),i1=>i1.POSMProduct).ToList().MapToModel();
            await _wareHouseService.InsertWarehouseProductAvailableAmount(stocks);

            var stocksViolated = posms.Where(p =>
                stocks.Any(st => p.POSMProductId == st.PosmProductId && p.Quantity > st.AvailableQuantity)).ToList();

            if (stocksViolated.Count > 0) throw new AppException($"{string.Join(",", stocksViolated.Select(x=>x.POSMProduct.Name))} exceed stock available quantity");

            transaction.IsConfirmed = true;
            await _transactionRepository.UpdateAsync(transaction);
            return transaction.ToMap<Transaction,TransactionModel>();
        }        

        public async Task<TransactionModel> ReceivePOSMStock(TransactionModel transactionModel)
        {
            var user = AppIdentity.AppUser;
            if (transactionModel is null) throw new AppException("Transaction not found");
            if (transactionModel.WDistributionRecieveTransactions.Count == 0) throw new AppException("No products provided");
            var referredDistribution = _transactionRepository.Find(x => x.Id == transactionModel.ReferenceTransactionId);
            if (referredDistribution is null) throw new AppException("Referred Distribution not found");
            WareHouse wareHouse = _wareHouse.Find(x => x.Id == transactionModel.WarehouseId);
            SalesPoint salesPoint = _salesPoint.Find(x => x.SalesPointId == transactionModel.SalesPointId);
            Transaction newTransaction = new Transaction()
            {
                CreatedBy = user.UserId,
                ReferenceTransactionId = transactionModel.ReferenceTransactionId,
                Remarks = transactionModel.Remarks,
                SalesPointId = transactionModel.SalesPointId,
                WarehouseId = transactionModel.WarehouseId,
                TransactionDate = DateTime.UtcNow,
                TransactionNumber = await GetTransactionNumber(TransactionType.Receive, wareHouse.Code, salesPoint.Code),
                TransactionType = TransactionType.Receive
            };
            await _transactionRepository.CreateAsync(newTransaction);

            List<WDistributionRecieveTransaction> wDistributionRecieveTransactions = new List<WDistributionRecieveTransaction>();
            foreach(var wDistributionRecieveTransaction in transactionModel.WDistributionRecieveTransactions)
            {
                WDistributionRecieveTransaction newWDistributionRecieveTransaction = new WDistributionRecieveTransaction()
                {
                    POSMProductId = wDistributionRecieveTransaction.POSMProductId,
                    Quantity = wDistributionRecieveTransaction.Quantity,
                    RecievedQuantity = wDistributionRecieveTransaction.RecievedQuantity,
                    TransactionId = newTransaction.Id
                };
                wDistributionRecieveTransactions.Add(newWDistributionRecieveTransaction);
            }
            await _wDistributionRecieveTransaction.CreateListAsync(wDistributionRecieveTransactions);


            await updatePOSMStockOnRecieve(transactionModel);
            var receivedPosms = transactionModel.WDistributionRecieveTransactions.Select(x => new ReceivedPOSM()
            {
                Quantity = x.RecievedQuantity,
                POSMProductId = x.POSMProductId
            }).ToList();
            await _spWisePosmLedgerService.SPWisePOSMLedgerReceivedStock(receivedPosms,transactionModel.SalesPointId);

            referredDistribution.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(referredDistribution);

            newTransaction.IsConfirmed = true;
            newTransaction.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(newTransaction);

            transactionModel = newTransaction.MapToModel();
            transactionModel.WDistributionRecieveTransactions = wDistributionRecieveTransactions.MapToModel();

            return transactionModel;
        }

        public async Task<List<TransactionModel>> GetReceivablePOSMDistributionByCurrentUser()
        {
            var salesPoints = _commonService.GetSalesPointsByFMUser(AppIdentity.AppUser.UserId);
            var salesPoinIds = salesPoints.Select(x => x.Id).ToList();
            var transactions = _transactionRepository.FindAllInclude(x => x.TransactionType == TransactionType.Distribute && x.IsConfirmed
                && x.TransactionStatus< TransactionStatus.Completed && x.SalesPointId != null
                && salesPoinIds.Contains((int)x.SalesPointId), i1=>i1.WareHouse).ToList();

            var tIds = transactions.Select(x => x.Id).ToList();
            var posms = _wDistributionTransaction.FindAllInclude(x => tIds.Contains(x.TransactionId), i1 => i1.POSMProduct).ToList().MapToModel();

            var createdByIds = transactions.Select(x => x.CreatedBy).ToList();
            var createdByUsers = (await _user.FindAllAsync(x => createdByIds.Contains(x.Id))).ToList().ToMap<UserInfo,UserInfoModel>();

            var trModels = transactions.MapToModel();
            trModels.ForEach(model =>
            {
                model.WDistributionTransactions = posms.FindAll(x => x.TransactionId == model.Id);
                model.CreatedByUser = createdByUsers.Find(x => x.Id == model.CreatedBy);
            });
            _commonService.InsertSalesPoints(trModels);
            return await Task.FromResult(trModels);
        }

        private async Task updatePOSMStockOnRecieve(TransactionModel transactionModel)
        {
            var user = AppIdentity.AppUser;
            var posmIds = transactionModel.WDistributionRecieveTransactions.Select(x => x.POSMProductId).ToList();
            var wareHouseStocks = _warehouseStockRepository.FindAll(x => x.WareHouseId == transactionModel.WarehouseId && posmIds.Contains(x.PosmProductId)).ToList();
            var salesPointStocks = _salesPointStock.FindAll(x => x.SalesPointId == transactionModel.SalesPointId && posmIds.Contains(x.POSMProductId)).ToList();
            var newStocks = new List<SalesPointStock>();
            foreach(var posmId in posmIds)
            {
                var wDist = transactionModel.WDistributionRecieveTransactions.Find(x => x.POSMProductId == posmId);
                var wStock = wareHouseStocks.Find(x => x.PosmProductId == posmId);
                wStock.Quantity -= wDist.RecievedQuantity;
                if (wStock.Quantity < 0) wStock.Quantity = 0;
                var sStock = salesPointStocks.Find(x => x.POSMProductId == posmId);
                if(sStock is null)
                {
                    SalesPointStock newStock = new SalesPointStock()
                    {
                        CreatedBy = user.UserId,
                        POSMProductId = posmId,
                        Quantity = wDist.RecievedQuantity,
                        SalesPointId = (int)transactionModel.SalesPointId,
                    };
                    newStocks.Add(newStock);
                }                
                else sStock.Quantity += wDist.RecievedQuantity;
            }
            await _warehouseStockRepository.UpdateListAsync(wareHouseStocks);
            if (salesPointStocks.Count > 0) await _salesPointStock.UpdateListAsync(salesPointStocks);
            if (newStocks.Count > 0) await _salesPointStock.CreateListAsync(newStocks);
        }

        public async Task<Pagination<TransactionModel>> GetStockDistributionTransactions(int pageSize, int pageIndex,
            DateTime fromDateTime, DateTime toDateTime, string search)
        {

            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _transactionRepository.FindAll(x => x.TransactionType == TransactionType.Distribute && x.TransactionDate >= fromDateTime && x.TransactionDate <= toDateTime);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var wareHouseIdsOfSearch = _wareHouse.Where(x => x.Name.Contains(search)).Select(x => x.Id).ToList();
                var spIdsOfSearch = _salesPoint.Where(x => x.Name.Contains(search)).Select(x=>x.SalesPointId).ToList();
                query = query.Where(x =>
                    x.TransactionNumber.Contains(search) || (x.WarehouseId != null && wareHouseIdsOfSearch.Contains(x.WarehouseId.Value)) ||
                    spIdsOfSearch.Contains(x.SalesPointId));
            }

            query = query.OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(pageIndex, pageSize);
            var transactionModels = transactions.ToList().ToMap<Transaction, TransactionModel>();
            var tIds = transactions.Select(x => x.Id).ToList();

            var wareHouseIds = transactions.Select(x => x.WarehouseId).ToList();
            var wareHouseModels = _wareHouse.FindAll(x => wareHouseIds.Any(id => id == x.Id)).ToList().ToMap<WareHouse,WareHouseModel>();

            var salesPointIds = transactions.Select(x => x.SalesPointId).ToList();
            var salesPointModels = _salesPoint.FindAll(x => salesPointIds.Any(id => id == x.SalesPointId)).ToList().ToMap<SalesPoint,SalesPointModel>();
            

            var wDistributionTransactions =_wDistributionTransaction.FindAll(x => tIds.Any(id => id == x.TransactionId)).ToList();
            var wDistributionTransactionModels = wDistributionTransactions.ToMap<WDistributionTransaction, WDistributionTransactionModel>();
            var posmProductIds = wDistributionTransactions.Select(x => x.POSMProductId).ToList();
            var posmProducts = _posmProduct.GetAllActive().Where(x => posmProductIds.Any(id => id == x.Id) && x.IsJTIProduct).ToList();
            var posmProductModels = posmProducts.ToMap<POSMProduct, POSMProductModel>();
            foreach(var trModel in transactionModels)
            {
                trModel.WareHouseModel = wareHouseModels.Find(wh => wh.Id == trModel.WarehouseId);
                trModel.WDistributionTransactions = wDistributionTransactionModels.FindAll(x => x.TransactionId == trModel.Id);
                trModel.SalesPoint = salesPointModels.Find(x => x.SalesPointId == trModel.SalesPointId);
                trModel.WDistributionTransactions.ForEach(wd =>
                {
                    wd.POSMProductModel = posmProductModels.Find(x => x.Id == wd.POSMProductId);
                });
            }

            

            var result = new Pagination<TransactionModel>(pageIndex, pageSize, transactions.TotalItemCount, transactionModels);
            return result;
        }

        public async Task<Pagination<TransactionModel>> GetReceivedTransactions(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _transactionRepository.FindAllInclude(x => x.TransactionType == TransactionType.Receive && x.TransactionStatus >= TransactionStatus.Completed && x.TransactionDate >= fromDateTime && x.TransactionDate <= toDateTime,i1=>i1.WareHouse);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(search));
            }
            query = query.OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(pageIndex, pageSize);
            var transactionModels = transactions.ToList().MapToModel();

            var tIds = transactionModels.Select(x => x.Id).ToList();
            var referenceIds = transactionModels.Select(x => x.ReferenceTransactionId).ToList();

            var referenceDistributions = _transactionRepository.FindAll(x => referenceIds.Contains(x.Id)).ToList().MapToModel();

            var wDistributionTransactions = _wDistributionRecieveTransaction.FindAllInclude(x => tIds.Contains(x.TransactionId), i1 => i1.POSMProduct).ToList();

            transactionModels.ForEach(m =>
            {
                m.ReferenceTransaction = referenceDistributions.Find(x => x.Id == m.ReferenceTransactionId);
                m.WDistributionRecieveTransactions = wDistributionTransactions.FindAll(x=>x.TransactionId == m.Id).MapToModel();
            });
            _commonService.InsertSalesPoints(transactionModels);
            Pagination<TransactionModel> paginatedList = new Pagination<TransactionModel>(pageIndex,pageSize,transactions.TotalItemCount,transactionModels);

            return paginatedList;
        }

        public async Task<Pagination<TransactionModel>> GetStockAddTransactions(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _transactionRepository.FindAllInclude(x => x.TransactionType == TransactionType.StockAdd && x.TransactionDate >= fromDateTime && x.TransactionDate <= toDateTime, i1 => i1.WareHouse);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(search));
            }
            query = query.OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(pageIndex, pageSize);
            var transactionModels = transactions.ToList().MapToModel();
            var tids = transactionModels.Select(x => x.Id).ToList();
            var addedPosms = _stockAddTransaction.FindAllInclude(x => tids.Contains(x.TransactionId), i1 => i1.PosmProduct).ToList();

            transactionModels.ForEach(m =>
            {
                m.StockAddTransactions = addedPosms.FindAll(x => x.TransactionId == m.Id).MapToModel();
            });
            _commonService.InsertSalesPoints(transactionModels);
            Pagination<TransactionModel> paginatedList = new Pagination<TransactionModel>(pageIndex, pageSize, transactions.TotalItemCount, transactionModels);

            return paginatedList;
        }


        public async Task<int> GetStockDistributionTotalTransactionsCount()
        {
            var count = _transactionRepository.FindAll(x=>x.TransactionType == TransactionType.Distribute).Count();
            return await Task.FromResult(count);
        }

        public async Task<TransactionModel> ConfirmRecievedPOSM(int transactionId)
        {
            var transaction = await _transactionRepository.FindIncludeAsync(x => x.Id == transactionId,x=>x.WareHouse);
            if(transaction is null) throw new AppException("Transaction not found");
            if (transaction.IsConfirmed) throw new AppException("Already confirmed");
            var distTransaction = await _transactionRepository.FindAsync(x => x.Id == transaction.ReferenceTransactionId);
            if (distTransaction is null) throw new AppException("Reference transaction not found");
            var trModel = transaction.MapToModel();
            var distributedPosms = await _wDistributionRecieveTransaction.FindAllAsync(x => x.TransactionId == transactionId);
            trModel.WDistributionRecieveTransactions = distributedPosms.ToMap<WDistributionRecieveTransaction, WDistributionRecieveTransactionModel>();
            await updatePOSMStockOnRecieve(trModel);
            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(transaction);
            distTransaction.TransactionStatus = TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(distTransaction);
            return trModel;            
        }

        public async Task SaveSalesPointStockAdjustmentTransaction(TransactionModel model)
        {           
            var salesPoint = await _salesPoint.FindAsync(x => x.SalesPointId == model.SalesPointId);
            if (salesPoint == null) throw new AppException("Salespoint not found");
            if (!model.SalesPointAdjustmentItems.Any()) throw new AppException("No products provided");

            foreach (var salesPointAdjustmentItem in model.SalesPointAdjustmentItems)
            {
                var holdingAmount = await _commonService.GetPosmHoldingAmount(salesPointAdjustmentItem.PosmProductId, salesPoint.SalesPointId);
                if (holdingAmount > salesPointAdjustmentItem.AdjustedQuantity)
                {
                    var posm = await _posmProduct.FindAsync(x => x.Id == salesPointAdjustmentItem.PosmProductId && x.IsJTIProduct);
                    throw new AppException($"Failed to confirm, Posm Product {posm.Code}- {posm.Name} holding amount more than adjusted quantity");
                }
            }

            var result = await CheckProductIsPendingOnSalesPointAdjustStock(model);
            if (!string.IsNullOrWhiteSpace(result)) throw new DefaultException(result);
            var user = AppIdentity.AppUser;
            var transaction = new Transaction
            {
                TransactionType = TransactionType.SalesPointStockAdjustment,
                Remarks = model.Remarks,
                TransactionDate = DateTime.UtcNow,
                IsConfirmed = model.IsConfirmed,
                TransactionNumber = await GetTransactionNumber(TransactionType.SalesPointStockAdjustment, salesPoint.Code),
                TransactionStatus = TransactionStatus.Pending,
                SalesPointId = salesPoint.Id,
                CreatedBy=user.UserId,
            };
            await _transactionRepository.CreateAsync(transaction);

            var stockAdjustmentItems = new List<SalesPointAdjustmentItem>();
            model.SalesPointAdjustmentItems.ForEach(x => x.PosmProduct = null);
            foreach (var item in model.SalesPointAdjustmentItems)
            {
                SalesPointAdjustmentItem newItem = item.ToMap<SalesPointAdjustmentItemModel, SalesPointAdjustmentItem>();
                newItem.TransactionId = transaction.Id;
                stockAdjustmentItems.Add(newItem);
            }
            await _salesPointAdjustmentItem.CreateListAsync(stockAdjustmentItems);
        }

      
        #region private Methods 

        private async Task<List<Transaction>> CreateWDistributionTransactionList(CreateWPOSM_DistributionTransaction model, WareHouse wareHouse, List<SalesPoint> salesPoints)
        {
            var user = AppIdentity.AppUser;
            List<Transaction> transactions = new List<Transaction>();
            //List<string> salesPointIds = model.WDistributionTransactionProducts.Select(x => x.SalesPointCode).ToList();
            var serialNumber = _commonService.GetTransactionCount(TransactionType.Distribute);
            foreach (var salesPoint in salesPoints)
            {
                serialNumber++;
                Transaction transaction = new Transaction() { 
                    WarehouseId=wareHouse.Id,
                    SalesPointId = salesPoint.SalesPointId,
                    TransactionType = TransactionType.Distribute,
                    CreatedBy= user.UserId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionNumber = await GetTransactionNumber(TransactionType.Distribute,wareHouse.Code,salesPoint.Code,serialNumber)
                };
                transactions.Add(transaction);
            }            
            return transactions;
        }

        private List<string> GetWStockDistributionExcelHeaders(StockDistributionData taskCreationData)
        {
            var headers = new List<string>() { "Central Warehouse Code", "Sales Point Code", "SalesPointName" };
            var posmProductsNames = taskCreationData.PosmProducts.Select(x => x.Name).ToList();
            headers.AddRange(posmProductsNames);
            return headers;
        }

        private async Task<StockDistributionData> GetWStockDistributionData(int wareHouseId,List<int> salesPointIds)
        {
            StockDistributionData stockDistributionData = new StockDistributionData();

            stockDistributionData.WareHouse = _wareHouse.Find(x => x.Id == wareHouseId).ToMap<WareHouse, WareHouseModel>();
            var user = AppIdentity.AppUser;

            List<SalesPoint> salesPoints = _salesPoint.FindAll(x => salesPointIds.Any(id => id == x.SalesPointId)).ToList();

            stockDistributionData.SalesPointViewModels = salesPoints.ToMap<SalesPoint, SalesPointModel>();
            var warehouseStocks = _warehouseStockRepository.GetAllActive().Where(x => x.WareHouseId == wareHouseId).ToList();
            var warehouseStockDataModel = warehouseStocks.MapToModel();

            await _wareHouseService.InsertWarehouseProductAvailableAmount(warehouseStockDataModel);
            var availablePosmIds = warehouseStockDataModel.Where(x => x.AvailableQuantity > 0).Select(x => x.PosmProductId).ToList();

            var posmProducts = _posmProduct.GetAllActive().Where(x => x.IsJTIProduct && availablePosmIds.Contains(x.Id)).ToList().ToMap<POSMProduct, POSMProductModel>();

            stockDistributionData.PosmProducts = posmProducts;

            return stockDistributionData;
        }
        private async Task<string> GetTransactionNumber(TransactionType type, string warehouseCode)
        {
            int id = 0;
            var preFix = GetPrefixForTransactionType(type);
            var data = await _transactionRepository.GetAll().Where(x => x.TransactionType == type && x.TransactionDate.Date == DateTime.UtcNow.Date).ToListAsync();
            id = data.OrderBy(x => x).Count() + 1;
            return $"{preFix}-{warehouseCode}-{DateTime.UtcNow.ToBangladeshTime():yyyyMMdd}-{id.ToString("D4")}";
        }

        private async Task<string> GetTransactionNumber(TransactionType type, string warehouseCode,string salesPointCode,int serial=-1)
        {
            var preFix = GetPrefixForTransactionType(type);
            if (serial == -1) {
                var data = await _transactionRepository.GetAll().Where(x => x.TransactionType == type && x.TransactionDate.Date == DateTime.UtcNow.Date).ToListAsync();
                serial = data.Count() + 1;
            }
            return $"{preFix}-{warehouseCode}-{salesPointCode}-{DateTime.UtcNow.ToBangladeshTime():yyyyMMdd}-{serial.ToString("D4")}";
        }

        private string GetPrefixForTransactionType(TransactionType type)
        {
            var result = "";
            switch (type)
            {
                case TransactionType.StockAdd:
                    result = "SA";
                    break;
                case TransactionType.StockAdjustment:
                    result = "SAD";
                    break;
                case TransactionType.Distribute:
                    result = "DS";
                    break;
                case TransactionType.Receive:
                    result = "RS";
                    break;
                case TransactionType.SalesPointStockAdjustment:
                    result = "SSAD";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        private async Task<(int decrease, int increase)> CalculateStockAdjustDecreaseIncrease(TransactionAdjustmentModel transaction)
        {
            var increase = 0;
            var decrease = 0;
            var items = await _stockAdjustmentRepository.FindAllAsync(x=>x.TransactionId == transaction.Id);
            if (!items.Any()) return (increase, decrease);
            {
                foreach (var item in items)
                {
                    if (item != null && item.SystemQuantity > item.AdjustedQuantity)
                    {
                        decrease += (item.SystemQuantity - item.AdjustedQuantity);
                    }

                    else if (item != null && item.SystemQuantity < item.AdjustedQuantity)
                    {
                        increase += (item.AdjustedQuantity - item.SystemQuantity);
                    }
                }
            }
            return (decrease,increase);
        }     

        #endregion
    }
}
