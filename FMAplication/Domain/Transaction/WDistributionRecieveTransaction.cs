using FMAplication.Core;
using FMAplication.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Transaction
{
    public class WDistributionRecieveTransaction: AuditableEntity<int>
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int POSMProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int RecievedQuantity { get; set; }
    }
}
