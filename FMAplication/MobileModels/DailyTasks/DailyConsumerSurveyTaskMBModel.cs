using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Reasons;

namespace FMAplication.MobileModels.DailyTasks
{
    public class ConsumerSurveyExecutionModel : DailyTaskBaseMbModel
    {

        public SurveyQuestionSet SurveyQuestionSet { get; set; }
        public int SurveyQuestionSetId { get; set; }
  
    }
    public class DailyConsumerSurveyTaskMBModel
    {
        public ConsumerSurveyExecutionModel SurveyExecution { get; set; }
        public ICollection<DailyConsumerSurveyTaskAnswerMBModel> QuestionAnswers { get; set; }

        public DailyConsumerSurveyTaskMBModel()
        {
            QuestionAnswers = new List<DailyConsumerSurveyTaskAnswerMBModel>();
        }
    }
}
