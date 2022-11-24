using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.Users;

namespace FMAplication.Domain.Organizations
{

    public class OrganizationUserRole : AuditableEntity<int>
    {
        public int OrgRoleId { get; set; }

        [ForeignKey("OrgRoleId")]
        public OrganizationRole OrgRole { get; set; }

        public int UserId { get; set; }
        public int UserSequence { get; set; }
        [ForeignKey("UserId")]
        public UserInfo UserInfo { get; set; }

        public int DesignationId { get; set; }

    }

}
