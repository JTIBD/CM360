using FMAplication.Core;
using FMAplication.Domain.Products;

namespace FMAplication.Domain.Sales
{
    public class SalesPointStock: AuditableEntity<int>
    {
        public int Id { get; set; }
        public int SalesPointId { get; set; }
        public int POSMProductId { get; set; }        
        public POSMProduct POSMProduct { get; set; }
        public int Quantity { get; set; }
    }
}
