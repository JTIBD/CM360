using FMAplication.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Domain.Brand
{
    public class Brand : AuditableEntity<int>
    {
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(128)]
        public string Code { get; set; }
        public string Details { get; set; }
    }
}
