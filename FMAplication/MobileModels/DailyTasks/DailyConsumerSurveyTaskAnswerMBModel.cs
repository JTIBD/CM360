using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyConsumerSurveyTaskAnswerMBModel
    {
        public int Id { get; set; }
        public DailyConsumerSurveyTaskMBModel ConsumerSurveyTask { get; set; }
        public int ConsumerSurveyTaskId { get; set; }

        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }


}
