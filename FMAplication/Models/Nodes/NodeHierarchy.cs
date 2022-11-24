using FMAplication.Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Nodes
{
    public class NodeHierarchy
    {        
        public Domain.Sales.Node Node { get; set; }        
        public List<SalesPoint> SalesPoints { get; set; }
        public List<NodeHierarchy> Nodes { get; set; }
        public string HierarchyCode { get; set; }
    }
}
