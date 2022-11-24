using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using FMAplication.Models.Products;

namespace FMAplication.Models.Reports
{
    public class CwStockUpdateModel
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public int PosmProductId { get; set; }
        public string PosmProductCode { get; set; }
        public string PosmProductName { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; }
        public string WareHouseName { get; set; }
    }
}
