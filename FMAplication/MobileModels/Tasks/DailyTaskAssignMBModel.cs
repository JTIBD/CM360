using FMAplication.MobileModels.AvCommunications;
using FMAplication.MobileModels.Surveys;
using FMAplication.Models.PosmAssign;
using System;
using System.Collections.Generic;
using FMAplication.MobileModels.Audits;
using FMAplication.MobileModels.ExecutionLimits;
using FMAplication.MobileModels.Reasons;
using FMAplication.MobileModels.Sales;

namespace FMAplication.MobileModels.Tasks
{
    public class DailyTaskAssignMBModel
    {
        public DateTime TaskDate { get; set; }
        public List<SalesPointMBModel> SalesPointModels { get; set; }
        public List<PosmTaskMBModel> PosmTasks { get; set; }
        public List<SurveyMBModel> SurveyTasks { get; set; }
        public List<ConsumerSurveyMBModel> ConsumerSurveyTasks { get; set; }
        public List<AvSetupMBModel> AvTasks { get; set; }
        public List<CommunicationSetupMBModel> CommunicationTasks { get; set; }        
        public List<AuditSetupMBModel> AuditTasks { get; set; }
        public List<ReasonsWithType> Reasons { get; set; }
        public List<GuidelineSetupMBModel> Guidelines { get; set; }
        public List<MinimumExecutionLimitMBModel> MinimumExecutionLimits { get; set; }
    }
}
