using FMAplication.Models.Sales;

namespace FMAplication.Models.Bases
{
    public interface IWithOutlet
    {
        public OutletModel Outlet { get; set; }
        public int OutletId { get; set; }
    }
}