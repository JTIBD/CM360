using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Products;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.Sales
{
    public class SalesPointDistributionData
    {
        public List<POSMProductModel> PosmProducts { get; set; }
        public SalesPointModel FromSalesPoint { get; set; }
        public List<SalesPointModel> ToSalesPoints { get; set; }
    }
}
