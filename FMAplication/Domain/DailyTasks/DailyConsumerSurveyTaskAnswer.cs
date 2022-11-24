using FMAplication.Core;
using FMAplication.Domain.Questions;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyConsumerSurveyTaskAnswer : AuditableEntity<int>
    {
        public DailyConsumerSurveyTask DailyConsumerSurveyTask { get; set; }
        public int DailyConsumerSurveyTaskId { get; set; }

        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}