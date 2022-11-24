using FMAplication.Models.Sales;

namespace FMAplication.Models.Bases
{
    public interface IWithRouteOptional
    {
        public int? RouteId { get; set; }
        public RouteModel Route { get; set; }
    }
}