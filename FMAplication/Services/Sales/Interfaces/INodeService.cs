using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Models.Sales;

namespace FMAplication.Services.Sales.Interfaces
{
    public interface INodeService
    {
        Task<IEnumerable<NodeModel>> GetAllNodesAsync();
        Task<NodeModel> GetNodeAsync(int id);
        Task<IEnumerable<Channel>> GetAllChannelAsync();
    }
}
