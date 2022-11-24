using FMAplication.Enumerations;
using System.ComponentModel.DataAnnotations;
using FMAplication.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FMAplication.Models.Products
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        public ActionType ActionType { get; set; }
        public bool IsJTIProduct { get; set; }
        public Status Status { get; set; }
        [Required]
        [StringLength(128)]
        public string Code { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public int? BrandId { get; set; }
        public int? SubBrandId { get; set; }
    }
}