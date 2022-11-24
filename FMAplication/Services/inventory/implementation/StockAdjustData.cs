using System.Collections.Generic;
using FMAplication.Models.Products;
using FMAplication.Models.wareHouse;

namespace FMAplication.Services.inventory.implementation
{
    public class StockAdjustData
    {
        public List<POSMProductStockModel> PosmProducts { get; set; }
        public WareHouseModel WareHouse { get; set; }
    }

    
}