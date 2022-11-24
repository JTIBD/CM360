using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Models.Sales;

namespace FMAplication.MobileModels.Sales
{
    public class OutletMBModel
    {
        public int OutletId { get; set; }
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(256, MinimumLength = 1)]
        public string BanglaName { get; set; }
        public int SalesPointId { get; set; }
        public int? RouteId { get; set; }
        [NotMapped]
        public RouteMBModel Route { get; set; }
        public SalesPointMBModel SalesPoint { get; set; }
        public int ChannelID { get; set; }
        public ChannelMBModel Channel { get; set; }
        public string OutletType { get; set; }
    }
}