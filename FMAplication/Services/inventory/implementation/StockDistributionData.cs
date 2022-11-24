using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using FMAplication.Models.wareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.inventory.implementation
{
    public class StockDistributionData
    {
        public List<POSMProductModel> PosmProducts { get; set; }
        public List<SalesPointModel> SalesPointViewModels { get; set; }
        public WareHouseModel WareHouse { get; set; }
    }
}
