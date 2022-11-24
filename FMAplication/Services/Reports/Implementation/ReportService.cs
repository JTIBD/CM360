using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Helpers;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Common;
using FMAplication.Models.DailyTasks;
using FMAplication.Models.Products;
using FMAplication.Models.Reports;
using FMAplication.Models.Sales;
using FMAplication.Models.SPWisePOSMLedgers;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;
using FMAplication.Repositories;
using FMAplication.RequestModels.Reports;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.Reports.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using OfficeOpenXml;
using X.PagedList;

namespace FMAplication.Services.Reports.Implementation
{
    public class ReportService:IReportService
    {
        private readonly ICommonService _common;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<AvSetup> _avSetup;
        private readonly IRepository<CommunicationSetup>_communicationSetup;
        private readonly IRepository<SalesPoint>_salesPoint;
        private readonly  IRepository<DailyConsumerSurveyTaskAnswer>_dailyConsumerSurveyTaskAnswer;
        private readonly IRepository<DailySurveyTaskAnswer> _dailySurveyTaskAnswer;
        private readonly IRepository<DailyAVTask> _dailyAVTask;
        private readonly IRepository<DailyCommunicationTask> _dailyCommunicationTasks;
        private readonly IRepository<DailyInformationTask> _dailyInformationTask;
        private readonly IRepository<DailyPosmTaskItems> _dailyPosmTaskItem;
        private readonly IRepository<Product> _product;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<DailyProductsAuditTask> _dailyProductsAuditTask;
        private IFileService _file;
        private readonly string StatusComplete = "Complete";
        private readonly string StatusInComplete = "Incomplete";
        private readonly string StatusOutletClosed = "Outlet Closed";
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<StockAddTransaction> _stockAddTransaction;
        private readonly IRepository<WDistributionTransaction> _wDistributionTransaction;
        private readonly IRepository<WDistributionRecieveTransaction> _wDistributionRecieveTransaction;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNodeMapping;
        private readonly IRepository<Node> _node;
        private readonly IRepository<WareHouseStock> _wStock;
        private readonly IRepository<SalesPointStock> _spStock;
        private readonly IRepository<SPWisePOSMLedger> _spWisePOSMLedger;
        private readonly IRepository<DailyAuditTask> _dailyAuditTask;
        private readonly IRepository<DailySurveyTask> _dailySurveyTask;
        private readonly IRepository<SalesPointReceivedTransfer> _spReceivedTransfers;
        private readonly IRepository<SalesPointReceivedTransferItem> _spReceivedTransferItems;

        public ReportService(ICommonService common, IRepository<DailyTask> dailyTask, IRepository<DailyPosmTask> dailyPosmTask, IRepository<AvSetup> avSetup, IRepository<CommunicationSetup> communicationSetup, IRepository<SalesPoint> salesPoint, IRepository<DailyConsumerSurveyTaskAnswer> dailyConsumerSurveyTaskAnswer, IRepository<DailySurveyTaskAnswer> dailySurveyTaskAnswer, IRepository<DailyAVTask> dailyAvTask, IRepository<DailyCommunicationTask> dailyCommunicationTasks, IRepository<DailyInformationTask> dailyInformationTask, IRepository<DailyPosmTaskItems> dailyPosmTaskItem, IRepository<Product> product, IRepository<POSMProduct> posmProduct, IRepository<DailyProductsAuditTask> dailyProductsAuditTask, IFileService file, IRepository<Transaction> transaction, IRepository<StockAddTransaction> stockAddTransaction, IRepository<WDistributionTransaction> wDistributionTransaction, IRepository<WDistributionRecieveTransaction> wDistributionRecieveTransaction, IRepository<SalesPointNodeMapping> salesPointNodeMapping, IRepository<Node> node, IRepository<WareHouseStock> wStock, IRepository<SalesPointStock> spStock, IRepository<SPWisePOSMLedger> spWisePosmLedger, IRepository<DailyAuditTask> dailyAuditTask, IRepository<DailySurveyTask> dailySurveyTask, IRepository<SalesPointReceivedTransfer> spReceivedTransfers, IRepository<SalesPointReceivedTransferItem> spReceivedTransferItems)
        {
            _common = common;
            _dailyTask = dailyTask;
            _dailyPosmTask = dailyPosmTask;
            _avSetup = avSetup;
            _communicationSetup = communicationSetup;
            _salesPoint = salesPoint;
            _dailyConsumerSurveyTaskAnswer = dailyConsumerSurveyTaskAnswer;
            _dailySurveyTaskAnswer = dailySurveyTaskAnswer;
            _dailyAVTask = dailyAvTask;
            _dailyCommunicationTasks = dailyCommunicationTasks;
            _dailyInformationTask = dailyInformationTask;
            _dailyPosmTaskItem = dailyPosmTaskItem;
            _product = product;
            _posmProduct = posmProduct;
            _dailyProductsAuditTask = dailyProductsAuditTask;
            _file = file;
            _transaction = transaction;
            _stockAddTransaction = stockAddTransaction;
            _wDistributionTransaction = wDistributionTransaction;
            _wDistributionRecieveTransaction = wDistributionRecieveTransaction;
            _salesPointNodeMapping = salesPointNodeMapping;
            _node = node;
            _wStock = wStock;
            _spStock = spStock;
            _spWisePOSMLedger = spWisePosmLedger;
            _dailyAuditTask = dailyAuditTask;
            _dailySurveyTask = dailySurveyTask;
            _spReceivedTransfers = spReceivedTransfers;
            _spReceivedTransferItems = spReceivedTransferItems;
        }

        public async Task<Pagination<DailyTaskModel>> GetAuditReport(int pageIndex, int pageSize, string search,
            DateTime FromDateTime, DateTime ToDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyAuditTask.GetAllActive().Where(x =>
                x.DailyTask.IsSubmitted &&
                salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyTask.DateTime >= FromDateTime && x.DailyTask.DateTime <= ToDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            var query2 = query.Include(x => x.DailyProductsAuditTask)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser)
                .Include(x => x.Reason);
            
            var result = await query2.OrderByDescending(x => x.DailyTask.DateTime).ToListAsync();
            var resultList = result.SelectMany(x=>x.IsOutletOpen ? x.DailyProductsAuditTask:new List<DailyProductsAuditTask>()
            {
                new DailyProductsAuditTask()
                {
                    DailyAuditTask = x,
                    DailyAuditTaskId = x.Id
                }
            }).ToList();
            var pagedList = resultList.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
            var dailyTasks = pagedList.Select(x => x.DailyAuditTask.DailyTask).DistinctBy(x => x.Id).ToList();
            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailyAuditTasks = pagedList.Select(x => x.DailyAuditTask).Where(x=>x.DailyTaskId == dailyTask.Id).DistinctBy(x => x.Id).ToList();
                foreach (var dailyAuditTask in dailyTask.DailyAuditTasks)
                {
                    dailyAuditTask.DailyProductsAuditTask =
                        pagedList.Where(x => x.DailyAuditTaskId == dailyAuditTask.Id).ToList();
                }
            }
            var models = dailyTasks.MapToModel();
            _common.InsertOutlets(models.SelectMany(x=>x.DailyAuditTasks).ToList());
            _common.InsertRoutesOptional(models.SelectMany(x=>x.DailyAuditTasks).Select(x=>x.Outlet).ToList());
            _common.InsertSalesPoints(models);

            var dailyAuditProducts = models.SelectMany(x => x.DailyAuditTasks).SelectMany(x => x.DailyProductsAuditTask)
                .Where(x => x.ActionType == ActionType.PriceAuditProduct ||
                            x.ActionType == ActionType.DistributionCheckProduct).ToList();
            var auditProductIds = dailyAuditProducts.Select(x => x.ProductId).ToList();
            var auditProducts = _product.GetAllActive().Where(x => auditProductIds.Contains(x.Id)).ToList().ToMap<Product,ProductModel>();
            dailyAuditProducts.ForEach(x=> x.Product = auditProducts.FirstOrDefault(p => p.Id == x.ProductId));

            var dailyAuditPOSMProducts = models.SelectMany(x => x.DailyAuditTasks).SelectMany(x => x.DailyProductsAuditTask)
                .Where(x => x.ActionType == ActionType.FacingCountProduct ||
                            x.ActionType == ActionType.PlanogramCheckProduct).ToList();
            var posmProductIds = dailyAuditPOSMProducts.Select(x => x.ProductId).ToList();

            var posmProducts = _posmProduct.GetAllActive().Where(x => posmProductIds.Contains(x.Id)).ToList()
                .ToMap<POSMProduct, POSMProductModel>();
            dailyAuditPOSMProducts.ForEach(x=>x.POSMProduct = posmProducts.FirstOrDefault(p=>p.Id == x.ProductId));

            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, resultList.Count, models);
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetSurveyReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailySurveyTask.GetAllActive().Where(x =>
                x.DailyTask.IsSubmitted &&
                salesPointIds.Contains(x.DailyTask.SalesPointId) &&
                x.DailyTask.DateTime >= fromDateTime && x.DailyTask.DateTime <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.DailySurveyTaskAnswers)
                .ThenInclude(x => x.Question)
                .Include(x => x.SurveyQuestionSet)
                .Include(x => x.Reason)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser);
            query = query.OrderByDescending(x => x.DailyTask.DateTime);

            var result = await query.ToListAsync();
            var resultList = result.SelectMany(x => x.IsCompleted && x.IsOutletOpen ? x.DailySurveyTaskAnswers : new List<DailySurveyTaskAnswer>()
            {
                new DailySurveyTaskAnswer()
                {
                    DailySurveyTask = x,
                    DailySurveyTaskId = x.Id
                }
            }).ToList();
            var pagedList = resultList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var dailyTasks = pagedList.Select(x => x.DailySurveyTask.DailyTask).DistinctBy(x=>x.Id).ToList();
            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailySurveyTasks = pagedList.Select(x => x.DailySurveyTask)
                    .Where(x => x.DailyTaskId == dailyTask.Id).DistinctBy(x => x.Id).ToList();
                foreach (var dSurveyTask in dailyTask.DailySurveyTasks)
                {
                    dSurveyTask.DailySurveyTaskAnswers = pagedList.Where(x => x.DailySurveyTaskId == dSurveyTask.Id).DistinctBy(x=>x.Id).ToList();
                }
            }
            var models = dailyTasks.MapToModel();
            _common.InsertOutlets(models.SelectMany(x=>x.DailySurveyTasks).ToList());
            _common.InsertRoutesOptional(models.SelectMany(x=>x.DailySurveyTasks).Select(x=>x.Outlet).ToList());
            _common.InsertSalesPoints(models);
            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, resultList.Count, models);
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetConsumerSurveyReport(int pageIndex, int pageSize,
            string search, DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyConsumerSurveyTaskAnswer.GetAllActive().Where(x =>
                x.DailyConsumerSurveyTask.DailyTask.IsSubmitted &&
                salesPointIds.Contains(x.DailyConsumerSurveyTask.DailyTask.SalesPointId) && x.DailyConsumerSurveyTask.DailyTask.DateTime >=fromDateTime && x.DailyConsumerSurveyTask.DailyTask.DateTime <= toDateTime);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyConsumerSurveyTask.DailyTask.SalesPointId));
            }
            query = query
            .Include(x => x.Question)
            .Include(x => x.DailyConsumerSurveyTask)
            .ThenInclude(x => x.DailyTask)
            .ThenInclude(x => x.CmUser)
            .Include(x => x.DailyConsumerSurveyTask)
            .ThenInclude(x => x.Reason)
            .Include(x => x.DailyConsumerSurveyTask)
            .ThenInclude(x => x.SurveyQuestionSet); ;

            query = query.OrderByDescending(x => x.DailyConsumerSurveyTask.DailyTask.DateTime);

            var result = await query.ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();
            var dailyTasks = result.ToList().Select(x => x.DailyConsumerSurveyTask.DailyTask).DistinctBy(x=>x.Id).ToList();
            foreach (var dTask in dailyTasks)
            {
                dTask.DailyConsumerSurveyTasks = resultList.Select(x => x.DailyConsumerSurveyTask)
                    .Where(x => x.DailyTaskId == dTask.Id).DistinctBy(x=>x.Id).ToList();
                foreach (var dConsumerTask in dTask.DailyConsumerSurveyTasks)
                {
                    dConsumerTask.DailyConsumerSurveyTaskAnswers =
                        resultList.Where(x => x.DailyConsumerSurveyTaskId == dConsumerTask.Id).ToList();
                }
            }
            var models = dailyTasks.MapToModel();
            _common.InsertSalesPoints(models);
            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, result.TotalItemCount, models);
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetAvReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyAVTask.GetAllActive().Where(x =>
                x.DailyTask.IsSubmitted && salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyTask.DateTime >= fromDateTime && x.DailyTask.DateTime <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.Reason)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser);

            query = query.OrderByDescending(x => x.DailyTask.DateTime);

            var result = await query.ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();

            var dailyTasks = resultList.Select(x => x.DailyTask).DistinctBy(x=>x.Id).ToList();

            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailyAVTasks = resultList.Where(x => x.DailyTaskId == dailyTask.Id).ToList();
            }
            var models = dailyTasks.MapToModel();
            var dailyAvTasks = models.SelectMany(x => x.DailyAVTasks).ToList();
            var avSetupIds = dailyAvTasks.Select(x=>x.AvSetupId);
            var avSetups = _avSetup.GetAllActive().Where(x => avSetupIds.Contains(x.Id)).Include(x=>x.Av).ToList().MapToModel();
            foreach (var dailyAvTaskModel in dailyAvTasks)
            {
                dailyAvTaskModel.AvSetup = avSetups.Find(x=>x.Id == dailyAvTaskModel.AvSetupId);
            }

            _common.InsertOutlets(dailyAvTasks);
            _common.InsertRoutesOptional(dailyAvTasks.Select(x=>x.Outlet).ToList());
            _common.InsertSalesPoints(models);

            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, result.TotalItemCount,models );
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetCommunicationReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyCommunicationTasks.GetAllActive().Where(x =>
                x.DailyTask.IsSubmitted && salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyTask.DateTime >= fromDateTime && x.DailyTask.DateTime <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.Reason)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser);

            query = query.OrderByDescending(x => x.DailyTask.DateTime);

            var result = await query.ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();

            var dailyTasks = resultList.Select(x => x.DailyTask).DistinctBy(x=>x.Id).ToList();
            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailyCommunicationTasks = resultList.Where(x => x.DailyTaskId == dailyTask.Id).ToList();
            }

            var models = dailyTasks.MapToModel();
            var dailyAvTasks = models.SelectMany(x => x.DailyCommunicationTasks).ToList();
            var communicationSetupIds = dailyAvTasks.Select(x => x.CommunicationSetupId);
            var communicationSetups = _communicationSetup.GetAllActive().Where(x => communicationSetupIds.Contains(x.Id)).Include(x => x.AvCommunication).ToList().MapToModel();
            foreach (var dailyAvTaskModel in dailyAvTasks)
            {
                dailyAvTaskModel.CommunicationSetup = communicationSetups.Find(x => x.Id == dailyAvTaskModel.CommunicationSetupId);
            }
            _common.InsertOutlets(dailyAvTasks);
            _common.InsertRoutesOptional(dailyAvTasks.Select(x=>x.Outlet).ToList());
            _common.InsertSalesPoints(models);
            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, result.TotalItemCount, models);
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetInformationReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyInformationTask.GetAllActive().Where(x =>
                x.DailyTask.IsSubmitted && salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyTask.DateTime >= fromDateTime && x.DailyTask.DateTime <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.Reason)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser);

            query = query.OrderByDescending(x => x.DailyTask.DateTime);

            var result = await query.ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();
            var dailyTasks = resultList.Select(x => x.DailyTask).DistinctBy(x=>x.Id).ToList();
            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailyInformationTasks = resultList.Where(x => x.DailyTaskId == dailyTask.Id).ToList();
            }
            var models = dailyTasks.MapToModel();
            _common.InsertOutlets(models.SelectMany(x=>x.DailyInformationTasks).ToList());
            _common.InsertRoutesOptional(models.SelectMany(x=>x.DailyInformationTasks).Select(x=>x.Outlet).ToList());
            _common.InsertSalesPoints(models);
            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, result.TotalItemCount, models);
            return list;
        }

        public async Task<Pagination<DailyTaskModel>> GetPOSMTaskReport(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            try
            {
                var user = AppIdentity.AppUser;
                var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
                var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
                var query = _dailyPosmTask.GetAllActive().Where(x =>
                        x.DailyTask.IsSubmitted &&
                        salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyTask.DateTime >= fromDateTime && x.DailyTask.DateTime <= toDateTime);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                    query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
                }
                var query2 = query.Include(x => x.DailyPosmTaskItems)
                    .ThenInclude(x=>x.PosmProduct)
                    .Include(x => x.DailyTask)
                    .ThenInclude(x => x.CmUser)
                    .Include(x => x.Reason);

                var result = await query2.OrderByDescending(x => x.DailyTask.DateTime).ToListAsync();
                var flattenItems  = result.SelectMany(x =>  x.DailyPosmTaskItems.Any() ? x.DailyPosmTaskItems : new List<DailyPosmTaskItems> {new DailyPosmTaskItems { DailyPosmTask = x, DailyPosmTaskId = x.Id}}).ToList();
                var pagedItems = flattenItems.Skip((pageIndex-1) * pageSize).Take(pageSize).ToList();

                var dailyTasks = pagedItems.Select(x => x.DailyPosmTask.DailyTask).DistinctBy(x=>x.Id).ToList();
                foreach (var dailyTask in dailyTasks)
                {
                    dailyTask.DailyPosmTasks = pagedItems.Select(x => x.DailyPosmTask).Where(x=>x.DailyTaskId == dailyTask.Id).DistinctBy(x=>x.Id).ToList();
                    foreach (var dailyTaskDailyPosmTask in dailyTask.DailyPosmTasks)
                    {
                        dailyTaskDailyPosmTask.DailyPosmTaskItems = pagedItems
                            .Where(x => x.DailyPosmTaskId == dailyTaskDailyPosmTask.Id).ToList();
                    }
                }

                var models = dailyTasks.MapToModel();

                _common.InsertOutlets(models.SelectMany(x => x.DailyPosmTasks).ToList());
                _common.InsertRoutesOptional(models.SelectMany(x => x.DailyPosmTasks).Select(x=>x.Outlet).ToList());
                _common.InsertSalesPoints(models);
                var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, flattenItems.Count, models);
                return list;
            }
            catch (Exception ex)
            {
                return new Pagination<DailyTaskModel>(pageIndex, pageSize, 0, new List<DailyTaskModel>());
            }
        }



        public async Task<Pagination<DailyTaskModel>> GetPOSMTaskReport2(int pageIndex, int pageSize, string search,
            DateTime fromDateTime, DateTime toDateTime)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();
            var query = _dailyPosmTaskItem.GetAllActive().Where(x =>
                x.DailyPosmTask.DailyTask.IsSubmitted &&
                salesPointIds.Contains(x.DailyPosmTask.DailyTask.SalesPointId) && x.CreatedTime >= fromDateTime && x.CreatedTime <= toDateTime);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.DailyPosmTask.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.PosmProduct)
                .Include(x => x.DailyPosmTask)
                .ThenInclude(x => x.Reason)
                .Include(x => x.DailyPosmTask)
                .ThenInclude(x => x.DailyTask)
                .ThenInclude(x=>x.CmUser);
            query = query.OrderByDescending(x => x.CreatedTime);

            var result = await query.ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();

            var dailyTasks = resultList.Select(x => x.DailyPosmTask.DailyTask).DistinctBy(x=>x.Id).ToList();
            foreach (var dailyTask in dailyTasks)
            {
                dailyTask.DailyPosmTasks = resultList.Select(x => x.DailyPosmTask).Where(x=>x.DailyTaskId == dailyTask.Id).DistinctBy(x=>x.Id).ToList();
                foreach (var dailyTaskDailyPosmTask in dailyTask.DailyPosmTasks)
                {
                    dailyTaskDailyPosmTask.DailyPosmTaskItems = resultList
                        .SelectMany(x => x.DailyPosmTask.DailyPosmTaskItems)
                        .Where(x => x.DailyPosmTaskId == dailyTaskDailyPosmTask.Id).DistinctBy(x=>x.Id).ToList();
                }
            }

            var models = dailyTasks.MapToModel();
            _common.InsertOutlets(models.SelectMany(x => x.DailyPosmTasks).ToList());
            _common.InsertSalesPoints(models);
            var list = new Pagination<DailyTaskModel>(pageIndex, pageSize, result.TotalItemCount, models);
            return list;
        }

        public async Task<FileData> ExportAuditReportToExcel(int queryParamsPageIndex, int queryParamsPageSize,
            string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetAuditReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(AuditReportExceColumn)).Cast<AuditReportExceColumn>().Skip(1).Select(x=>x.GetDescription()).ToList();

           _file.SetTableStyle(workSheet, headers.Count);
           _file.SetHeaderStyle(workSheet, headers.Count);
           _file.InsertHeaders(headers, workSheet);
            Insert_AuditReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file. GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_AuditReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailyAuditTaskModel in dailyTaskModel.DailyAuditTasks)
                {
                    Action insertBasicColumns = () =>
                    {
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Date].Value =
                            DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                        workSheet.Cells[currentRowNumberForData, (int) AuditReportExceColumn.Outlet].Value = dailyAuditTaskModel.Outlet?.Name;
                        workSheet.Cells[currentRowNumberForData, (int) AuditReportExceColumn.OutletCode].Value = dailyAuditTaskModel.Outlet?.Code;
                        workSheet.Cells[currentRowNumberForData, (int) AuditReportExceColumn.Route].Value = dailyAuditTaskModel.Outlet?.Route?.RouteName;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                    };

                    if (!dailyAuditTaskModel.IsOutletOpen)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int) AuditReportExceColumn.Status].Value =
                            StatusOutletClosed;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Reason].Value =
                            StatusOutletClosed;
                        currentRowNumberForData++;
                        continue;
                    }

                    var displayStatus = this.StatusComplete;
                    var reason = "";
                    if (!dailyAuditTaskModel.IsCompleted)
                    {
                        displayStatus = this.StatusInComplete;
                        reason = dailyAuditTaskModel.Reason?.ReasonInEnglish;
                    }

                    var distributionChecked = dailyAuditTaskModel.DailyProductsAuditTask.Where(x => x.ActionType == ActionType.DistributionCheckProduct);

                    foreach (var item in distributionChecked)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Amount].Value = item.Result;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ActionType].Value = ActionType.DistributionCheckProduct.GetDescription();
                        if(item.Product is object) workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ProductName].Value = item.Product.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Reason].Value = reason;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Status].Value = displayStatus;

                        currentRowNumberForData++;
                    }

                    var priceAuditProducts = dailyAuditTaskModel.DailyProductsAuditTask.Where(x => x.ActionType == ActionType.PriceAuditProduct);

                    foreach (var item in priceAuditProducts)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Amount].Value = item.Result;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ActionType].Value = ActionType.PriceAuditProduct.GetDescription();
                        if (item.Product is object) workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ProductName].Value = item.Product.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Reason].Value = reason;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Status].Value = displayStatus;
                        currentRowNumberForData++;
                    }


                    var faceCountedProducts = dailyAuditTaskModel.DailyProductsAuditTask.Where(x => x.ActionType == ActionType.FacingCountProduct);

                    foreach (var item in faceCountedProducts)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Amount].Value = item.Result;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ActionType].Value = ActionType.FacingCountProduct.GetDescription();
                        if (item.POSMProduct is object) workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ProductName].Value = item.POSMProduct.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Reason].Value = reason;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Status].Value = displayStatus;
                        currentRowNumberForData++;
                    }

                    var planogramCheckedProducts = dailyAuditTaskModel.DailyProductsAuditTask.Where(x => x.ActionType == ActionType.PlanogramCheckProduct);

                    foreach (var item in planogramCheckedProducts)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Amount].Value = item.Result;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ActionType].Value = ActionType.PlanogramCheckProduct.GetDescription();
                        if (item.POSMProduct is object) workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.ProductName].Value = item.POSMProduct.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Reason].Value = reason;
                        workSheet.Cells[currentRowNumberForData, (int)AuditReportExceColumn.Status].Value = displayStatus;
                        currentRowNumberForData++;
                    }
                }
            }

            
        }

        private void Insert_SurveyReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailySurveyTask in dailyTaskModel.DailySurveyTasks)
                {
                    Action insertBasicColumns = () =>
                    {
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Date].Value =
                            DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Outlet].Value = dailySurveyTask.Outlet?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.OutletCode].Value = dailySurveyTask.Outlet?.Code;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Route].Value = dailySurveyTask.Outlet?.Route?.RouteName;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                        workSheet.Cells[currentRowNumberForData, (int) SurveyReportExceColumn.SurveyName].Value = dailySurveyTask.SurveyQuestionSet?.Name;
                    };

                    if (!dailySurveyTask.IsOutletOpen)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Status].Value =
                            StatusOutletClosed;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Reason].Value =
                            StatusOutletClosed;
                        currentRowNumberForData++;
                        continue;
                    }
                    if (!dailySurveyTask.IsCompleted)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Status].Value = StatusInComplete;
                        if (dailySurveyTask.Reason is object) workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Reason].Value =
                             dailySurveyTask.Reason.ReasonInEnglish;
                        currentRowNumberForData++;
                        continue;
                    }

                    foreach (var item in dailySurveyTask.DailySurveyTaskAnswers)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Answer].Value = item.Answer;
                        if(item.Question is object) workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Question].Value = item.Question.QuestionTitle;
                        workSheet.Cells[currentRowNumberForData, (int)SurveyReportExceColumn.Status].Value = this.StatusComplete;
                        currentRowNumberForData++;
                    }
                }
            }


        }

        public async Task<FileData> ExportSurveyReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetSurveyReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(SurveyReportExceColumn)).Cast<SurveyReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_SurveyReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_ConsumerSurveyReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailySurveyTask in dailyTaskModel.DailyConsumerSurveyTasks)
                {
                    Action insertBasicColumns = () =>
                    {
                        workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.Date].Value =
                            DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                        workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.SurveyName].Value = dailySurveyTask.SurveyQuestionSet?.Name;
                    };

                    foreach (var item in dailySurveyTask.DailyConsumerSurveyTaskAnswers)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.Answer].Value = item.Answer;
                        if (item.Question is object) workSheet.Cells[currentRowNumberForData, (int)ConsumerSurveyReportExceColumn.Question].Value = item.Question.QuestionTitle;
                        currentRowNumberForData++;
                    }
                }
            }


        }

        public async Task<FileData> ExportConsumerSurveyReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetConsumerSurveyReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(ConsumerSurveyReportExceColumn)).Cast<ConsumerSurveyReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_ConsumerSurveyReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_AVReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailyAVTask in dailyTaskModel.DailyAVTasks)
                {
                   
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Date].Value =
                            DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                        workSheet.Cells[currentRowNumberForData, (int) AvReportExceColumn.Outlet].Value = dailyAVTask.Outlet?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.OutletCode].Value = dailyAVTask.Outlet?.Code;
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Route].Value = dailyAVTask.Outlet?.Route?.RouteName;
                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.AVName].Value = dailyAVTask.AvSetup?.Av?.CampaignName;

                        if (!dailyAVTask.IsOutletOpen)
                        {
                            workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Status].Value =
                                StatusOutletClosed;
                            workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Reason].Value =
                                StatusOutletClosed;
                            currentRowNumberForData++;
                            continue;
                        }
                        if (!dailyAVTask.IsCompleted)
                        {
                            workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Status].Value = StatusInComplete;
                            if (dailyAVTask.Reason is object) workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Reason].Value =
                                dailyAVTask.Reason.ReasonInEnglish;
                            currentRowNumberForData++;
                            continue;
                        }

                        workSheet.Cells[currentRowNumberForData, (int)AvReportExceColumn.Status].Value = this.StatusComplete;
                        currentRowNumberForData++;


                }
            }


        }

        public async Task<FileData> ExportAvReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetAvReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(AvReportExceColumn)).Cast<AvReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_AVReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_CommunicationReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailyCommunicationTask in dailyTaskModel.DailyCommunicationTasks)
                {

                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Date].Value =
                        DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Outlet].Value = dailyCommunicationTask.Outlet?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.OutletCode].Value = dailyCommunicationTask.Outlet?.Code;
                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Route].Value = dailyCommunicationTask.Outlet?.Route?.RouteName;

                    if (dailyCommunicationTask.CommunicationSetup is object)
                        workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.CommunicationName].Value =
                            dailyCommunicationTask.CommunicationSetup.AvCommunication?.CampaignName;

                    if (!dailyCommunicationTask.IsOutletOpen)
                    {
                        workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Status].Value =
                            StatusOutletClosed;
                        workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Reason].Value =
                            StatusOutletClosed;
                        currentRowNumberForData++;
                        continue;
                    }
                    if (!dailyCommunicationTask.IsCompleted)
                    {
                        workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Status].Value = StatusInComplete;
                        if (dailyCommunicationTask.Reason is object) workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Reason].Value =
                            dailyCommunicationTask.Reason.ReasonInEnglish;
                        currentRowNumberForData++;
                        continue;
                    }

                    workSheet.Cells[currentRowNumberForData, (int)CommunicationReportExceColumn.Status].Value = this.StatusComplete;
                    currentRowNumberForData++;


                }
            }


        }

        public async Task<FileData> ExportCommunicationReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetCommunicationReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(CommunicationReportExceColumn)).Cast<CommunicationReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_CommunicationReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);


            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }


        private void Insert_InformationReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailyInformationTask in dailyTaskModel.DailyInformationTasks)
                {

                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Date].Value =
                        DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Outlet].Value = dailyInformationTask.Outlet?.Name;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.OutletCode].Value = dailyInformationTask.Outlet?.Code;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Route].Value = dailyInformationTask.Outlet?.Route?.RouteName;

                    if (dailyInformationTask.Outlet.Name == "Dulal store")
                    {
                        var test = 5;
                    }

                    if (!dailyInformationTask.IsOutletOpen)
                    {
                        workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Status].Value =
                            StatusOutletClosed;
                        workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Reason].Value =
                            StatusOutletClosed;
                        currentRowNumberForData++;
                        continue;
                    }
                    if (!dailyInformationTask.IsCompleted)
                    {
                        workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Status].Value = StatusInComplete;
                        if (dailyInformationTask.Reason is object) workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Reason].Value =
                            dailyInformationTask.Reason.ReasonInEnglish;
                        currentRowNumberForData++;
                        continue;
                    }

                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Insight].Value = dailyInformationTask.InsightDescription;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Request].Value = dailyInformationTask.RequestDescription;
                    workSheet.Cells[currentRowNumberForData, (int)InformationReportExceColumn.Status].Value = this.StatusComplete;
                    currentRowNumberForData++;


                }
            }


        }

        public async Task<FileData> ExportInformationReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetInformationReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(InformationReportExceColumn)).Cast<InformationReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_InformationReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            workSheet.Column((int)InformationReportExceColumn.Insight).Width = 50;
            workSheet.Column((int)InformationReportExceColumn.Insight).Style.WrapText = true;
            workSheet.Column((int)InformationReportExceColumn.Request).Width = 50;
            workSheet.Column((int)InformationReportExceColumn.Request).Style.WrapText = true;


            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_POSMTaskReportExcelRows(Pagination<DailyTaskModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var dailyTaskModel in result.Data)
            {
                foreach (var dailyPOSMTask in dailyTaskModel.DailyPosmTasks)
                {
                    Action insertBasicColumns = () =>
                    {
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Date].Value =
                            DateTime.Parse(dailyTaskModel.DateTimeStr).ToBangladeshTime().ToDisplayString();
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.CM_User].Value = dailyTaskModel.CmUser?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.SalesPoint].Value = dailyTaskModel.SalesPoint?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Outlet].Value = dailyPOSMTask.Outlet?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.OutletCode].Value = dailyPOSMTask.Outlet?.Code;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Route].Value = dailyPOSMTask.Outlet?.Route?.RouteName;
                    };

                    if (!dailyPOSMTask.IsOutletOpen)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Status].Value =
                            StatusOutletClosed;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Reason].Value =
                            StatusOutletClosed;
                        currentRowNumberForData++;
                        continue;
                    }
                    if (!dailyPOSMTask.IsCompleted)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Status].Value = StatusInComplete;
                        if (dailyPOSMTask.Reason is object) workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Reason].Value =
                            dailyPOSMTask.Reason.ReasonInEnglish;
                        currentRowNumberForData++;
                        continue;
                    }

                    foreach (var dailyPosmTaskItem in dailyPOSMTask.DailyPosmTaskItems)
                    {
                        insertBasicColumns();
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Amount].Value = dailyPosmTaskItem.Quantity;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.ProductName].Value = dailyPosmTaskItem.PosmProduct?.Name;
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.ActionType].Value = dailyPosmTaskItem.ExecutionType.GetDescription();
                        workSheet.Cells[currentRowNumberForData, (int)POSMTaskReportExceColumn.Status].Value = StatusComplete;
                        currentRowNumberForData++;
                    }
                }
            }


        }

        public async Task<FileData> ExportPOSMTaskReportToExcel(int queryParamsPageIndex, int queryParamsPageSize, string queryParamsSearch,
            DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime)
        {
            var result = await GetPOSMTaskReport(queryParamsPageIndex, queryParamsPageSize, queryParamsSearch, queryParamsFromDateTime,
                queryParamsToDateTime);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(POSMTaskReportExceColumn)).Cast<POSMTaskReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_POSMTaskReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void InsertCWStockUpdateExcelRows(List<CwStockUpdateModel> data, ExcelWorksheet worksheet)
        {
            int excelStartRow = 2;
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[excelStartRow + i, (int) CWStockUpdateColumn.Date].Value = data[i].Date.ToBangladeshTime().ToDisplayString();
                worksheet.Cells[excelStartRow + i, (int)CWStockUpdateColumn.POSMCode].Value = data[i].PosmProductCode;
                worksheet.Cells[excelStartRow + i, (int)CWStockUpdateColumn.POSMName].Value = data[i].PosmProductName;
                worksheet.Cells[excelStartRow + i, (int)CWStockUpdateColumn.Quantity].Value = data[i].Quantity;
                worksheet.Cells[excelStartRow + i, (int)CWStockUpdateColumn.Supplier].Value = data[i].Supplier;
                worksheet.Cells[excelStartRow + i, (int)CWStockUpdateColumn.WareHouseName].Value = data[i].WareHouseName;
            }
        }

        private async Task<List<CwStockUpdateModel>> GetCWStockUpdate(DateTime fromDateTime, DateTime toDateTime,
            List<int> cwIds)
        {
            var results =
                _stockAddTransaction.FindAllInclude(x => x.Status == Status.Active, x => x.Transaction,
                        posm => posm.PosmProduct)
                    .Include(x => x.Transaction.WareHouse)
                    .Where(x => x.Transaction.Status == Status.Active && x.Transaction.IsConfirmed
                                && x.Transaction.TransactionDate >= fromDateTime 
                                && x.Transaction.TransactionDate <= toDateTime
                                && x.Transaction.WarehouseId != null &&
                                cwIds.Contains((int) x.Transaction.WarehouseId))
                    .ToList()
                    .GroupBy(x => new
                        {
                        x.Supplier,
                        x.Transaction.WarehouseId,
                        x.PosmProductId,
                        x.Transaction.TransactionDate.Date
                    }).ToList()
                    .Select(x => new CwStockUpdateModel()
                        {
                        TransactionId = x.Select(y => y.TransactionId).FirstOrDefault(),
                        Date = x.Select(y => y.Transaction.TransactionDate).FirstOrDefault(),
                        PosmProductId = x.Key.PosmProductId,
                        PosmProductName = x.Select(y => y.PosmProduct.Name).FirstOrDefault(),
                        PosmProductCode = x.Select(y => y.PosmProduct.Code).FirstOrDefault(),
                        Quantity = x.Select(y => y.Quantity).Sum(),
                        Supplier = x.Key.Supplier,
                        WareHouseName = x.Select(y => y.Transaction.WareHouse.Name).FirstOrDefault()
                        }).ToList();

            return results;
        }

        private List<CWDistributionReportToExcelData> GetCWDistributionReportToExcelData(ExportCWDistributionReportToExcelModel payload)
        {
            var distributions = _transaction.GetAllActive().Where(x => x.TransactionType == TransactionType.Distribute &&
                x.IsConfirmed && x.TransactionDate >= payload.FromDateTime && x.TransactionDate <= payload.ToDateTime && x.SalesPointId != null &&
                payload.SalesPointIds.Contains((int)x.SalesPointId)).Include(x=>x.WareHouse).OrderByDescending(x=>x.TransactionDate).ToList().MapToModel();

            _common.InsertSalesPoints(distributions);

            var trIds = distributions.Select(x => x.Id).ToList();
            var items = _wDistributionTransaction.GetAllActive().Where(x => trIds.Contains(x.TransactionId))
                .Include(x => x.POSMProduct).ToList().MapToModel();
            distributions.ForEach(x => x.WDistributionTransactions = items.Where(item => item.TransactionId == x.Id).ToList());

            var recieves = _transaction.GetAllActive().Where(x => x.TransactionType == TransactionType.Receive && trIds.Contains(x.ReferenceTransactionId)).Include(x=>x.WareHouse).ToList().MapToModel();

            _common.InsertSalesPoints(recieves);


            var receivedtrIds = recieves.Select(x => x.Id).ToList();
            var receiveditems = _wDistributionRecieveTransaction.GetAllActive().Where(x => receivedtrIds.Contains(x.TransactionId))
                .Include(x => x.POSMProduct).ToList().MapToModel();
            var referenceTransactionIds = recieves.Select(x => x.ReferenceTransactionId).ToList();
            
            recieves.ForEach(x =>
            {
                x.WDistributionRecieveTransactions = receiveditems.Where(item => item.TransactionId == x.Id).ToList();
                x.ReferenceTransaction = distributions.FirstOrDefault(r => r.Id == x.ReferenceTransactionId);
            });

            List<CWDistributionReportToExcelData> result = new List<CWDistributionReportToExcelData>();

            var spNodeMappIds = _salesPointNodeMapping.GetAll().Where(x => payload.SalesPointIds.Contains(x.SalesPointId)).ToList();
            var teritoriIds = spNodeMappIds.Select(x => x.NodeId).ToList();

            var territories = _node.GetAllActive().Where(x => teritoriIds.Contains(x.NodeId)).ToList();

            List<NodeModel> nodes = _common.GetParentNodesBySalesPoint(payload.SalesPointIds);

            foreach (var distribution in distributions)
            {
                TransactionModel receivedTransaction = null;
                if (distribution.TransactionStatus == TransactionStatus.Completed)
                {
                    receivedTransaction =
                        recieves.FirstOrDefault(x => x.ReferenceTransactionId == distribution.Id);
                }
                foreach (var wDistributionTransaction in distribution.WDistributionTransactions)
                {
                    var row = new CWDistributionReportToExcelData
                    {
                        SalesPoint = distribution.SalesPoint?.Name,
                        Distribution = wDistributionTransaction.Quantity,
                        POSMName = wDistributionTransaction.POSMProductModel?.Name,
                        TransactionDate = distribution.TransactionDate.ToBangladeshTime().ToDisplayString(),
                        WareHouseName = distribution.WareHouseModel?.Name,
                    };

                    if (receivedTransaction is object)
                    {
                        var receivedItem = receivedTransaction.WDistributionRecieveTransactions.FirstOrDefault(x =>
                            x.POSMProductId == wDistributionTransaction.POSMProductId);
                        row.IsReceived = true;
                        row.ReceivedDate = receivedTransaction.TransactionDate.ToBangladeshTime().ToDisplayString();
                        if(receivedItem is object) row.ReceivedQuantity = receivedItem.RecievedQuantity;
                        row.Status = "Received";
                    }
                    else
                    {
                        row.Status = "In Transit";
                        row.Intransit = wDistributionTransaction.Quantity;

                    }

                    var territoryId = spNodeMappIds.FirstOrDefault(x => x.SalesPointId == distribution.SalesPointId)?.NodeId;
                    var territory = territories.FirstOrDefault(x => x.NodeId == territoryId);
                    row.Teritory = territory?.Name;
                    
                    var area = nodes.FirstOrDefault(x => x.NodeId == territory?.ParentId);
                    row.Area = area?.Name;

                    var region = nodes.FirstOrDefault(x => x.NodeId == area?.ParentId);
                    row.Region = region?.Name;
                    result.Add(row);
                }
                
            }

            result = result.ToList();

            return result;
        }

        public async Task<FileData> ExportCWStockUpdateToExcel(int queryParamsPageIndex, int queryParamsPageSize,
            string queryParamsSearch, DateTime queryParamsFromDateTime, DateTime queryParamsToDateTime,
            List<int> cwIdList)
        {
            var result = await GetCWStockUpdate(queryParamsFromDateTime, queryParamsToDateTime, cwIdList);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("CW Stock Update");
            List<string> headers = Enum.GetValues(typeof(CWStockUpdateColumn)).Cast<CWStockUpdateColumn>().Skip(1).Select(x => x.GetDescription()).ToList();
            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            InsertCWStockUpdateExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private void Insert_CWDistributionReportExcelRows(List<CWDistributionReportToExcelData> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;

            foreach (var row in result)
            {
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.TransactionDate].Value =  row.TransactionDate;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.CWName].Value = row.WareHouseName;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Region].Value = row.Region;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Area].Value = row.Area;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Teritory].Value = row.Teritory;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Salespoint].Value = row.SalesPoint;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.POSMItem].Value = row.POSMName;
                if (!row.IsReceived)
                {
                    workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Intransit].Value = row.Intransit;
                }

                if (row.IsReceived)
                {
                    workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.ReceivedDate].Value = row.ReceivedDate;
                    workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.ReceivedQuantity].Value = row.ReceivedQuantity;
                }
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Distribution].Value = row.Distribution;
                workSheet.Cells[currentRowNumberForData, (int)CWDistributionReportExceColumn.Status].Value = row.Status;
                currentRowNumberForData++;
            }


        }

        public async Task<FileData> ExportCWDistributionReportToExcel(ExportCWDistributionReportToExcelModel payload)
        {
            var result = GetCWDistributionReportToExcelData(payload);
            
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(CWDistributionReportExceColumn)).Cast<CWDistributionReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_CWDistributionReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }


        private List<WareHouseStockModel> GetWareHouseStocks(List<int> wareHouseIds)
        {
            var query = _wStock.GetAllActive();
            if (wareHouseIds.Any()) query = query.Where(x => wareHouseIds.Contains(x.WareHouseId));
            query = query.Include(x => x.POSMProduct)
                .Include(x=>x.WareHouse);
            var result = query.ToList().MapToModel();
            return result;
        }

        private void Insert_CWStockReportExcelRows(List<WareHouseStockModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            foreach (var stock in result)
            {
                workSheet.Cells[currentRowNumberForData, (int)CWStockReportExceColumn.Quantity].Value = stock.Quantity;
                workSheet.Cells[currentRowNumberForData, (int)CWStockReportExceColumn.POSMName].Value = stock.POSMProduct?.Name;
                workSheet.Cells[currentRowNumberForData, (int)CWStockReportExceColumn.POSMCode].Value = stock.POSMProduct?.Code;
                workSheet.Cells[currentRowNumberForData, (int)CWStockReportExceColumn.Warehouse].Value = stock.WareHouse?.Name;
                currentRowNumberForData++;
            }
        }

        public async Task<FileData> ExportCWStockReportToExcel(List<int> wareHouseIds)
        {
            var result = GetWareHouseStocks(wareHouseIds);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(CWStockReportExceColumn)).Cast<CWStockReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_CWStockReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        private async Task<List<SalesPointStockModel>> GetSalesPointStocks(List<int> spIds)
        {
            var query = _spStock.GetAllActive();
            if (spIds.Any()) query = query.Where(x => spIds.Contains(x.SalesPointId));
            query = query.Include(x => x.POSMProduct);
            var result = await query.OrderByDescending(x => x.CreatedTime).ToListAsync();
            var resultList = result.MapToModel();
            _common.InsertSalesPoints(resultList);
            _common.InsertAvalableSpQuantity(resultList);
            return resultList;
        }

        private void Insert_SPStockReportExcelRows(List<SalesPointStockModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            foreach (var stock in result)
            {
                workSheet.Cells[currentRowNumberForData, (int)SPStockReportExceColumn.Quantity].Value = stock.Quantity;
                workSheet.Cells[currentRowNumberForData, (int)SPStockReportExceColumn.AvailableQuantity].Value = stock.AvailableQuantity;
                workSheet.Cells[currentRowNumberForData, (int)SPStockReportExceColumn.POSMName].Value = stock.POSMProduct?.Name;
                workSheet.Cells[currentRowNumberForData, (int)SPStockReportExceColumn.POSMCode].Value = stock.POSMProduct?.Code;
                workSheet.Cells[currentRowNumberForData, (int)SPStockReportExceColumn.Salespoint].Value = stock.SalesPoint?.Name;
                currentRowNumberForData++;
            }
        }

        public async Task<FileData> ExportSPStockReportToExcel(List<int> spIds)
        {
            var result = await GetSalesPointStocks(spIds);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(SPStockReportExceColumn)).Cast<SPStockReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_SPStockReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }


        private void Insert_SPWisePosmReportExcelRows(List<SPWisePOSMLedgerModel> result, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            var salesPointIds = result.Select(x => x.SalesPointId).ToList();
            var salespoints = _salesPoint.GetAllActive().Where(x => salesPointIds.Contains(x.SalesPointId)).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x=> salesPointIds.Contains(x.SalesPointId)).ToList();

            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIds);
            foreach (var ledger in result)
            {
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.Date].Value = ledger.Date.ToBangladeshTime().ToDisplayString();
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.Salespoint].Value =  ledger.SalesPoint?.Name;
                var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == ledger.SalesPointId)?.NodeId;
                var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.Teritory].Value = teritory?.Name;
                var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.Area].Value = area?.Name;
                var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.Region].Value = region?.Name;
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.ClosingStock].Value = ledger.ClosingStock;
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.ReceivedStock].Value = ledger.ReceivedStock;
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.POSMItem].Value = ledger.PosmProduct?.Name;
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.ExecutedStock].Value = ledger.ExecutedStock;
                workSheet.Cells[currentRowNumberForData, (int)SPWisePosmLedgerReportExceColumn.OpeningStock].Value = ledger.OpeningStock;

                currentRowNumberForData++;
            }
        }

        private async Task<List<SPWisePOSMLedgerModel>> GetspWisePosmLedgers(ExportSPWisePosmLedgerPayload payload)
        {
            var result = await _spWisePOSMLedger.GetAllActive().Where(x =>
                payload.PosmProductIds.Contains(x.PosmProductId) && payload.SalesPointIds.Contains(x.SalesPointId) &&
                payload.FromDateTime <= x.Date && x.Date <= payload.ToDateTime).Include(x => x.PosmProduct).ToListAsync();
            var models = result.MapToModel();

            _common.InsertSalesPoints(models);
            return models;
        }

        private async Task<List<SPWisePOSMLedgerModel>> GetspWisePosmLedgers2(ExportSPWisePosmLedgerPayload payload)
        {

            var posmReceivedTransactions = _transaction.GetAllActive().Where(x =>
                x.TransactionType == TransactionType.Receive &&
                payload.SalesPointIds.Contains(x.SalesPointId)).
                Include(x=>x.WDistributionRecieveTransactions).ToList().Where(x=> x.TransactionDate.BangladeshDateInUtc() <= payload.ToDateTime).ToList();

            posmReceivedTransactions.ForEach(x=>x.WDistributionRecieveTransactions = x.WDistributionRecieveTransactions.Where(rcTr=> payload.PosmProductIds.Contains(rcTr.POSMProductId)).ToList() );

            var spReceives = await _spReceivedTransfers.GetAllActive().Where(x =>
                  payload.SalesPointIds.Contains(x.ToSalesPointId)).ToListAsync();
            spReceives = spReceives.Where(x => x.TransactionDate.BangladeshDateInUtc() <= payload.ToDateTime).ToList();

            var spReceiveIds = spReceives.Select(x => x.Id).ToList();

            var spReceivedItems = _spReceivedTransferItems.GetAllActive()
                .Where(x => spReceiveIds.Contains(x.TransferId) && payload.PosmProductIds.Contains(x.POSMProductId)).Include(x => x.POSMProduct).ToList();

            spReceives.ForEach(x=>x.Items = spReceivedItems.Where(item=> item.TransferId == x.Id).ToList());

            var posmTasks =  _dailyTask.GetAllActive().Where(x => x.DateTime <= payload.ToDateTime &&
                                                                  payload.SalesPointIds.Contains(x.SalesPointId)).Include(x=>x.DailyPosmTasks)
                .ThenInclude(x=>x.DailyPosmTaskItems).ThenInclude(x=>x.PosmProduct).ToList();
            posmTasks.SelectMany(x => x.DailyPosmTasks).ToList().ForEach(pt =>
                pt.DailyPosmTaskItems = pt.DailyPosmTaskItems.Where(x => payload.PosmProductIds.Contains(x.PosmProductId)).ToList());

            var spStocks = _spStock.GetAllActive().Where(x => payload.SalesPointIds.Contains(x.SalesPointId) && payload.PosmProductIds.Contains(x.POSMProductId))
                .Include(x => x.POSMProduct).ToList();

            var records = spStocks.Select(x => new SPWisePOSMLedger()
            {
                SalesPointId = x.SalesPointId,
                PosmProductId = x.POSMProductId,
                Date = payload.FromDateTime,
                PosmProduct = x.POSMProduct,
            }).ToList();

            foreach (var ledger in records)
            {
                var posmReceivedTillFromDate = posmReceivedTransactions.Where(x => x.TransactionDate < payload.FromDateTime && x.SalesPointId == ledger.SalesPointId)
                    .SelectMany(x => x.WDistributionRecieveTransactions)
                    .Where(x => x.POSMProductId == ledger.PosmProductId).Sum(x => x.RecievedQuantity);
                
                var spReceivedTillFromDate = spReceives.Where(x => x.TransactionDate < payload.FromDateTime && x.ToSalesPointId == ledger.SalesPointId)
                    .SelectMany(x => x.Items).Where(x => x.POSMProductId == ledger.PosmProductId)
                    .Sum(x => x.ReceivedQuantity);

                var executedTillFromDate = posmTasks.Where(x => x.DateTime < payload.FromDateTime && x.SalesPointId == ledger.SalesPointId)
                    .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                    .Where(x => x.PosmProductId == ledger.PosmProductId).Sum(x => x.Quantity);

                ledger.OpeningStock = posmReceivedTillFromDate + spReceivedTillFromDate - executedTillFromDate;
                if (ledger.OpeningStock < 0) ledger.OpeningStock = 0;

                var currentPosmReceivedReceived = posmReceivedTransactions
                    .Where(x => x.TransactionDate.BangladeshDateInUtc() == payload.FromDateTime &&
                                x.SalesPointId == ledger.SalesPointId)
                    .SelectMany(x => x.WDistributionRecieveTransactions)
                    .Where(x => x.POSMProductId == ledger.PosmProductId).Sum(x => x.RecievedQuantity);

                var currentSpReceivedReceived = spReceives.Where(x =>
                        x.ToSalesPointId == ledger.SalesPointId && x.TransactionDate.BangladeshDateInUtc() == payload.FromDateTime)
                    .SelectMany(x => x.Items).Where(x => x.POSMProductId == ledger.PosmProductId)
                    .Sum(x => x.ReceivedQuantity);

                ledger.ReceivedStock = currentSpReceivedReceived + currentPosmReceivedReceived;

                ledger.ExecutedStock = posmTasks
                    .Where(x => x.SalesPointId == ledger.SalesPointId && x.DateTime.BangladeshDateInUtc() == payload.FromDateTime)
                    .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                    .Where(x => x.PosmProductId == ledger.PosmProductId).Sum(x => x.Quantity);

                
                ledger.ClosingStock = ledger.OpeningStock + ledger.ExecutedStock - ledger.ExecutedStock;
                if (ledger.ClosingStock < 0) ledger.ClosingStock = 0;

            }

            var previousRecords = records;

            for (var date = payload.FromDateTime.AddDays(1) ; date <= payload.ToDateTime; date = date.AddDays(1))
            {

                var newRecords = new List<SPWisePOSMLedger>();
                foreach (var previeousRecord in previousRecords)
                {
                    var record = new SPWisePOSMLedger()
                    {
                        Date = date,
                        SalesPointId = previeousRecord.SalesPointId,
                        PosmProductId = previeousRecord.PosmProductId,
                        OpeningStock = previeousRecord.ClosingStock,
                        PosmProduct = previeousRecord.PosmProduct,
                    };

                    var posmReceived = posmReceivedTransactions
                        .Where(x => x.TransactionDate.BangladeshDateInUtc() == date &&
                                    x.SalesPointId == record.SalesPointId)
                        .SelectMany(x => x.WDistributionRecieveTransactions)
                        .Where(x => x.POSMProductId == record.PosmProductId).Sum(x => x.RecievedQuantity);

                    var spReceived = spReceives.Where(x =>
                            x.ToSalesPointId == record.SalesPointId && x.TransactionDate.BangladeshDateInUtc() == date)
                        .SelectMany(x => x.Items).Where(x => x.POSMProductId == record.PosmProductId)
                        .Sum(x => x.ReceivedQuantity);

                    record.ReceivedStock = posmReceived + spReceived;

                    record.ExecutedStock = posmTasks
                        .Where(x => x.SalesPointId == record.SalesPointId && x.DateTime.BangladeshDateInUtc() == date)
                        .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                        .Where(x => x.PosmProductId == record.PosmProductId).Sum(x => x.Quantity);

                    record.ClosingStock = record.OpeningStock + record.ReceivedStock - record.ExecutedStock;

                    if (record.ClosingStock < 0) record.ClosingStock = 0;

                    newRecords.Add(record);
                }

                records.AddRange(newRecords);
                previousRecords = newRecords;
            }

            var result = records.MapToModel();
            _common.InsertSalesPoints(result);
            return result;
        }

        public async Task<FileData> ExportSPWisePosmLedgerReportToExcel2(ExportSPWisePosmLedgerPayload payload)
        {
            var result = await GetspWisePosmLedgers2(payload);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(SPWisePosmLedgerReportExceColumn)).Cast<SPWisePosmLedgerReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_SPWisePosmReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }

        public async Task<FileData> ExportSPWisePosmLedgerReportToExcel(ExportSPWisePosmLedgerPayload payload)
        {
            var result = await GetspWisePosmLedgers(payload);

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = Enum.GetValues(typeof(SPWisePosmLedgerReportExceColumn)).Cast<SPWisePosmLedgerReportExceColumn>().Skip(1).Select(x => x.GetDescription()).ToList();

            _file.SetTableStyle(workSheet, headers.Count);
            _file.SetHeaderStyle(workSheet, headers.Count);
            _file.InsertHeaders(headers, workSheet);
            Insert_SPWisePosmReportExcelRows(result, workSheet);
            _file.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = _file.GetFileData(excel.GetAsByteArray());
            excel.Dispose();
            return fileData;
        }
    }
}
