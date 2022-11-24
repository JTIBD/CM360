using FMAplication.Attributes;
using FMAplication.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Domain.Questions
{
    public class SurveyQuestionSet : AuditableEntity<int>
    {        
        [UniqueKey(groupId: "1", order: 0)]
        [Column(Order = 1)]
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
    }
}
