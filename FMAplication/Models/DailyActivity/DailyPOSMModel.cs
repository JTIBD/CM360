using fm_application.Models.DailyActivity;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.DailyPOSM
{
    public class DailyPOSMModel
    {
        public int Id { get; set; }
        public bool IsPOSMInstallation { get; set; }
        public bool IsPOSMRepair { get; set; }
        public bool IsPOSMRemoval { get; set; }
        public int DailyCMActivityId { get; set; }
        public int POSMInstallationStatus { get; set; }
        public int POSMRepairStatus { get; set; }
        public int POSMRemovalStatus { get; set; }
        [StringLength(512)]
        public string POSMRemovalIncompleteReason { get; set; }
        [StringLength(512)]
        public string POSMInstallationIncompleteReason { get; set; }
        [StringLength(512)]
        public string POSMRepairIncompleteReason { get; set; }
        public List<POSMReportModel> POSMProducts { get; set; } = new List<POSMReportModel>();
        public List<POSMReportModel> POSMInstallationProducts { get; set; } = new List<POSMReportModel>();
        public List<POSMReportModel> POSMRepairProducts { get; set; } = new List<POSMReportModel>();
        public List<POSMReportModel> POSMRemovalProducts { get; set; } = new List<POSMReportModel>();
    }
}
