using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Audit
{
    public class AuditPOSMProduct:AuditableEntity<int>
    {
        public int AuditSetupId { get; set; }
        public AuditSetup AuditSetup { get; set; }
        public int POSMProductId { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public ActionType ActionType { get; set; }

    }
}
