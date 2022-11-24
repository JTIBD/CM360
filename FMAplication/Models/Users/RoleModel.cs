using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Enumerations;

namespace FMAplication.Models.Users
{
    public class RoleModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public Status Status { get; set; }
    }
}