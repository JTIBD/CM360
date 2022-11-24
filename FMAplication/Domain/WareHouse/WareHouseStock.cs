using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.WareHouse
{
    public class WareHouseStock:AuditableEntity<int>
    {
        public int WareHouseId { get; set; }
        public WareHouse WareHouse { get; set; }
        public int PosmProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
    }
}
