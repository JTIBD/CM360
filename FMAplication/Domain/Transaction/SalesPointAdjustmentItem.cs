using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Transaction
{
    public class SalesPointAdjustmentItem:AuditableEntity<int>
    {
            public int TransactionId { get; set; }
            public Transaction Transaction { get; set; }
            public int PosmProductId { get; set; }
            public POSMProduct PosmProduct { get; set; }
            public int SystemQuantity { get; set; }
            public int AdjustedQuantity { get; set; }
    }
}
