using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Enumerations;

namespace FMAplication.Domain.WareHouse
{
    public class WareHouse:AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public WareHouseType Type { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhone { get; set; }
    }
}
