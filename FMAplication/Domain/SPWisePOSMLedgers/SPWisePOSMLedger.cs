using System;
using FMAplication.Core;
using FMAplication.Domain.Products;

namespace FMAplication.Domain.SPWisePOSMLedgers
{
    public class SPWisePOSMLedger : AuditableEntity<int>
    {
       
        public POSMProduct PosmProduct { get; set; }
        public int PosmProductId { get; set; }

        public int SalesPointId { get; set; }
        public DateTime Date { get; set; }

        public int OpeningStock { get; set; }
        public int ReceivedStock { get; set; }
        public int ExecutedStock { get; set; }
        public int ClosingStock { get; set; }
    }
}
