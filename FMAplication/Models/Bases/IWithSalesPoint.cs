using FMAplication.Models.Sales;

namespace FMAplication.Models.Bases
{
    public interface IWithSalesPoint
    {
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }

    }
}