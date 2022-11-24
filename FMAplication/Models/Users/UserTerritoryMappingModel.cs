using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;

namespace FMAplication.Models.Users
{
    public class UserTerritoryMappingModel
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public Node Territory { get; set; }

        public int UserInfoId { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
