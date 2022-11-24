using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;

namespace FMAplication.Models.Questions
{
    public class QuestionOptionModel
    {
        public int Id { get; set; }
   
        public string OptionTitle { get; set; }
        public string OptionEmoticon { get; set; }

        public int Sequence { get; set; }

        public int Status { get; set; }

        public int QuestionId { get; set; }
        public int DailyCMActivityId { get; set; }        
        public int SurveyId { get; set; }

        public int SurveyReportId { get; set; }
    }
}
