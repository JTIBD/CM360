using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Sales;

namespace FMAplication.Domain.ExecutionLimits
{
    public class MinimumExecutionLimit : AuditableEntity<int> 
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesPointId { get; set; }
        public int TargetVisitedOutlet { get; set; }
    }
}
