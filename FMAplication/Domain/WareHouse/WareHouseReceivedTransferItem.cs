using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.WareHouse
{
    public class WareHouseReceivedTransferItem:AuditableEntity<int>
    {
        public int TransferId { get; set; }
        public WareHouseReceivedTransfer Transfer { get; set; }
        public int POSMProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQuantity { get; set; }
    }
}
