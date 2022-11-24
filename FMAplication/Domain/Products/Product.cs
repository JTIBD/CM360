using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Domain.Brand;
using FMAplication.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Products
{
    public class Product : AuditableEntity<int>
    {
        protected Product()
        {
            IsJTIProduct = true;
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        public ActionType ActionType { get; set; }

        public bool IsJTIProduct { get; set; }

        [UniqueKey(groupId: "1", order: 0)]
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }
        public string ImageUrl { get; set; }

        public int? BrandId { get; set; }
        public int? SubBrandId { get; set; }

        public FMAplication.Domain.Brand.Brand Brand { get; set; }
        public SubBrand SubBrand { get; set; }
    }
}
