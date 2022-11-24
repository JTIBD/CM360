using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Nodes;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Bases;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Task;
using FMAplication.Domain.WareHouse;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using X.PagedList;

namespace FMAplication.Services.Common.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<UserTerritoryMapping> _userTerritoryMapping;
        private readonly IRepository<Node> _node;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNodeMapping;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<UserInfo> _userInfo;
        private readonly IRepository<Hierarchy> _hierarchy;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<WareHouseTransfer> _wareHouseTransfer;
        private readonly IRepository<WareHouseReceivedTransfer> _wareHouseReceivedTransfer;
        private readonly IRepository<SalesPointTransfer> _salesPointTransfer;
        private readonly IRepository<SalesPointReceivedTransfer> _salesPointReceivedTransfer;
        private readonly IRepository<Outlet> _outlet;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<DailyPosmTaskItems> _dailyPosmTaskItem;
        private readonly IRepository<PosmTaskAssign> _posmTaskAssign;
        private readonly IRepository<SalesPointStock> _salesPointStock;
        private readonly IRepository<Route> _route;


        public CommonService(IRepository<UserTerritoryMapping> userTerritoryMapping, 
            IRepository<Node> node, IRepository<SalesPointNodeMapping> salesPointNodeMapping, 
            IRepository<SalesPoint> salesPoint, IRepository<UserInfo> userInfo, 
            IRepository<Hierarchy> hierarchy, IRepository<Transaction> transaction, 
            IRepository<WareHouseTransfer> wareHouseTransfer, IRepository<WareHouseReceivedTransfer> wareHouseReceivedTransfer, 
            IRepository<SalesPointTransfer> salesPointTransfer, IRepository<SalesPointReceivedTransfer> salesPointReceivedTransfer, IRepository<Outlet> outlet, 
            IRepository<DailyTask> dailyTask, IRepository<DailyPosmTask> dailyPosmTask, IRepository<DailyPosmTaskItems> dailyPosmTaskItem, IRepository<PosmTaskAssign> posmTaskAssign, IRepository<SalesPointStock> salesPointStock, IRepository<Route> route)
        {
            _userTerritoryMapping = userTerritoryMapping;
            _node = node;
            _salesPointNodeMapping = salesPointNodeMapping;
            _salesPoint = salesPoint;
            _userInfo = userInfo;
            _hierarchy = hierarchy;
            _transaction = transaction;
            _wareHouseTransfer = wareHouseTransfer;
            _wareHouseReceivedTransfer = wareHouseReceivedTransfer;
            _salesPointTransfer = salesPointTransfer;
            _salesPointReceivedTransfer = salesPointReceivedTransfer;
            _outlet = outlet;
            _dailyTask = dailyTask;
            _dailyPosmTask = dailyPosmTask;
            _dailyPosmTaskItem = dailyPosmTaskItem;
            _posmTaskAssign = posmTaskAssign;
            _salesPointStock = salesPointStock;
            _route = route;
        }
        public List<SalesPoint> GetSalesPointsByFMUser(int userId)
        {
            var nodeIds = GetNodeIdsByFmUser(userId);
            var salesPointNodeMappings = _salesPointNodeMapping.FindAll(x => nodeIds.Any(id => id == x.NodeId)).ToList();
            var salesPointIds = salesPointNodeMappings.Select(x => x.SalesPointId).ToList();
            List<SalesPoint> salesPoints = _salesPoint.FindAll(x => salesPointIds.Any(sId => sId == x.SalesPointId)).ToList();
            return salesPoints;
        }

        public List<int> GetNodeIdsByFmUser(int userId)
        {
            var nodeIds = _userTerritoryMapping.FindAll(x => x.UserInfoId == userId).Select(x => x.NodeId).ToList();
            var desc = GetDescendantNodeIds(nodeIds);
            nodeIds.AddRange(desc);
            nodeIds = nodeIds.Distinct().ToList();
            return nodeIds;
        }
        public List<int> GetDescendantNodeIds(List<int> nodeIds)
        {
            List<int> allDescendantsNodeIds = new List<int>();
            List<int> childNodeIds = _node.FindAll(n => n.ParentId != null && nodeIds.Any(id => id == n.ParentId.Value)).Select(x => x.NodeId).ToList();
            if (childNodeIds.Count > 0)
            {
                allDescendantsNodeIds.AddRange(childNodeIds);
                var descendants = GetDescendantNodeIds(childNodeIds);
                allDescendantsNodeIds.AddRange(descendants);
            }
            return allDescendantsNodeIds;
        }

        public List<int> GetParentNodeIds(List<int> nodeIds)
        {
            List<int> allDescendantsNodeIds = new List<int>();
            List<Node> parentNodes = _node.FindAll(n => nodeIds.Contains(n.NodeId)).ToList();
            var nationalNode = parentNodes.FirstOrDefault(x => x.ParentId == null);
            List<int> parentNodeIds = parentNodes.Where(x => x.ParentId != null).Select(x => x.ParentId.Value).ToList();
            if (parentNodeIds.Count > 0)
            {
                allDescendantsNodeIds.AddRange(parentNodeIds);
                var descendants = GetParentNodeIds(parentNodeIds);
                allDescendantsNodeIds.AddRange(descendants);
            }
            if (nationalNode != null)
                allDescendantsNodeIds.Add(nationalNode.NodeId);
            return allDescendantsNodeIds;
        }

        public List<NodeHierarchy> GetNodeTreeByFmUser(int userId)
        {
            UserInfo user = _userInfo.Find(x => x.Id == userId);
            var hierchy = _hierarchy.GetAllActive().FirstOrDefault(x => x.Id == user.HierarchyId);
            
            var nodeIds = _userTerritoryMapping.FindAll(x => x.UserInfoId == userId).Select(x => x.NodeId).ToList();
            List<NodeHierarchy> nodeHierarchies = GetNodeWithHierarchies(nodeIds);
            nodeHierarchies.ForEach(nd=>nd.HierarchyCode = hierchy?.Code);
            SetDescenantHierarchies(nodeHierarchies);

            var areas = nodeHierarchies.SelectMany(x => x.Nodes).SelectMany(x => x.Nodes).ToList();

            foreach (var area in areas)
            {
                area.Nodes = area.Nodes.Where(n => n.SalesPoints?.Any() == true).ToList();
            }

            return nodeHierarchies;
        }

        public List<NodeHierarchy> GetNodeWithHierarchies(List<int> nodeIds)
        {
            List<NodeHierarchy> nodeHierarchies = new List<NodeHierarchy>();

            var Nodes = _node.FindAll(x => nodeIds.Any(id => x.NodeId == id)).ToList();

            nodeHierarchies = Nodes.Select(n => new NodeHierarchy()
            {
                Node = n
            }).ToList();
            return nodeHierarchies;
        }

        public void SetDescenantHierarchies(List<NodeHierarchy> nodeHierarchies)
        {
            var nodeIds = nodeHierarchies.Select(x => x.Node.NodeId).ToList();
            var descendantNodes = _node.FindAll(x => x.ParentId != null && nodeIds.Contains((int)x.ParentId)).ToList();
            var descandantNodeIds = descendantNodes.Select(x => x.NodeId).ToList();
            
            List<NodeHierarchy> descendantHierarchies = GetNodeWithHierarchies(descandantNodeIds);

            var spMaps = _salesPointNodeMapping.FindAll(x => nodeIds.Contains(x.NodeId)).ToList();
            var sids = spMaps.Select(x => x.SalesPointId).ToList();
            var salesPoints = _salesPoint.FindAll(x => sids.Contains(x.SalesPointId)).ToList();

            foreach (var nodeHierarchy in nodeHierarchies)
            {
                nodeHierarchy.Nodes = descendantHierarchies.FindAll(x => x.Node.ParentId != null && x.Node.ParentId == nodeHierarchy.Node.NodeId);
                if(nodeHierarchy.Nodes.Count == 0)
                {
                    var nodeId = nodeHierarchy.Node.NodeId;
                    var ids = spMaps.FindAll(x => x.NodeId == nodeId).Select(s=>s.SalesPointId).ToList();
                    nodeHierarchy.SalesPoints = salesPoints.FindAll(x => ids.Contains(x.SalesPointId)).ToList();
                }                
            }

            if(descendantHierarchies.Count > 0) SetDescenantHierarchies(descendantHierarchies);

        }
        public void ValidateDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate >= toDate) throw new AppException("Start date cannot be later or equal to end date");
            if (toDate < DateTime.UtcNow) throw new AppException("End date cannot be passed date");
        }

        public async Task<string> GetTransactionNumber(TransactionType type, string sourceEntityCode)
        {
            int id = 0;
            var preFix = GetPrefixForTransactionType(type);
            var data = await _transaction.GetAll().Where(x => x.TransactionType == type && x.TransactionDate.Date == DateTime.UtcNow.Date).ToListAsync();
            id = data.OrderBy(x => x).Count() + 1;
            return $"{preFix}-{sourceEntityCode}-{DateTime.UtcNow.ToBangladeshTime():yyyyMMdd}-{id.ToString("D4")}";
        }

        public  int GetTransactionCount(TransactionType type)
        {
            if(type == TransactionType.CW_Transfer) return _wareHouseTransfer.GetAll().Count(x => x.TransactionDate.Date == DateTime.UtcNow.Date);
            if (type == TransactionType.CW_Receive) return _wareHouseReceivedTransfer.GetAll().Count(x => x.TransactionDate.Date == DateTime.UtcNow.Date);
            if (type == TransactionType.SP_Transfer) return _salesPointTransfer.GetAll().Count(x => x.TransactionDate.Date == DateTime.UtcNow.Date);
            if (type == TransactionType.SP_Receive) return _salesPointReceivedTransfer.GetAll().Count(x => x.TransactionDate.Date == DateTime.UtcNow.Date);
            return _transaction.GetAll().Count(x => x.TransactionType == type && x.TransactionDate.Date == DateTime.UtcNow.Date);
        }

        public async Task<string> GetTransactionNumber(TransactionType type, string sourceEntityCode, string destinationEntityCode, int serial = -1)
        {
            var preFix = GetPrefixForTransactionType(type);
            if (serial == -1)
            {
                var data = await _transaction.GetAll().Where(x => x.TransactionType == type && x.TransactionDate.Date == DateTime.UtcNow.Date).ToListAsync();
                serial = data.Count() + 1;
            }
            return $"{preFix}-{sourceEntityCode}-{destinationEntityCode}-{DateTime.UtcNow.ToBangladeshTime():yyyyMMdd}-{serial.ToString("D4")}";
        }

        public async Task<List<T>> GetExistingSetups<T>(IRepository<T> repository, List<BaseSetupModel> setups,bool checkViolation = true) where T  :BaseSetup
        {
            if (setups is null || setups.Count == 0) throw new AppException("No data provided");
            DateTime FromDate = setups[0].FromDate;
            DateTime ToDate = setups[0].ToDate;
            if (setups[0].Status != Status.InActive) this.ValidateDateRange(FromDate, ToDate);
            var userType = setups[0].UserType;
            var userTypes = new List<AssignedUserType>() {userType};
            if (userType == AssignedUserType.BOTH)
            {
                userTypes.Add(AssignedUserType.CMR);
                userTypes.Add(AssignedUserType.TMS);
            }
            else
            {
                userTypes.Add(AssignedUserType.BOTH);
            }


            var salesPointIds = setups.Select(sv => sv.SalesPointId).ToList();
            var existingSetups = await repository.GetAllActive().Where(x =>
                    userTypes.Contains(x.UserType) && salesPointIds.Contains(x.SalesPointId))
                .ToListAsync();
            existingSetups = existingSetups.Where(x => IsOverlapping(FromDate, ToDate, x.FromDate, x.ToDate)).ToList();

            if (setups.Count == 1 && setups[0].Id != 0)
            {
                existingSetups = existingSetups.Where(x => x.Id != setups[0].Id).ToList();
            }

            if (checkViolation)
            {
                var salesPointIdOfExistingSetups = existingSetups.Select(x => x.SalesPointId).ToList();
                var salesPointsOfExistingSetups = _salesPoint.GetAllActive()
                    .Where(x => salesPointIdOfExistingSetups.Contains(x.SalesPointId)).ToList();
                if (existingSetups.Any()) HandleConflictingCase(FromDate, ToDate, existingSetups, salesPointsOfExistingSetups);
                //HandleUserTypeValidation(userType, existingSetups, salesPointsOfExistingSetups);
            }
            return existingSetups;
        }

        public void HandleUserTypeValidation<T>(AssignedUserType userType, List<T> existingSetups,
            List<SalesPoint> salesPointsOfExistingSetups) where T : BaseSetup
        {
            if (userType != AssignedUserType.BOTH)
            {
                var violations = existingSetups.Where(x => x.UserType == AssignedUserType.BOTH).ToList();
                if (violations.Any())
                {
                    var sIds = violations.Select(x => x.SalesPointId).ToList();
                    var sps = salesPointsOfExistingSetups.Where(x => sIds.Contains(x.SalesPointId)).ToList();
                    var spNameStr = string.Join(",", sps.Select(x => x.Name));
                    throw new AppException($"Setup exist for both user type in salespoint {spNameStr}");
                }
            }
        }

        public void HandleConflictingCase<T>(DateTime fromDateTime, DateTime toDateTime, List<T> existingSetups,List<SalesPoint>sps) where T:BaseSetup
        {

            var bdToday = DateTime.UtcNow.BangladeshDateInUtc();
            if (fromDateTime.Date == bdToday.Date)
            {
                var setupsOfToday = existingSetups.Where(x => x.FromDate <= bdToday && bdToday <= x.ToDate).ToList();
                if (setupsOfToday.Any())
                {
                    var spOfSetupsOfToday =
                    sps.Where(x => setupsOfToday.Any(setup => setup.SalesPointId == x.SalesPointId)).ToList();
                    var spNameStr = string.Join(",", spOfSetupsOfToday.Select(x => x.Name));

                    throw new AppException($"Cannot set start date today while setup already exist in {spNameStr}");
                }
                
            }
                

            var spNamesOfSetupsStartedAfterNew = sps
                .Where(x => existingSetups.Any(existing => existing.SalesPointId == x.SalesPointId && existing.FromDate >= fromDateTime) )
                .Select(x => x.Name).ToList();
            if(spNamesOfSetupsStartedAfterNew.Any())
            {
                var spNamesStrOfSetupsStartedAfterNew = string.Join(",", spNamesOfSetupsStartedAfterNew);
                throw new AppException(
                    $"Start date cannot before or equal existing setups in {spNamesStrOfSetupsStartedAfterNew}");
            }

        }
        public void InsertOutlets<T>(List<T> list) where T : IWithOutlet
        {
            var outletIds = list.Select(x => x.OutletId).ToList();
            var outlets = _outlet.GetAllActive().Where(x => outletIds.Contains(x.OutletId)).ToList();
            var models = outlets.ToMap<Outlet, OutletModel>();
            list.ForEach(x=>x.Outlet = models.Find(m=>m.OutletId == x.OutletId));
        }

        public void InsertRoutesOptional<T>(List<T> list) where T : IWithRouteOptional
        {
            var routeIds = list.Where(x=>x.RouteId != null).Select(x => x.RouteId.Value).ToList();
            var routes = _route.GetAllActive().Where(x => routeIds.Contains(x.RouteId)).ToList();
            var models = routes.ToMap<Route, RouteModel>();
            list.ForEach(x => x.Route = models.Find(m => m.RouteId == x.RouteId));
        }

        public void InsertAvalableSpQuantity(List<SalesPointStockModel> spStocks)
        {
            var spIds = spStocks.Select(x => x.SalesPointId).ToList();

            var spTransfers = _salesPointTransfer.GetAllActive().Where(x =>
                spIds.Contains(x.FromSalesPointId) && x.IsConfirmed &&
                x.TransactionStatus != TransactionStatus.Completed).Include(x => x.SalesPointTransferItems)
                .ToList();

            var executionTypes = new List<PosmWorkType>()
                {PosmWorkType.Installation, PosmWorkType.RemovalAndReInstallation};
            var today = DateTime.UtcNow.BangladeshDateInUtc();
            var posmTasks = _dailyTask.GetAllActive()
                .Where(x => !x.IsSubmitted && spIds.Contains(x.SalesPointId) && x.DateTime.Date < today.Date && 
                            x.DateTime.Date >= today.Date.AddDays(-2) && 
                            x.DailyPosmTasks.Any(dp=> dp.DailyPosmTaskItems.Any(dpi=> executionTypes.Contains(dpi.ExecutionType))) 
                            && x.DateTime.Date.AddDays(2) <= today.Date)
                            .Include(x => x.DailyPosmTasks).ThenInclude(x=>x.DailyPosmTaskItems)
                            .ToList();
            posmTasks.ForEach(x=>x.DailyPosmTasks.ForEach(dp=>dp.DailyPosmTaskItems = dp.DailyPosmTaskItems.Where(dpi=>executionTypes.Contains(dpi.ExecutionType)).ToList()));

            var assignedPosms = _posmTaskAssign.GetAllActive()
                .Where(x => spIds.Contains(x.SalesPointId) && x.Date.Date >= today.Date).ToList();


            foreach (var spStock in spStocks)
            {
                var bookedSpTransferQuantity = spTransfers.Where(x => x.FromSalesPointId == spStock.SalesPointId)
                    .SelectMany(x => x.SalesPointTransferItems)
                    .Where(x => x.POSMProductId == spStock.POSMProductId).Select(x=>x.Quantity).Sum();

                var bookedPosmTasks = posmTasks.Where(x => x.SalesPointId == spStock.SalesPointId)
                    .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                    .Where(x => x.PosmProductId == spStock.POSMProductId).Select(x => x.Quantity).Sum();

                var posmAssinged = assignedPosms
                    .Where(x => x.SalesPointId == spStock.SalesPointId && x.PosmProductId == spStock.POSMProductId)
                    .Select(x => x.Quantity).Sum();

                var totalBooked = bookedSpTransferQuantity + bookedPosmTasks + posmAssinged;
                spStock.AvailableQuantity = spStock.Quantity -  totalBooked;
                if (spStock.AvailableQuantity < 0) spStock.AvailableQuantity = 0;
            }


        }

        public void CheckSpAvailableQuantityViolationForSPAdjustment(int transactionId)
        {
            var transaction =  _transaction.GetAllActive().
                Where(x => x.Id == transactionId && x.TransactionType == TransactionType.SalesPointStockAdjustment)
                .Include(x => x.SalesPointAdjustmentItems).ThenInclude(x => x.PosmProduct).FirstOrDefault()?.MapToModel();
            if (transaction == null) throw new AppException("Transaction not found");

            var posmIds = transaction.SalesPointAdjustmentItems.Select(x => x.PosmProductId).ToList();
            var stocks = _salesPointStock.GetAllActive()
                .Where(x => x.SalesPointId == transaction.SalesPointId && posmIds.Contains(x.POSMProductId)).ToList().MapToModel();
            InsertAvalableSpQuantity(stocks);
            var violatedStock = transaction.SalesPointAdjustmentItems.Where(x =>
                    stocks.Any(st => st.POSMProductId == x.PosmProductId && st.Quantity - x.AdjustedQuantity > st.AvailableQuantity))
                .ToList();
            if (violatedStock.Any())
            {
                var posmnames = string.Join(",", violatedStock.Select(x => x.PosmProduct.Name));
                throw new AppException($"Decreased quantity exceeds available quantity for {posmnames}");
            }
        }

        public void CheckSpTransferAvailableQuantityViolation(int transactionId)
        {
            var transaction = _salesPointTransfer.GetAllActive().Where(x => x.Id == transactionId)
                .Include(x => x.SalesPointTransferItems).ThenInclude(x => x.POSMProduct).FirstOrDefault()?.MapToModel();
            var posmIds = transaction.Items.Select(x => x.POSMProductId).ToList();
            var stocks = _salesPointStock.GetAllActive()
                .Where(x => x.SalesPointId == transaction.FromSalesPointId && posmIds.Contains(x.POSMProductId)).ToList().MapToModel();
            InsertAvalableSpQuantity(stocks);
            var violatedStock = transaction.Items.Where(x =>
                    stocks.Any(st =>
                    {
                        var availableQuantity = st.AvailableQuantity;
                        if (transaction.IsConfirmed) availableQuantity += x.Quantity;
                        return st.POSMProductId == x.POSMProductId && availableQuantity < x.Quantity;
                    }))
                .ToList();
            if (violatedStock.Any())
            {
                var posmnames = string.Join(",", violatedStock.Select(x => x.POSMProduct.Name));
                throw new AppException($"Quantity quantity is less than available quantity for {posmnames}");
            }
        }

        public void InsertSalesPoints<T>(List<T> list) where T : IWithSalesPoint
        {
            var salesPointIds = list.Select(x => x.SalesPointId).ToList();
            var outlets = _salesPoint.GetAllActive().Where(x => salesPointIds.Contains(x.SalesPointId)).ToList();
            var models = outlets.ToMap<SalesPoint, SalesPointModel>();
            list.ForEach(x => x.SalesPoint = models.Find(m => m.SalesPointId == x.SalesPointId));
        }

        public List<NodeModel> GetParentNodesBySalesPoint(List<int> salesPointIds)
        {
            var nodeIds = _salesPointNodeMapping.GetAll().Where(x => salesPointIds.Contains(x.SalesPointId)).Select(x=>x.NodeId).ToList();
            var parentNodeIds =  GetParentNodeIds(nodeIds);
            nodeIds.AddRange(parentNodeIds);
            var nodes = _node.GetAllActive().Where(x => nodeIds.Contains(x.NodeId)).ToList().ToMap<Node, NodeModel>();
            return nodes;

        }

        public bool IsOverlapping(DateTime from1,DateTime to1, DateTime from2, DateTime to2)
        {
            if (from2 <= from1 && from1 <= to2) return true;
            if (from2 <= to1 && to1 <= to2) return true;
            if (from1 <= from2 && from2 <= to1) return true;
            if (from1 <= to2 && to2 <= to1) return true;
            if (from1 < from2 && to1 > to2) return true;
            if (from2 < from1 && to2 > to1) return true;
            return false;
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
                case TransactionType.CW_Transfer:
                    result = "CWTR";
                    break;
                case TransactionType.CW_Receive:
                    result = "CWRC";
                    break;
                case TransactionType.SP_Transfer:
                    result = "SPTR";
                    break;
                case TransactionType.SP_Receive:
                    result = "SPRC";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public async Task<int> GetPosmHoldingAmount(int posmProductId, int salespointId)
        {
            var dayBeforeYesterday = DateTime.UtcNow.AddDays(-2);

            var dailyTasks =  _dailyTask.FindAll(x => x.DateTime.Date >= dayBeforeYesterday.Date
                                                     && !x.IsSubmitted && x.SalesPointId == salespointId).ToList();

            List<int> dailyTaskIds = dailyTasks.Select(x => x.Id).ToList();

            var dailyPosmTasks = await _dailyPosmTask.GetAllActive().Where(x => x.IsCompleted && dailyTaskIds.Contains(x.DailyTaskId)).ToListAsync();
            var dailyPomsTaskIds = await dailyPosmTasks.Select(x => x.Id).ToListAsync();


            var dailyTaskItems = await _dailyPosmTaskItem.GetAllActive().Where(x => x.PosmProductId == posmProductId &&
                                                                              dailyPomsTaskIds.Contains(x.DailyPosmTaskId) &&
                                                                              (x.ExecutionType == PosmWorkType.Installation ||
                                                                               x.ExecutionType == PosmWorkType.RemovalAndReInstallation)).ToListAsync();

            return dailyTaskItems.Sum(x => x.Quantity);
        }
    }
}
