using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;

namespace FMAplication.Domain.TransactionWorkFlows
{
    public class TransactionNotification : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }

      
        public int TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }

        public int TransactionWorkFlowId { get; set; }

        public bool IsSeen { get; set; }
    }
}
