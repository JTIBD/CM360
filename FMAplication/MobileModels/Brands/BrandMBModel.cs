using System.ComponentModel.DataAnnotations;

namespace FMAplication.MobileModels.Brands
{
    public class BrandMBModel
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Code { get; set; }
    }
}