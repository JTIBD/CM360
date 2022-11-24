using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Transaction
{   
    public class WareHouseTransferItem : AuditableEntity<int>
    {
        public int TransactionId { get; set; }
        public WareHouseTransfer Transaction { get; set; }
        public int POSMProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
    }
}
