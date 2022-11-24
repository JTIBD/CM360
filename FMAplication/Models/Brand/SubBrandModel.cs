using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Models.Brand
{
    public class SubBrandModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Details { get; set; }
        public int Status { get; set; }

        public int BrandId { get; set; }
        public BrandModel Brand { get; set; }
    }
}
