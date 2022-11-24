using FMAplication.Models.Questions;
using FMAplication.Enumerations;
using FMAplication.Domain.DailyActivities;

namespace fm_application.Models.DailyActivity
{
    public class SurveyReporModel
    {
        public int Id { get; set; }
        public int DailyCMActivityId { get; set; }
        public int CMId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int SurveyId { get; set; }
        public QuestionModel Question { get; set; }
        public SurveyQuestionSetModel Survey { get; set; }
        public bool IsConsumerSurvey { get; set; }

        public Status Status { get; set; }

        public DailyCMActivity DailyCMActivity { get; set; }
    }
}