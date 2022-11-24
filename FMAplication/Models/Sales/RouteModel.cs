using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Sales
{
    public class RouteModel
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
        public int NumberOfOutlets { get; set; }

        [StringLength(128)]
        public string ContactNo { get; set; }

        public int SalesPointId { get; set; }
        public int? NodeId { get; set; }
    }
}
