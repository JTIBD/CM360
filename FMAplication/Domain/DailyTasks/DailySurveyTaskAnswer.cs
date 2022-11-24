using FMAplication.Core;
using FMAplication.Domain.Questions;

namespace FMAplication.Domain.DailyTasks
{
    public class DailySurveyTaskAnswer : AuditableEntity<int>
    {
        public DailySurveyTask DailySurveyTask { get; set; }
        public int DailySurveyTaskId { get; set; }

        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}