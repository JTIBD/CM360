
using FMAplication.Domain.Examples;
using FMAplication.Domain.Sales;
using FMAplication.Extensions;
using FMAplication.Models.Examples;
using FMAplication.Repositories;
using FMAplication.Services.Sales.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Models.Sales;
using X.PagedList;
namespace fm_application.Services.Sales.Implementation
{
    public class NodeService : INodeService
    {
        private readonly IRepository<Node> _node;
        private readonly IRepository<Channel> _channel;

        public NodeService(IRepository<Node> node, IRepository<Channel> channel)
        {
            _node = node;
            _channel = channel;
        }

        public async Task<IEnumerable<NodeModel>> GetAllNodesAsync()
        {
            var result = await _node.GetAllAsync();
            return result.ToMap<Node, NodeModel>();
        }

        public async Task<IEnumerable<Channel>> GetAllChannelAsync()
        {
            var result = await _channel.GetAllAsync();
            return result;
        }

        public async Task<NodeModel> GetNodeAsync(int id)
        {
            var result = await _node.FindAsync(s => s.NodeId == id);
            return result.ToMap<Node, NodeModel>();
        }
    }
}