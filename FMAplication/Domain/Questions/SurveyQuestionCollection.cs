using FMAplication.Attributes;
using FMAplication.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Domain.Questions
{
    public class SurveyQuestionCollection : AuditableEntity<int>
    {
        [UniqueKey(groupId: "1", order: 0)]
        //[Key,Column(Order = 1)]
        [Column(Order = 0)]
        [Required]
        public int QuestionId { get; set; }

        //[Key,Column(Order = 2)]
        [UniqueKey(groupId: "1", order: 1)]
        [Required]
        [Column(Order = 1)]
        public int SurveyId { get; set; }
        public Question Question { get; set; }
    }
}
