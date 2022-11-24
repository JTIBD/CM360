using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Sales;

namespace FMAplication.Domain.Users
{
    public class CmsUserSalesPointMapping : AuditableEntity<int>
    {
        public CMUser CmUser { get; set; }
        public int CmUserId { get; set; }

       
        public int SalesPointId { get; set; }

    }
}
