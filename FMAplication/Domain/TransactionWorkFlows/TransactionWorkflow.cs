using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Users;
using FMAplication.Domain.WorkFlows;
using FMAplication.Enumerations;

namespace FMAplication.Domain.TransactionWorkFlows
{
    public class TransactionWorkflow : AuditableEntity<int>
    {
        public int WorkFlowId { get; set; }

        public TWStatus TWStatus { get; set; }

        public int? SubmittedById { get; set; }
        public UserInfo SubmittedBy { get; set; }
        
        public int TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }    


        public int? UserId { get; set; }
        public UserInfo User { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int Sequence { get; set; }


        public int WorkflowConfigurationId { get; set; }

    }
}
