using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMAplication.Core;

namespace FMAplication.Domain.Questions
{
    public class Question : AuditableEntity<int>
    {
        [Required]
        [StringLength(256)]
        public string QuestionTitle { get; set; }

        [Required]
        [StringLength(256)]
        public string QuestionType { get; set; }
        public ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
    }
}
