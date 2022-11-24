using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;

namespace FMAplication.Domain.WorkFlows
{

    public class WorkFlowConfiguration : AuditableEntity<int>
    {

        public int WorkFlowId { get; set; }
        [ForeignKey("WorkFlowId")]
        public WorkFlow WorkFlow { get; set; }

        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public UserInfo User { get; set; }

        public int Sequence { get; set; }
    }
}
