using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyPosmAuditTask : AuditableEntity<int>
    {
        public POSMProduct PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public ActionType ActionType { get; set; }
        public int Result { get; set; }
        public DailyAuditTask DailyAuditTask { get; set; }
        public int DailyAuditTaskId { get; set; }
    }
}
