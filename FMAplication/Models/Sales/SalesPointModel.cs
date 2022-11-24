using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Sales
{
    public class SalesPointModel
    {
        public int Id { get; set; }
        public int SalesPointId { get; set; }
 
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string BanglaName { get; set; }
        [StringLength(500)]
        public string OfficeAddress { get; set; }
        [StringLength(500)]
        public string ContactNo { get; set; }
        [StringLength(128)]
        public string EmailAddress { get; set; }
        public int DeliveryChannelType { get; set; }
        [StringLength(128)]
        public string Latitude { get; set; }
        [StringLength(128)]
        public string Longitude { get; set; }
        public int SequenceId { get; set; }
        [StringLength(128)]
        public string TownName { get; set; }
        public int? DepotCode { get; set; }
        [StringLength(256)]
        public string OfficeAddressBangla { get; set; }
    }
}
