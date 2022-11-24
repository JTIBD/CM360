using FMAplication.Domain.Products;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Repositories;
using FMAplication.Services.POSMProducts.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Audit;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Task;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Exceptions;
using FMAplication.Services.AzureStorageService.Interfaces;
using FMAplication.Services.Sales.Interfaces;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services.POSMProducts.Implementation
{
    public class POSMProductService : IPOSMProductService
    {
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ISalesPointService _salesPointService;
        private readonly IRepository<AuditPOSMProduct> _auditPosmProduct;
        private readonly IRepository<PosmTaskAssign> _posmTaskAssign;
        private readonly IRepository<SalesPointAdjustmentItem> _salespointAdjustmentItems;
        private readonly IRepository<SalesPointReceivedTransferItem> _salespointReceivedTransferItem;
        private readonly IRepository<SalesPointTransferItem> _salespointTransferItem;
        private readonly IRepository<SPWisePOSMLedger> _spWiseLedger;
        private readonly IRepository<StockAddTransaction> _stockAddTransaction;
        private readonly IRepository<StockAdjustmentItems> _stockAdjustmentItem;
        private readonly IRepository<WareHouseReceivedTransferItem> _warehouseReceivedTransferItem;
        private readonly IRepository<WareHouseTransferItem> _warehouseTransferItems;
        private readonly IRepository<WDistributionRecieveTransaction> _wDistributionReceivedTransaction;
        private readonly IRepository<WDistributionTransaction> _wDistributionTransaction;

        public POSMProductService(IRepository<POSMProduct> example, 
            IBlobStorageService blobStorageService, 
            ISalesPointService salesPointService, IRepository<AuditPOSMProduct> auditPosmProduct, IRepository<PosmTaskAssign> posmTaskAssign, IRepository<SalesPointAdjustmentItem> salespointAdjustmentItems, IRepository<SalesPointReceivedTransferItem> salespointReceivedTransferItem, IRepository<SalesPointTransferItem> salespointTransferItem, IRepository<SPWisePOSMLedger> spWiseLedger, IRepository<StockAddTransaction> stockAddTransaction, IRepository<StockAdjustmentItems> stockAdjustmentItem, IRepository<WareHouseReceivedTransferItem> warehouseReceivedTransfer, IRepository<WareHouseTransferItem> warehouseTransferItems, IRepository<WDistributionRecieveTransaction> wDistributionReceivedTransfer, IRepository<WDistributionTransaction> wDistributionTransaction)
        {
            _posmProduct = example;
            _blobStorageService = blobStorageService;
            _salesPointService = salesPointService;
            _auditPosmProduct = auditPosmProduct;
            _posmTaskAssign = posmTaskAssign;
            _salespointAdjustmentItems = salespointAdjustmentItems;
            _salespointReceivedTransferItem = salespointReceivedTransferItem;
            _salespointTransferItem = salespointTransferItem;
            _spWiseLedger = spWiseLedger;
            _stockAddTransaction = stockAddTransaction;
            _stockAdjustmentItem = stockAdjustmentItem;
            _warehouseReceivedTransferItem = warehouseReceivedTransfer;
            _warehouseTransferItems = warehouseTransferItems;
            _wDistributionReceivedTransaction = wDistributionReceivedTransfer;
            _wDistributionTransaction = wDistributionTransaction;
        }


        public async Task<POSMProductModel> CreateAsync(POSMProductModel model)
        {
            var example = model.ToMap<POSMProductModel, POSMProduct>();
            var result = await _posmProduct.CreateAsync(example);

            return result.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReferences(id);
            var result = await _posmProduct.DeleteAsync(s => s.Id == id);
            return result;

        }

        private async Task CheckReferences(int posmId)
        {
            var auditPosmProducts = await _auditPosmProduct.GetAll().CountAsync(x => x.POSMProductId == posmId);
            var posmTaskAssign = await _posmTaskAssign.GetAll().CountAsync(x => x.PosmProductId == posmId);
            var salesPointAdjustmentItems =
                await _salespointAdjustmentItems.GetAll().CountAsync(x => x.PosmProductId == posmId);
            var salespointReceivedTransferItems = await 
                _salespointReceivedTransferItem.GetAll().CountAsync(x => x.POSMProductId == posmId);
            var spTransferItems = await _salespointTransferItem.GetAll().CountAsync(x => x.POSMProductId == posmId);
            var stockAddTransactions = await _stockAddTransaction.GetAll().CountAsync(x => x.PosmProductId == posmId);
            var stockAdjustmentItems = await _stockAdjustmentItem.GetAll().CountAsync(x => x.PosmProductId == posmId);
            var warehouseReceivedTransferItems = await _warehouseReceivedTransferItem.GetAll().CountAsync(x=>x.POSMProductId == posmId);
            var warehouseTransferItems =
                await _warehouseTransferItems.GetAll().CountAsync(x => x.POSMProductId == posmId);
            var wDistributionReceivedTransactions =
                await _wDistributionReceivedTransaction.GetAll().CountAsync(x => x.POSMProductId == posmId);

            var wDistributionTransactions =
                await _wDistributionTransaction.GetAll().CountAsync(x => x.POSMProductId == posmId);

            var references = new List<string>();
            if(auditPosmProducts > 0) references.Add("Audit setup");
            if(posmTaskAssign > 0) references.Add("Posm assign");
            if(salesPointAdjustmentItems > 0) references.Add("Salespoint stock adjustment");
            if(salespointReceivedTransferItems > 0) references.Add("Salespoint received transfer");
            if(spTransferItems > 0) references.Add("Salespoint transfer");
            if (stockAddTransactions > 0) references.Add("Warehouse stock add transaction");
            if (stockAdjustmentItems > 0) references.Add("Warehouse stock adjustment");
            if (warehouseReceivedTransferItems > 0) references.Add("Warehouse received transfer");
            if (warehouseTransferItems > 0) references.Add("Warehouse transfer");
            if (wDistributionReceivedTransactions > 0) references.Add("Warehouse received transaction");
            if (wDistributionTransactions > 0) references.Add("Warehouse distribution transaction");

            if (references.Any()) throw new AppException($"The posm product has been used in {string.Join(", ",references)}");

        }

        public async Task<bool> IsCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _posmProduct.IsExistAsync(s => s.Code == code)
                : await _posmProduct.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }
        public async Task<POSMProductModel> GetPOSMProductAsync(int id)
        {
            var result = await _posmProduct.FindAsync(s => s.Id == id);
            return result.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<IEnumerable<POSMProductModel>> GetPOSMProductsAsync()
        {
            var result = await _posmProduct.GetAllAsync();
            return result.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<IPagedList<POSMProductModel>> GetPagedPOSMProductsAsync(int pageNumber, int pageSize)
        {
            var result = await _posmProduct.GetAll().OrderByDescending(s => s.CreatedTime).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<POSMProduct, POSMProductModel>();

        }

        public async Task<IPagedList<POSMProductModel>> GetApprovedPOSMProductsAsync(int pageNumber, int pageSize)
        {
            var result = await _posmProduct.GetAll().OrderByDescending(s => s.CreatedTime).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<POSMProduct, POSMProductModel>();

        }

        public async Task<IEnumerable<POSMProductModel>> GetQueryPOSMProductsAsync()
        {
            var result = await _posmProduct.ExecuteQueryAsyc<POSMProductModel>("SELECT * FROM POSMProducts");
            return result;
        }

        public async Task<POSMProductModel> SaveAsync(POSMProductModel model)
        {
            var existingProduct = await _posmProduct.FindAsync(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());
            if (existingProduct != null) throw new AppException($"Product exists with same name");
            if (model.IsPlanogram && model.PlanogramImageFile == null)
                throw new AppException("You must need to provide a Planogram Image");

            #region Image save
            if (model.IsPlanogram && model.PlanogramImageFile != null)
                model.PlanogramImageUrl = await GetFilePath(model.PlanogramImageFile);

            if (model.ImageFile != null)
                model.ImageUrl = await GetFilePath(model.ImageFile);
            #endregion

            var example = model.ToMap<POSMProductModel, POSMProduct>();
            var result = await _posmProduct.CreateOrUpdateAsync(example);
            if (result.IsJTIProduct) await _salesPointService.AddPosmToSalesPointStockAndPosmLedger(result);
            return result.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<POSMProductModel> UpdateAsync(POSMProductModel model)
        {
            

            var planogramImage = model.PlanogramImageFile == null && model.PlanogramImageUrl == "";
            if (model.IsPlanogram && planogramImage) 
                throw new AppException("You must need to provide a Planogram Image");

            var posmProduct = await _posmProduct.FindAsync(x => x.Id == model.Id); 
            if (posmProduct == null) throw new AppException("Posm Product not found");
            if (posmProduct.Name != model.Name)
            {
                var existingProductWithSameName = _posmProduct.Find(x => x.Id != model.Id && x.Name == model.Name);
                if (existingProductWithSameName is object) throw new AppException($"Product exist with the updated name");
            }
            posmProduct.Code = model.Code;
            posmProduct.Name = model.Name;
            posmProduct.CampaignId = model.CampaignId;
            posmProduct.BrandId = model.BrandId;
            posmProduct.SubBrandId = model.SubBrandId;
            posmProduct.Status = model.Status;
            posmProduct.Type = (int)model.Type;
            posmProduct.IsDigitalSignatureEnable = model.IsDigitalSignatureEnable;
            posmProduct.IsPlanogram = model.IsPlanogram;
            if (posmProduct.IsJTIProduct && !model.IsJTIProduct)
                throw new AppException("You can't change JIT Product to non JTI product");

            posmProduct.IsJTIProduct = model.IsJTIProduct;

            #region Image save
            if (model.IsPlanogram && model.PlanogramImageFile != null && model.PlanogramImageUrl != posmProduct.PlanogramImageUrl)
                posmProduct.PlanogramImageUrl = await GetFilePath(model.PlanogramImageFile);

            if (model.ImageFile != null && model.ImageUrl != posmProduct.ImageUrl)
                posmProduct.ImageUrl = await GetFilePath(model.ImageFile);
            #endregion

            var result = await _posmProduct.UpdateAsync(posmProduct);
            if (result.IsJTIProduct) await _salesPointService.AddPosmToSalesPointStockAndPosmLedger(result);

            return result.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<List<POSMProductModel>> GetAllPOSMProductsAsync()
        {
            var posms = await _posmProduct.GetAllActive().Where(x=>x.IsJTIProduct).ToListAsync();
            return posms.ToMap<POSMProduct, POSMProductModel>();
        }

        public async Task<List<POSMProductModel>> GetAllJtiPosmProducts()
        {
            var posms = await _posmProduct.GetAllActive().Where(x=>x.IsJTIProduct).ToListAsync();
            return posms.ToMap<POSMProduct, POSMProductModel>();
        }

        #region Private methods

        

        private async Task<string> GetFilePath(IFormFile file)
        {
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                string mimeType = "application/octet-stream";//model.File.ContentType;


                byte[] fileData;
                await using (var target = new MemoryStream())
                {
                    await file.CopyToAsync(target);
                    fileData = target.ToArray();
                }

                string folder = "POSMProduct";

                var filePath = await _blobStorageService.UploadFileToBlobAsync(fileName,
                    fileData, mimeType, folder);
                return filePath;
            }

            return "";

        }

        #endregion
    }
}
