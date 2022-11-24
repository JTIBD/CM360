using FMAplication.Core;

namespace FMAplication.Domain
{
    public class ReasonReasonTypeMapping:AuditableEntity<int>
    {
        public int ReasonId { get; set; }
        public Reason Reason { get; set; }
        public int ReasonTypeId { get; set; }
        public ReasonType ReasonType { get; set; }
    }
}