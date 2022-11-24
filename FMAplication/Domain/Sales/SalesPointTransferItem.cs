using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Sales
{
    public class SalesPointTransferItem:AuditableEntity<int>
    {
        public int TransferId { get; set; }
        public SalesPointTransfer Transfer { get; set; }
        public int POSMProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
    }
}
