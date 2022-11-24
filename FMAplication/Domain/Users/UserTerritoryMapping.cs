using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Domain.Sales;

namespace FMAplication.Domain.Users
{

    public class UserTerritoryMapping : AuditableEntity<int>
    {
        public int NodeId { get; set; }

        //[ForeignKey("NodeId")]
        //public Node Territory { get; set; }


        public int UserInfoId { get; set; }


        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }
    }
}
