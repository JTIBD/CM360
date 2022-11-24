using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Enumerations;
using FMAplication.Models.Bases;

namespace FMAplication.Models.Sales
{

    public class OutletModel: IWithRouteOptional
    {
        public int OutletId { get; set; }
        
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string BanglaName { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string OutletNameBangla { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string OwnerName { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string OwnerNameBangla { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string AddressBangla { get; set; }

        [StringLength(128)]
        public string ContactNumber { get; set; }

        [StringLength(128)]
        public string Location { get; set; }

        [StringLength(128)]
        public string Latitude { get; set; }

        [StringLength(128)]
        public string Longitude { get; set; }

        public int SalesPointId { get; set; }
        public int? RouteId { get; set; }
        public int? NodeId { get; set; }
        [NotMapped]
        public RouteModel Route { get; set; }
        [NotMapped]

        public SalesPointModel SalesPoint { get; set; }
        public int ChannelID { get; set; }
        public ChannelModel Channel { get; set; }
        public int DailyActivityId { get; set; }
        public string OutletType { get; set; }

    }
}
