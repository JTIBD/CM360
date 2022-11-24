
using FMAplication.Core;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Extensions;

namespace FMAplication.Domain.Transaction
{
    public class Transaction : AuditableEntity<int>
    {
        public string TransactionNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        [NotMapped] 
        public string TransactionDateStr => TransactionDate.ToIsoString();
        public string ChalanNumber { get; set; }

        public WareHouse.WareHouse WareHouse { get; set; }  
        public int? WarehouseId { get; set; }
        
        
        public int SalesPointId { get; set; }
        public int ReferenceTransactionId { get; set; }

        public List<WDistributionTransaction> WDistributionTransactions { get; set; }
        public List<WDistributionRecieveTransaction> WDistributionRecieveTransactions { get; set; }
        public List<SalesPointAdjustmentItem> SalesPointAdjustmentItems { get; set; }
        public List<StockAdjustmentItems> StockAdjustmentItems { get; set; }

    }
}
