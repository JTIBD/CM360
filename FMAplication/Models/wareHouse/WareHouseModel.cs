using FMAplication.Core;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.wareHouse
{
    public class WareHouseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public WareHouseType Type { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhone { get; set; }
    }
}
