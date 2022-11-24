using FMAplication.Domain.Examples;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Extensions;
using FMAplication.Models.Examples;
using FMAplication.Repositories;
using FMAplication.Services.Sales.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Sales.Implementation
{
    public class OutletService : IOutletService
    {
        private readonly IRepository<Outlet> _outlet;
        private readonly IRepository<Route> _route;
        private readonly IRepository<UserTerritoryMapping> _userTerritoryMappings;
        private readonly IRepository<Node> _node;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNodeMappings;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<Channel> _channel;

        public OutletService(IRepository<Outlet> outlet, IRepository<Route> route, 
            IRepository<UserTerritoryMapping> userTerritoryMappings, 
            IRepository<Node> node, 
            IRepository<SalesPointNodeMapping> salesPointNodeMappings,
            IRepository<SalesPoint> salesPoint,
            IRepository<Channel> channel)
        {
            _outlet = outlet;
            _route=route;
            this._userTerritoryMappings = userTerritoryMappings;
            this._node = node;
            this._salesPointNodeMappings = salesPointNodeMappings;
            _salesPoint = salesPoint;
            _channel = channel;
        }


        // public async Task<ExampleModel> CreateAsync(ExampleModel model)
        // {
        //     var example = model.ToMap<ExampleModel, Example>();
        //     var result = await _example.CreateAsync(example);
        //     return result.ToMap<Example, ExampleModel>();
        // }

        // public async Task<int> DeleteAsync(int id)
        // {
        //     var result = await _example.DeleteAsync(s => s.Id == id);
        //     return result;

        // }

        public async Task<Outlet> GetOutletsByOutletId(int outletId)
        {
            var result = await _outlet.FindAllAsync(s => s.OutletId == outletId);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Outlet>> GetOutletsByRouteId(int id)
        {
            var result = await _outlet.FindAllAsync(s => s.RouteId == id);
            return result;
        }

        public async Task<IEnumerable<Outlet>> GetOutletsByChannelId(int id)
        {
            if (id== 0)
            {
                return null;
            }
            var result = await _outlet.FindAllAsync(s => s.ChannelID == id);
            return result;
        }

        public async Task<IEnumerable<Outlet>> GetOutletsByChannelIdandSalespointId(int id, int sid)
        {
            if (id == 0)
            {
                return null;
            }
            var result = await _outlet.FindAllAsync(s => s.ChannelID == id && s.SalesPointId == sid);
            return result;
        }

        public async Task<IEnumerable<Route>> GetRoutesBySalesPointId(int id)
        {
            var result = await _route.FindAllAsync(s => s.SalesPointId == id);
            return result;
        }

        public async Task<IEnumerable<Route>> GetRoutesByFMId(int id)
        {
            #region get all child node
            var nodeIds = (await _userTerritoryMappings.FindAllAsync(s => s.UserInfoId == id)).Select(x => x.NodeId).ToList();
            var allNodes = _node.GetAll().ToList();
            List<int> territoryNodeIds = new List<int>();

            // get all territory node ids
            foreach (var nodeId in nodeIds)
            {
                territoryNodeIds.AddRange(GetTerritoryNodeIds(allNodes, nodeId, new List<int>()).ChildNodeIds);
            }
            territoryNodeIds = territoryNodeIds.Distinct().ToList();
            #endregion

            var salesPointIds = (await _salesPointNodeMappings.FindAllAsync(s => territoryNodeIds.Any(n => n == s.NodeId))).Select(x => x.SalesPointId).ToList();
            var result = (await _route.FindAllAsync(s => salesPointIds.Any(sp => sp == s.SalesPointId))).OrderBy(x => x.RouteName);

            return result;
        }

        public async Task<IEnumerable<SalesPoint>> GetSalesPointByFMId(int id)
        {
            #region get all child node
            var nodeIds = (await _userTerritoryMappings.FindAllAsync(s => s.UserInfoId == id)).Select(x => x.NodeId).ToList();
            var allNodes = _node.GetAll().ToList();
            List<int> territoryNodeIds = new List<int>();

            // get all territory node ids
            foreach (var nodeId in nodeIds)
            {
                territoryNodeIds.AddRange(GetTerritoryNodeIds(allNodes, nodeId, new List<int>()).ChildNodeIds);
            }
            territoryNodeIds = territoryNodeIds.Distinct().ToList();
            #endregion

            var salesPointIds = (await _salesPointNodeMappings.FindAllAsync(s => territoryNodeIds.Any(n => n == s.NodeId))).Select(x => x.SalesPointId).ToList();
            var result = (await _salesPoint.FindAllAsync(s => salesPointIds.Any(sp => sp == s.SalesPointId))).OrderBy(x => x.Name);

            return result;
        }

        public (IList<Node> AllNodes, int NodeId, IList<int> ChildNodeIds) GetTerritoryNodeIds(IList<Node> AllNodes, int NodeId, IList<int> ChildNodeIds)
        {
            if (!AllNodes.Any(x => x.ParentId == NodeId))
            {
                ChildNodeIds.Add(NodeId);
                return (AllNodes, NodeId, ChildNodeIds);
            }
            else
            {
                var nodes = AllNodes.Where(x => x.ParentId == NodeId).ToList();
                foreach (var node in nodes)
                {
                    GetTerritoryNodeIds(AllNodes, node.NodeId, ChildNodeIds);
                }
            }

            return (AllNodes, NodeId, ChildNodeIds);
        }

        // public async Task<IEnumerable<ExampleModel>> GetExamplesAsync()
        // {
        //     var result = await _example.GetAllAsync();
        //     return result.ToMap<Example, ExampleModel>();
        // }

        // public async Task<IPagedList<ExampleModel>> GetPagedExamplesAsync(int pageNumber, int pageSize)
        // {
        //     var result = await _example.GetAllPagedAsync(pageNumber, pageSize);
        //     return result.ToMap<Example, ExampleModel>();

        // }



    }
}
