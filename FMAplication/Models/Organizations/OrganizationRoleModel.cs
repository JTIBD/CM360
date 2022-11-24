using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;

namespace FMAplication.Models.Organizations
{
    public class OrganizationRoleModel
    {
        public OrganizationRoleModel()
        {
            this.ConfigList = new List<OrganizationRoleMappingModel>();
            this.UserList = new List<int>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public Status Status { get; set; }
        public int DesignationId { get; set; }
        public List<int> UserList { get; set; }

        public List<OrganizationRoleMappingModel> ConfigList;
    }
}
