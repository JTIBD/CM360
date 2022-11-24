using FMAplication.Core;
using FMAplication.Domain.Products;

namespace FMAplication.Domain.Transaction
{
    public class StockAdjustmentItems  :  AuditableEntity<int>
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public int PosmProductId { get; set; }
        public POSMProduct PosmProduct { get; set; }

        public int SystemQuantity { get; set; }
        public int AdjustedQuantity { get; set; }
       
    }
}