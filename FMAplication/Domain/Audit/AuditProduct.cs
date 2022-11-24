using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Audit
{
    public class AuditProduct:AuditableEntity<int>
    {
        public int AuditSetupId { get; set; }
        public AuditSetup AuditSetup { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ActionType ActionType { get; set; }

    }
}
