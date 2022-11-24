using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Domain.Users
{

    public class UserRoleMapping :AuditableEntity<int>
    {      
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

       
        public int UserInfoId { get; set; }


        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }      
    }
}
