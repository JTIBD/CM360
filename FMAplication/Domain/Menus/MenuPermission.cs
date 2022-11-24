using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.Users;

namespace FMAplication.Domain.Menus
{
    public class MenuPermission : AuditableEntity<int>
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
    }
}
