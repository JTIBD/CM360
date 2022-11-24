using System.Collections.Generic;
using FMAplication.Core;

namespace FMAplication.Domain
{
    public class ReasonType: AuditableEntity<int>
    {
        public string Text { get; set; }
        public string Code { get; set; }
        public List<ReasonReasonTypeMapping> ReasonReasonTypeMappings { get; set; }
    }
}