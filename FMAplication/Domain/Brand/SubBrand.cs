using FMAplication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Domain.Brand
{
    public class SubBrand : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
