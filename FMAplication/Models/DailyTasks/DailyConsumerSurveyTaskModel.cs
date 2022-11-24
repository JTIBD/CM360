using System.Collections.Generic;
using FMAplication.Models.Bases;
using FMAplication.Models.Questions;
using FMAplication.Models.Reasons;

namespace FMAplication.Models.DailyTasks
{
    public class DailyConsumerSurveyTaskModel:DailyBaseTaskModel
    {
        public SurveyQuestionSetModel SurveyQuestionSet { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public List<DailyConsumerSurveyTaskAnswerModel> DailyConsumerSurveyTaskAnswers { get; set; }

    }
}