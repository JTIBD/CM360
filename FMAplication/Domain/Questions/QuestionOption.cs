using FMAplication.Attributes;
using FMAplication.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Domain.Questions
{
    public class QuestionOption : AuditableEntity<int>
    {
        // [UniqueKey(groupId: "1", order: 0)]

        [Required]
        [StringLength(256)]
        public string OptionTitle { get; set; }

        public int Sequence { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
