using FMAplication.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Extensions;
using FMAplication.MobileModels.DailyTasks;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyTask  : AuditableEntity<int>
    {
        public CMUser CmUser { get; set; }
        public int CmUserId { get; set; }
        public int SalesPointId { get; set; }
        public DateTime DateTime { get; set; }
        [NotMapped] 
        public string DateTimeStr => this.DateTime.ToIsoString();
        public bool IsSubmitted { get; set; }
        public List<DailyPosmTask> DailyPosmTasks { get; set; }
        public List<DailyAuditTask> DailyAuditTasks { get; set; }
        public List<DailySurveyTask> DailySurveyTasks { get; set; }
        public List<DailyConsumerSurveyTask> DailyConsumerSurveyTasks { get; set; }
        public List<DailyAVTask> DailyAVTasks { get; set; }
        public List<DailyCommunicationTask> DailyCommunicationTasks { get; set; }
        public List<DailyInformationTask> DailyInformationTasks { get; set; }
    }
}
