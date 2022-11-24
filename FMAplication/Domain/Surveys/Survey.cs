using FMAplication.Domain.Questions;
using FMAplication.Domain.Bases;

namespace FMAplication.Domain.Surveys
{
    public class Survey: BaseSetup
    {
        public string Code { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public SurveyQuestionSet SurveyQuestionSet { get; set; }
        public bool IsConsumerSurvey { get; set; }
    }
}
