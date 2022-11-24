using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.wareHouse
{
    public class WareHouseDistributionData
    {
        public List<POSMProductModel> PosmProducts { get; set; }
        public WareHouseModel FromWareHouse { get; set; }
        public WareHouseModel ToWareHouse { get; set; }
    }
}
