using System.ComponentModel.DataAnnotations;

namespace FMAplication.MobileModels.Sales
{
    public class RouteMBModel
    {
        public int RouteId { get; set; }
        //[UniqueKey]
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string RouteName { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string RouteNameBangla { get; set; }
        public int SalesPointId { get; set; }
    }
}