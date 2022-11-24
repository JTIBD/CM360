using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Products;

namespace FMAplication.Models.Transaction
{
    public class StockAdjustmentItemsModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }
        public Domain.Transaction.Transaction Transaction { get; set; }

        public int PosmProductId { get; set; }
        public POSMProduct PosmProduct { get; set; }

        public int SystemQuantity { get; set; }
        public int AdjustedQuantity { get; set; }
    }
}
