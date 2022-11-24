using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;

namespace FMAplication.Domain.Task
{
    public class PosmTaskAssign : AuditableEntity<int>
    {
        public int SalesPointId { get; set; }

        public int PosmProductId { get; set; }
        public POSMProduct PosmProduct { get; set; }

        public int CmUserId { get; set; } 
        public CMUser CmUser { get; set; }

        public PosmTaskAssignStatus TaskStatus { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }   
    }
}
