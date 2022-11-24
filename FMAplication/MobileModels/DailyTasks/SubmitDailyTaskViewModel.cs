using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.DailyTasks
{
    public class SubmitDailyTaskViewModel
    {
        public DailyPosmTaskMBModel DailyPosmTask { get; set; }
        public DailySurveyTaskMBModel DailySurveyTask { get; set; }
        public DailyAvTaskMBModel DailyAvTask { get; set; }
        public DailyCommunicationTaskMBModel DailyCommunicationTask { get; set; }
        public DailyInformationTaskMBModel DailyInformationTask { get; set; }
        public DailyAuditTaskMBModel DailyAuditTask { get; set; }
        public int DailyTaskId { get; set; }
    }
}
