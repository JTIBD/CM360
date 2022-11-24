using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Common
{
    public class CustomObject<T>
    {
        [Required]
        public T Data { get; set; }
    }
}
