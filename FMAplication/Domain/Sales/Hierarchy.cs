using FMAplication.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.Sales
{
    public class Hierarchy : AuditableEntity<int>
    {
        [StringLength(256, MinimumLength = 1)]
        public string HierarchyName { get; set; }
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
    }
}
