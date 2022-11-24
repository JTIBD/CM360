using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Models
{
    public class LoginModel
    {
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
