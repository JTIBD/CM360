using FMAplication.Core;
using FMAplication.Domain.Reports;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.DailyActivities
{

    public class DailyAudit : AuditableEntity<int>
    {
        public int DailyCMActivityId { get; set; }
        public bool IsDistributionCheck { get; set; }
        public bool IsFacingCount { get; set; }
        public bool IsPlanogramCheck { get; set; }
        public bool IsPriceAudit { get; set; }
        public int DistributionCheckStatus { get; set; }
        public int FacingCountStatus { get; set; }
        public int PlanogramCheckStatus { get; set; }
        public int PriceAuditCheckStatus { get; set; }
        [StringLength(512)]
        public string DistributionCheckIncompleteReason { get; set; }
        [StringLength(512)]
        public string FacingCountCheckIncompleteReason { get; set; }
        [StringLength(512)]
        public string PlanogramCheckIncompleteReason { get; set; }
        [StringLength(512)]
        public string PriceAuditCheckIncompleteReason { get; set; }
        public List<AuditReport> AllProducts { get; set; } = new List<AuditReport>();
    }
}
