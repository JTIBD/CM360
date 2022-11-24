using System.ComponentModel.DataAnnotations;
using FMAplication.Enumerations;
using Microsoft.AspNetCore.Http;

namespace FMAplication.MobileModels.Products
{
    public class POSMProductMBModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]

        public string Name { get; set; }

        public string Code { get; set; }


    }
}
