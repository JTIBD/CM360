using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Domain.Brand;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Products
{
    public class POSMProduct : AuditableEntity<int>
    {
        protected POSMProduct()
        {
            IsJTIProduct = true;
            IsDigitalSignatureEnable = true;

        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public int Type { get; set; }

        public bool IsJTIProduct { get; set; }

        public bool IsDigitalSignatureEnable { get; set; }
        public bool IsPlanogram { get; set; }
        public string PlanogramImageUrl { get; set; }
        public string ImageUrl { get; set; }

        [UniqueKey(groupId: "1", order: 0)]
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        public int? BrandId { get; set; }
        public int? SubBrandId { get; set; }
        public int CampaignId { get; set; }
        public FMAplication.Domain.Brand.Brand Brand { get; set; }
        public SubBrand SubBrand { get; set; }
        public FMAplication.Domain.Campaign.Campaign Campaign { get; set; }

    }
}
