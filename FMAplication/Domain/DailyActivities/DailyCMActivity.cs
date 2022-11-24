using FMAplication.Core;
using FMAplication.Domain.Reports;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Domain.DailyActivities
{
    public class DailyCMActivity : AuditableEntity<int>
    {
        public int OutletId { get; set; }
        public int CMId { get; set; }
        public DateTime Date { get; set; }
        public int AssignedFMUserId { get; set; }
        public bool IsAudit { get; set; }
        public bool IsSurvey { get; set; }
        public bool IsPOSM { get; set; }
        public bool IsConsumerSurveyActive { get; set; }
        //public Outlet Outlet { get; set; }
        public UserInfo AssignedFMUser { get; set; }
        public CMUser CM { get; set; }

        public DailyPOSM DailyPOSM { get; set; }
        public DailyAudit DailyAudit { get; set; }
        public List<SurveyReport> SurveyQuestions { get; set; } = new List<SurveyReport>();

    }
}
