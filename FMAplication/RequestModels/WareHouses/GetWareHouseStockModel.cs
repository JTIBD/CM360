using System.Collections.Generic;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels.WareHouses
{
    public class GetWareHouseStockModel:PaginationBaseParams
    {
        public List<int> WarehouseIds { get; set; }
    }
}