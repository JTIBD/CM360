using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;

namespace FMAplication.Models.Questions
{
    public class QuestionModel
    {
        // public QuestionModel()
        // {
        //     this.QuestionOptions = new List<QuestionOption>();
        // }
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionTitle { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionType { get; set; }

        public Status Status { get; set; }

        public int DailyCMActivityId { get; set; }
        
        public int SurveyId { get; set; }

        public int SurveyReportId { get; set; }

        public bool IsEditable { get; set; }

        public ICollection<QuestionOptionModel> QuestionOptions { get; set; } 
            = new List<QuestionOptionModel>();

    }
}
