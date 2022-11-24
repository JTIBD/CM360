using FMAplication.common;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Sales;
using FMAplication.Repositories;
using FMAplication.Services.Sales.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Transaction;
using FMAplication.Models.Common;
using FMAplication.Models.Products;
using FMAplication.Models.SPWisePOSMLedgers;
using FMAplication.Models.Transaction;
using FMAplication.RequestModels.Sales;
using FMAplication.RequestModels.WareHouses;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.TransactionWorkflow;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FMAplication.Services.Sales.Implementation
{
    public class SalesPointService : ISalesPointService
    {

        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<SalesPointStock> _salesPointStock;
        private readonly IRepository<SalesPointTransfer> _salesPointTransfer;
        private readonly IRepository<SalesPointTransferItem> _salesPointTransferItem;
        private readonly IRepository<SalesPointReceivedTransfer> _salesPointReceivedTransfer;
        private readonly IRepository<SalesPointReceivedTransferItem> _salesPointReceivedTransferItem;
        private readonly IRepository<SPWisePOSMLedger> _posmLedger;
        private ICommonService _common;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly  IFileService _file;
        private readonly ISPWisePosmLedgerService _spWisePosmLedgerService;
        private readonly ITransactionWorkflowService _transactionWorkflow;
        private readonly INotificationService _notificationService;

        public SalesPointService(IRepository<SalesPoint> salesPoint, IRepository<SalesPointStock> salesPointStock,
            IRepository<SalesPointTransfer> salesPointTransfer, IRepository<SalesPointTransferItem> salesPointTransferItem, 
            IRepository<SalesPointReceivedTransfer> salesPointReceivedTransfer, IRepository<SalesPointReceivedTransferItem> salesPointReceivedTransferItem, 
            IRepository<SPWisePOSMLedger> posmLedger,
            ICommonService common, IRepository<POSMProduct> posmProduct, IFileService file, ISPWisePosmLedgerService spWisePosmLedger, 
            ITransactionWorkflowService transactionWorkflow, INotificationService notificationService)
        {
            _salesPoint = salesPoint;
            _salesPointStock = salesPointStock;
            _salesPointTransfer = salesPointTransfer;
            _salesPointTransferItem = salesPointTransferItem;
            _salesPointReceivedTransfer = salesPointReceivedTransfer;
            _salesPointReceivedTransferItem = salesPointReceivedTransferItem;
            _posmLedger = posmLedger;
            _common = common;
            _posmProduct = posmProduct;
            _file = file;
            _spWisePosmLedgerService = spWisePosmLedger;
            _transactionWorkflow = transactionWorkflow;
            _notificationService = notificationService;
        }

        public Task<SalesPointModel> CreateAsync(SalesPointModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SalesPointModel>> GetQuerySalesPointAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SalesPointModel> GetSalesPointAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SalesPointModel>> GetAllSalesPointAsync()
        {
            var result = await _salesPoint.GetAllAsync();
            return result.ToMap<SalesPoint,SalesPointModel>();
        }

        public Task<SalesPointModel> SaveAsync(SalesPointModel model)
        {
            throw new NotImplementedException();
        }

        public Task<SalesPointModel> UpdateAsync(SalesPointModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<SalesPointStockModel>> GetStocks(GetSalesPointStockModel payload)
        {
            var query = _salesPointStock.GetAllActive().Where(x => x.POSMProduct.Status == Status.Active && x.POSMProduct.IsJTIProduct);

            if (!string.IsNullOrWhiteSpace(payload.Search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Code.Contains(payload.Search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId)
                                         || x.POSMProduct.Name.Contains(payload.Search)
                                         || x.POSMProduct.Code.Contains(payload.Search));
            }

            query = query.Where(x => payload.SalesPointIds.Contains(x.SalesPointId));
            query = query.Include(x => x.POSMProduct);
            query = query.OrderByDescending(x => x.CreatedTime);
            var stocks = await query.ToPagedListAsync(payload.PageIndex, payload.PageSize);
            var stockModels = stocks.ToList().MapToModel();
            _common.InsertSalesPoints(stockModels);
            _common.InsertAvalableSpQuantity(stockModels);

            Pagination<SalesPointStockModel> paginatedList = new Pagination<SalesPointStockModel>(payload.PageIndex, payload.PageSize, stocks.TotalItemCount, stockModels);

            return paginatedList;
        }

        public async Task<List<SalesPointTransferModel>> AddSalesPointTransfers(List<SalesPointTransferModel> model)
        {
            if (!model.Any()) throw new AppException("No Data provided");
            var user = AppIdentity.AppUser;
            var newEntries = new List<SalesPointTransfer>();
            var newChildEntries = new List<SalesPointTransferItem>();

            var sourceSPIds = model.Select(x => x.FromSalesPointId).ToList();
            var destinationSPIds = model.Select(x => x.ToSalesPointId).ToList();
            var salesPoints = _salesPoint.GetAllActive().Where(x => sourceSPIds.Contains(x.SalesPointId) || destinationSPIds.Contains(x.SalesPointId)).ToList();
            var sourceSps = salesPoints.Where(x => sourceSPIds.Contains(x.SalesPointId)).ToList();
            var destinationSps = salesPoints.Where(x => destinationSPIds.Contains(x.SalesPointId)).ToList();
            var serialNumber = _common.GetTransactionCount(TransactionType.SP_Transfer);

            foreach (var item in model)
            {
                serialNumber++;
                var sourceSp = sourceSps.Find(x => x.SalesPointId == item.FromSalesPointId);
                var destinationSp = destinationSps.Find(x => x.SalesPointId == item.ToSalesPointId);
                if (!item.Items.Any())
                    throw new AppException($"No items provided for source salespoint {sourceSp.Name}");
                var entry = item.ToMap<SalesPointTransferModel, SalesPointTransfer>();
                entry.CreatedBy = user.UserId;
                entry.TransactionDate = DateTime.UtcNow;
                entry.TransactionNumber =  await _common.GetTransactionNumber(TransactionType.SP_Transfer, sourceSp.Code,destinationSp.Code,serialNumber);
                newEntries.Add(entry);
            }

            await _salesPointTransfer.CreateListAsync(newEntries);
            // ReSharper disable once PossibleNullReferenceException
            model.ForEach(m=>m.Id = newEntries.Find(x=> x.ToSalesPointId == m.ToSalesPointId).Id);
            foreach (var item in model)
            {
                item.Items.ForEach(x => x.TransferId = item.Id);
                var newItems = item.Items
                    .ToMap<SalesPointTransferItemModel, SalesPointTransferItem>();
                newChildEntries.AddRange(newItems);
            }

            await _salesPointTransferItem.CreateListAsync(newChildEntries);
            return model;
        }

        public async Task<Pagination<SalesPointTransferModel>> GetSalesPointTransactions(GetSalesPointTransactions data)
        {
            if (data.FromDateTime > data.ToDateTime) (data.FromDateTime, data.ToDateTime) = (data.ToDateTime, data.FromDateTime);
            var query = _salesPointTransfer.GetAllActive().Where(x => x.TransactionDate >= data.FromDateTime && x.TransactionDate <= data.ToDateTime);
            if (data.TransactionStatus != -1)
            {
                query = query.Where(x => x.TransactionStatus == (TransactionStatus) data.TransactionStatus);
            }
            if (!string.IsNullOrWhiteSpace(data.Search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(data.Search));
            }
            query = query.OrderByDescending(x => x.TransactionDate);
            var transactions = await query.ToPagedListAsync(data.PageIndex, data.PageSize);
            
            var transactionModels = transactions.ToList().MapToModel();
            var tIds = transactions.Select(x => x.Id).ToList();
            var transactionIds = transactions.Where(x => x.IsConfirmed && x.TransactionStatus == TransactionStatus.WaitingForApproval).Select(x => x.Id).ToList();
            var transactionWorkflows = await _transactionWorkflow.GetTransactionWorkFlows(transactionIds);
            var toSalesPointIds = transactions.Select(x => x.ToSalesPointId).ToList();
            var toSalesPoints = _salesPoint.GetAllActive().Where(x => toSalesPointIds.Contains(x.Id)).ToList().ToMap<SalesPoint, SalesPointModel>();

            var items = _salesPointTransferItem.GetAllActive().Where(x => tIds.Contains(x.TransferId)).Include(x => x.POSMProduct).ToList();
            transactionModels.ForEach(x => {
                x.Items = items.FindAll(i => i.TransferId == x.Id).MapToModel();
                var transactionWorkflow = transactionWorkflows.FirstOrDefault(t => t.TransactionId == x.Id);
                if (transactionWorkflow != null) x.TransactionWorkflow = transactionWorkflow;
            });
            InsertSalesPointIntoTransfer(transactionModels);
            var result = new Pagination<SalesPointTransferModel>(data.PageIndex, data.PageSize, transactions.TotalItemCount, transactionModels);
            return result;
        }

        public async Task<SalesPointTransferModel> ConfirmSalesPointTransfer(int transactionId)
        {
            var transaction = _salesPointTransfer.GetAllActive().FirstOrDefault(x => x.Id == transactionId);
            if (transaction is null) throw new AppException("Transaction not found");

            var posms = _salesPointTransferItem.GetAllActive().Where(x => x.TransferId == transactionId).Include(x => x.POSMProduct).ToList();
            var posmIds = posms.Select(x => x.POSMProductId).ToList();

            var stocks = _salesPointStock.GetAllActive()
                .Where(x => x.SalesPointId == transaction.FromSalesPointId && posmIds.Contains(x.POSMProductId))
                .Include(x => x.POSMProduct).ToList().MapToModel();

            _common.InsertAvalableSpQuantity(stocks);

            var stocksViolated = stocks.Where(st => posms.Any(posm => posm.POSMProductId == st.POSMProductId && posm.Quantity > st.AvailableQuantity)).ToList();
            var violatedProductNames = stocksViolated.Select(x => x.POSMProduct.Name).ToList();

            if (stocksViolated.Count > 0) throw new AppException($"{string.Join(",", violatedProductNames)} exceed stock available");


            var actualTransaction = transaction.ToMap<SalesPointTransfer, Transaction>();
            actualTransaction.TransactionType = TransactionType.SP_Transfer;
            actualTransaction.SalesPointId = transaction.FromSalesPointId;

            await _transactionWorkflow.CheckValidWorkflowSetup(actualTransaction);

            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.WaitingForApproval;
            await _salesPointTransfer.UpdateAsync(transaction);


           

            var result = await _transactionWorkflow.CreateTransactionWorkflow(actualTransaction);
            if (result)
                await _transactionWorkflow.SendNotification(actualTransaction);


            return transaction.MapToModel();
        }
        public async Task ApproveAwaitingReceiveSalesPointTransfer(int transactionId)
        {
            var transaction = _salesPointTransfer.GetAllActive().FirstOrDefault(x => x.Id == transactionId);
            if (transaction is null) throw new AppException("Transaction not found");

           
            transaction.IsConfirmed = true;
            transaction.TransactionStatus = TransactionStatus.WaitingForReceive;
            await _salesPointTransfer.UpdateAsync(transaction);
        }
        public async Task<List<SalesPointModel>> GetByCodes(List<string> codes)
        {
            var r = await _salesPoint.GetAllActive().Where(x => codes.Contains(x.Code)).ToListAsync();
            return r.ToMap<SalesPoint, SalesPointModel>();
        }

        public async Task<List<SalesPointTransferModel>> GetReceivableTransfersByCurrentUser()
        {

            var transactions = await _salesPointTransfer.GetAllActive().Where(x => x.IsConfirmed
                && x.TransactionStatus == TransactionStatus.WaitingForReceive).ToListAsync();

            var tIds = transactions.Select(x => x.Id).ToList();
            var toCWIds = transactions.Select(x => x.ToSalesPointId).ToList();
            var toSalesPoints = _salesPoint.GetAllActive().Where(x => toCWIds.Contains(x.Id)).ToList().ToMap<SalesPoint, SalesPointModel>();
            var items = _salesPointTransferItem.GetAllActive().Where(x => tIds.Contains(x.TransferId)).Include(x => x.POSMProduct).ToList().MapToModel();

            var trModels = transactions.MapToModel();
            trModels.ForEach(model =>
            {
                model.Items = items.FindAll(x => x.TransferId == model.Id);
            });
            InsertSalesPointIntoTransfer(trModels);
            return trModels;
        }

        public async Task<SalesPointReceivedTransferModel> RecieveTransfer(SalesPointReceivedTransferModel transaction)
        {
            var user = AppIdentity.AppUser;
            if (transaction is null) throw new AppException("Transaction not found");
            if (transaction.Items.Count == 0) throw new AppException("No products provided");
            var referredDistribution = _salesPointTransfer.GetAllActive().FirstOrDefault(x => x.Id == transaction.SourceTransferId);
            if (referredDistribution is null) throw new AppException("Source transaction not found");

            var posmIds = transaction.Items.Select(x => x.POSMProductId).ToList();
            var spStocks = _salesPointStock.GetAllActive()
                .Where(x => x.SalesPointId == transaction.FromSalesPointId && posmIds.Contains(x.POSMProductId)).Include(x=>x.POSMProduct)
                .ToList().MapToModel();
            _common.InsertAvalableSpQuantity(spStocks);

            var stockViolatedItems = spStocks.Where(st => transaction.Items.Any(item =>
                    item.POSMProductId == st.POSMProductId &&
                    item.ReceivedQuantity > st.AvailableQuantity + item.Quantity))
                .ToList();
                //transaction.Items.Where(item => spStocks.Any(st =>
                //st.POSMProductId == item.POSMProductId &&
                //item.ReceivedQuantity > st.AvailableQuantity + item.Quantity)).ToList();
            if (stockViolatedItems.Any())
            {
                var posmNames = string.Join(",", stockViolatedItems.Select(x => x.POSMProduct.Name));
                throw new AppException($"Received quantity exceeds stock available quantity for {posmNames}");
            }

            SalesPoint toWareHouse = _salesPoint.GetAllActive().FirstOrDefault(x => x.Id == transaction.ToSalesPointId);
            SalesPoint fromWareHouse = _salesPoint.GetAllActive().FirstOrDefault(x => x.Id == transaction.FromSalesPointId);
            var newReceivedTransfer = transaction.ToMap<SalesPointReceivedTransferModel, SalesPointReceivedTransfer>();
            newReceivedTransfer.CreatedBy = user.UserId;
            newReceivedTransfer.TransactionDate = DateTime.UtcNow;
            var serial = _common.GetTransactionCount(TransactionType.SP_Receive) +1;
            newReceivedTransfer.TransactionNumber = await _common.GetTransactionNumber(TransactionType.CW_Receive,fromWareHouse.Code, toWareHouse.Code,serial);
            await _salesPointReceivedTransfer.CreateAsync(newReceivedTransfer);

            var items = transaction.Items.ToMap<SalesPointReceivedTransferItemModel, SalesPointReceivedTransferItem>();
            items.ForEach(x => x.TransferId = newReceivedTransfer.Id);

            await _salesPointReceivedTransferItem.CreateListAsync(items);

            referredDistribution.TransactionStatus = TransactionStatus.Completed;
            await _salesPointTransfer.UpdateAsync(referredDistribution);

            newReceivedTransfer.IsConfirmed = true;
            newReceivedTransfer.TransactionStatus = TransactionStatus.Completed;
            await _salesPointReceivedTransfer.UpdateAsync(newReceivedTransfer);

            transaction = newReceivedTransfer.MapToModel();
            transaction.Items = items.MapToModel();

            await updatePOSMStockOnRecieve(transaction);
            var receivedProducts = transaction.Items.Select(x => new ReceivedPOSM()
            {
                Quantity = x.ReceivedQuantity,
                POSMProductId = x.POSMProductId
            }).ToList();
            await _spWisePosmLedgerService.SPWisePOSMLedgerReceivedStock(receivedProducts,transaction.ToSalesPointId);


            return transaction;
        }

        public async Task<Pagination<SalesPointReceivedTransferModel>> GetReceivedTransfers(GetSalesPointReceivedTransfers queryParams)
        {
            if (queryParams.FromDateTime > queryParams.ToDateTime) (queryParams.FromDateTime, queryParams.ToDateTime) = (queryParams.ToDateTime, queryParams.FromDateTime);
            var query = _salesPointReceivedTransfer.GetAllActive().Where(x => x.TransactionStatus >= TransactionStatus.Completed && x.TransactionDate >= queryParams.FromDateTime && x.TransactionDate <= queryParams.ToDateTime);
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(x => x.TransactionNumber.Contains(queryParams.Search));
            }
            query = query.OrderByDescending(x => x.TransactionDate);
            var transfers = await query.ToPagedListAsync(queryParams.PageIndex, queryParams.PageSize);
            var transactionModels = transfers.ToList().MapToModel();

            var tIds = transactionModels.Select(x => x.Id).ToList();
            var sourceTransferIds = transactionModels.Select(x => x.SourceTransferId).ToList();

            var referenceDistributions = _salesPointTransfer.GetAllActive().Where(x => sourceTransferIds.Contains(x.Id)).ToList().MapToModel();

            var receivedItems = _salesPointReceivedTransferItem.GetAllActive().Where(x => tIds.Contains(x.TransferId)).Include(x => x.POSMProduct).ToList();

            transactionModels.ForEach(m =>
            {
                m.SourceTransfer = referenceDistributions.Find(x => x.Id == m.SourceTransferId);
                m.Items = receivedItems.FindAll(x => x.TransferId == m.Id).MapToModel();
            });

            InsertSalesPointIntoReceived(transactionModels);

            Pagination<SalesPointReceivedTransferModel> paginatedList = new Pagination<SalesPointReceivedTransferModel>(queryParams.PageIndex, queryParams.PageSize, transfers.TotalItemCount, transactionModels);

            return paginatedList;
        }

        public FileData DownloadExcelForSalesPointDistribution(DownloadExcelForSalesPointDistribution payload)
        {
            SalesPointDistributionData taskCreationData = GetWStockDistributionData(payload.FromSalesPointId, payload.ToSalesPointIds);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = GetWStockDistributionExcelHeaders(taskCreationData);

            SetTableStyle(workSheet, headers.Count);
            SetHeaderStyle(workSheet, headers.Count);
            InsertHeaders(headers, workSheet);
            Insert_SalesPointPOSM_DistributionExcelRows(taskCreationData, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        public async Task AddPosmToSalesPointStockAndPosmLedger(POSMProduct posmProduct)
        {
            var salesPointStocks = new List<SalesPointStock>();
            var posmLedgers = new List<SPWisePOSMLedger>();
            var ledgerDates =await _posmLedger.GetAllActive().Select(x => x.Date).Distinct().ToListAsync();

            var lastDate = ledgerDates.OrderByDescending(x=>x.Date).FirstOrDefault();
            var salesPointIds = await _salesPoint.GetAllActive().Select(x=>x.SalesPointId).ToListAsync();

            var isExistPosm = await _posmLedger.AnyAsync(x => x.PosmProductId == posmProduct.Id);
            if (isExistPosm) return;

            foreach (var salesPointId in salesPointIds)
            {
                salesPointStocks.Add(new SalesPointStock {SalesPointId = salesPointId, POSMProductId = posmProduct.Id, Quantity = 0});
                posmLedgers.Add(new SPWisePOSMLedger {
                    ClosingStock = 0, OpeningStock = 0, ReceivedStock = 0, ExecutedStock =  0, 
                    SalesPointId = salesPointId, PosmProductId = posmProduct.Id,
                    Date = lastDate
                });
            }
            await _salesPointStock.CreateListAsync(salesPointStocks);
            await _posmLedger.CreateListAsync(posmLedgers);
        }

        public async Task<SalesPointTransferModel> GetSalesPointTransferById(int id)
        {
            var loggedInUser = AppIdentity.AppUser;

            var transfer = await _salesPointTransfer.GetAllActive().Where(x => x.Id == id)
                .Include(x => x.SalesPointTransferItems)
                .ThenInclude(x=>x.POSMProduct)
                .FirstOrDefaultAsync();

            if (transfer is null) throw new AppException("Transaction not found");

            var fromSalespoint = _salesPoint.GetAllActive()
                .FirstOrDefault(x => x.SalesPointId == transfer.FromSalesPointId);
            var toSalespoint = _salesPoint.GetAllActive()
                .FirstOrDefault(x => x.SalesPointId == transfer.ToSalesPointId);
            var transferModel = transfer.MapToModel();
            transferModel.FromSalesPoint = fromSalespoint?.ToMap<SalesPoint, SalesPointModel>();
            transferModel.ToSalesPoint = toSalespoint?.ToMap<SalesPoint, SalesPointModel>();

            var transactionNotification = await _notificationService.TransactionNotification(loggedInUser.UserId, transferModel.Id);
            if (transactionNotification != null) transferModel.TransactionNotification = transactionNotification;
            return transferModel;
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

        private void InsertSalesPointIntoTransfer(List<SalesPointTransferModel> models)
        {
            var fromSids = models.Select(x => x.FromSalesPointId).ToList();
            var toSids = models.Select(x => x.ToSalesPointId).ToList();
            var sps = _salesPoint.GetAllActive()
                .Where(x => fromSids.Contains(x.SalesPointId) || toSids.Contains(x.SalesPointId)).ToList().ToMap<SalesPoint,SalesPointModel>();
            models.ForEach(x =>
            {
                x.FromSalesPoint = sps.FirstOrDefault(sp => sp.SalesPointId == x.FromSalesPointId);
                x.ToSalesPoint = sps.FirstOrDefault(sp => sp.SalesPointId == x.ToSalesPointId);
            });
        }

        private void InsertSalesPointIntoReceived(List<SalesPointReceivedTransferModel> models)
        {
            var fromSids = models.Select(x => x.FromSalesPointId).ToList();
            var toSids = models.Select(x => x.ToSalesPointId).ToList();
            var sps = _salesPoint.GetAllActive()
                .Where(x => fromSids.Contains(x.SalesPointId) || toSids.Contains(x.SalesPointId)).ToList().ToMap<SalesPoint, SalesPointModel>();
            models.ForEach(x =>
            {
                x.FromSalesPoint = sps.FirstOrDefault(sp => sp.SalesPointId == x.FromSalesPointId);
                x.ToSalesPoint = sps.FirstOrDefault(sp => sp.SalesPointId == x.ToSalesPointId);
            });
        }
        private List<string> GetWStockDistributionExcelHeaders(SalesPointDistributionData taskCreationData)
        {
            var headers = new List<string>() { "From SalesPoint Code", "From SalesPoint Name", "To SalesPoint Code", "To SalesPoint Name", };
            var posmProductsNames = taskCreationData.PosmProducts.Select(x => x.Name).ToList();
            headers.AddRange(posmProductsNames);
            return headers;
        }

        private SalesPointDistributionData GetWStockDistributionData(int fromSalesPointId, List<int> toSalesPointId)
        {
            SalesPointDistributionData distributionData = new SalesPointDistributionData();

            distributionData.FromSalesPoint = _salesPoint.GetAllActive().FirstOrDefault(x => x.Id == fromSalesPointId).ToMap<SalesPoint, SalesPointModel>();
            distributionData.ToSalesPoints = _salesPoint.GetAllActive().Where(x => toSalesPointId.Contains(x.SalesPointId)).ToList().ToMap<SalesPoint, SalesPointModel>();
            var user = AppIdentity.AppUser;

            var spStockModels = _salesPointStock.GetAllActive().Where(x => x.SalesPointId == fromSalesPointId)
                .Include(x => x.POSMProduct).ToList().MapToModel();

            _common.InsertAvalableSpQuantity(spStockModels);

            spStockModels = spStockModels.Where(x => x.AvailableQuantity > 0).ToList();


            var posmProducts = spStockModels.Select(x=>x.POSMProduct).Where(x=>x.IsJTIProduct).ToList();

            distributionData.PosmProducts = posmProducts;

            return distributionData;
        }

        private async Task updatePOSMStockOnRecieve(SalesPointReceivedTransferModel transactionModel)
        {
            var user = AppIdentity.AppUser;
            var posmIds = transactionModel.Items.Select(x => x.POSMProductId).ToList();
            var sourceSalesPointStocks = _salesPointStock.GetAllActive().Where(x => x.SalesPointId == transactionModel.FromSalesPointId && posmIds.Contains(x.POSMProductId)).ToList();
            var destinationSalesPointStocks = _salesPointStock.GetAllActive().Where(x => x.SalesPointId == transactionModel.ToSalesPointId && posmIds.Contains(x.POSMProductId)).ToList();
            var newStocks = new List<SalesPointStock>();
            foreach (var posmId in posmIds)
            {
                var item = transactionModel.Items.Find(x => x.POSMProductId == posmId);
                var srouceWStock = sourceSalesPointStocks.Find(x => x.POSMProductId == posmId);
                srouceWStock.Quantity -= item.ReceivedQuantity;
                if (srouceWStock.Quantity < 0) srouceWStock.Quantity = 0;
                var destinationWStock = destinationSalesPointStocks.Find(x => x.POSMProductId == posmId);

                if (destinationWStock is null)
                {
                    SalesPointStock newStock = new SalesPointStock()
                    {
                        CreatedBy = user.UserId,
                        POSMProductId = posmId,
                        Quantity = item.ReceivedQuantity,
                        SalesPointId = transactionModel.ToSalesPointId,
                    };
                    newStocks.Add(newStock);
                }
                else destinationWStock.Quantity += item.ReceivedQuantity;
            }
            await _salesPointStock.UpdateListAsync(sourceSalesPointStocks);
            await _salesPointStock.UpdateListAsync(destinationSalesPointStocks);
            if (newStocks.Any()) await _salesPointStock.CreateListAsync(newStocks);
        }

        private static void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }

        private void Insert_SalesPointPOSM_DistributionExcelRows(SalesPointDistributionData data, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            data.ToSalesPoints.ForEach(tSp =>
            {
                workSheet.Cells[currentRowNumberForData, 1].Value = $"{data.FromSalesPoint.Code}";
                workSheet.Cells[currentRowNumberForData, 2].Value = $"{data.FromSalesPoint.Name}";
                workSheet.Cells[currentRowNumberForData, 3].Value = $"{tSp.Code}";
                workSheet.Cells[currentRowNumberForData, 4].Value = $"{tSp.Name}";
                currentRowNumberForData++;
            });
            
        }
    }
}
