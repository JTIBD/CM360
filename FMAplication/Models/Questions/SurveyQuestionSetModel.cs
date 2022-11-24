using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using fm_application.Models.DailyActivity;
using FMAplication.Enumerations;

namespace FMAplication.Models.Questions
{
    public class SurveyQuestionSetModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public Status Status { get; set; }
        public List<int> QuestionsId { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public int DailyCMActivityId { get; set; }
        public bool IsConsumerSurvey { get; set; }
        public bool IsEditable { get; set; }
        public bool IsDeletable { get; set; } = true;
        public List<SurveyReporModel> SurveyReportQuestions { get; set; } = new List<SurveyReporModel>();
    }
}
