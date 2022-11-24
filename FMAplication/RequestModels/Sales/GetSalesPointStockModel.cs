using System.Collections.Generic;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels.Sales
{
    public class GetSalesPointStockModel:PaginationBaseParams
    {
        public List<int> SalesPointIds { get; set; }
        
    }
}