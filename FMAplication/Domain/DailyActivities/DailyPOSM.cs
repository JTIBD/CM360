using FMAplication.Core;
using FMAplication.Domain.Reports;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.DailyActivities
{

    public class DailyPOSM : AuditableEntity<int>
    {
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
        public List<POSMReport> POSMProducts { get; set; } = new List<POSMReport>();   

    }
}
