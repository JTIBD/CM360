using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.DailyActivity
{
    public class DailyCMTaskReportModel
    {
        public DateTime Date { get; set; }
        public string DisplayDate { get; set; }
        public string FMUserName { get; set; }
        public string CMUserName { get; set; }
        public string OutletNames { get; set; }
        public int CMId { get; set; }
        public int CompletedPOSM { get; set; }
        public int TotalPOSM { get; set; }
        public int CompletedAudit { get; set; }
        public int TotalAudit { get; set; }
        public int CompletedSurvey { get; set; }
        public int TotalSurvey { get; set; } 

        public int NumberOfRow {get; set;}

        public string TotalPercentage {get; set;}
        public string POSMPercentage {get; set;}

        public string AuditPercentage {get; set;}

        public string SurveyPercentage {get; set;}


    }
}
