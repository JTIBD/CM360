using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Sales
{
    public class NodeModel
    {
        public NodeModel()
        {
            this.NodeIdList = new List<int>();
        }
        public int Id { get; set; }
        public int NodeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Status { get; set; }

        public List<int> NodeIdList { get; set; }

    }
}
