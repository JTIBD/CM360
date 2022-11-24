using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Domain.Menus;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Users
{
    public class Role : AuditableEntity<int>
    {
        [UniqueKey]
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
        public List<UserRoleMapping> Users { get; set; }
        public List<MenuPermission> Permissions { get; set; }

    }
}
