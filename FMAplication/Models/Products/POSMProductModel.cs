using FMAplication.Enumerations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using FMAplication.Helpers;
using Newtonsoft.Json;

namespace FMAplication.Models.Products
{
    public class POSMProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]

        public string Name { get; set; }

        [Required]
        public int Type { get; set; }

        public bool IsJTIProduct { get; set; }

        public Status Status { get; set; }

        public bool IsDigitalSignatureEnable { get; set; }
        public bool IsPlanogram { get; set; }

        public string PlanogramImageUrl { get; set; }

        public string ImageUrl { get; set; }

        public string ImageUrlReal => Utility.GetRealUrl(ImageUrl);

        [Required]
        [StringLength(128)]
        public string Code { get; set; }

        public IFormFile PlanogramImageFile { get; set; }
        public IFormFile ImageFile { get; set; }

        public int? BrandId { get; set; }
        public int? SubBrandId { get; set; }
        public int CampaignId { get; set; }
    }
}
