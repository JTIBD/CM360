using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Sales;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;

namespace FMAplication.Models.ExecutionLimits
{
    public class GetMinimumExecutionLimitModel
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public string SearchText { get; set; } = "";
        public int Id { get; set; }
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public int TargetVisitedOutlet { get; set; }
    }
}
