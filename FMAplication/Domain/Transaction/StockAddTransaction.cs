using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Transaction
{
    public class StockAddTransaction : AuditableEntity<int>
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int PosmProductId { get; set; }
        public POSMProduct PosmProduct { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; }
    }
}
