using fm_application.Models.DailyActivity;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.DailyAudit
{
    public class DailyAuditModel
    {
        public int Id { get; set; }
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

        public List<AuditReportModel> DistributionCheckProducts { get; set; } = new List<AuditReportModel>();
        public List<AuditReportModel> FacingCountProducts { get; set; } = new List<AuditReportModel>();
        public List<AuditReportModel> PlanogramCheckProducts { get; set; } = new List<AuditReportModel>();
        public List<AuditReportModel> PriceAuditProducts { get; set; } = new List<AuditReportModel>();
        public List<AuditReportModel> AllProducts { get; set; } = new List<AuditReportModel>();

    }
}
