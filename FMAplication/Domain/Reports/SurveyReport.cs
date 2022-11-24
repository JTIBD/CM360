using FMAplication.Core;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Examples;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Reports
{

    public class SurveyReport : AuditableEntity<int>
    {
        public int DailyCMActivityId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int SurveyId { get; set; }
        public Question Question { get; set; }
        public SurveyQuestionSet Survey { get; set; }
        public DailyCMActivity DailyCMActivity { get; set; }
        public bool IsConsumerSurvey { get; set; }
    }
}
