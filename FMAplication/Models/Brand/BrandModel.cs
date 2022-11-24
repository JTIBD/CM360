using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMAplication.Enumerations;

namespace FMAplication.Models.Brand
{
    public class BrandModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Details { get; set; }
        public Status Status { get; set; }

    }
}
