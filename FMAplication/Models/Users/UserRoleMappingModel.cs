﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Users
{
    public class UserRoleMappingModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }


       // public RoleModel Role { get; set; }


        public int UserInfoId { get; set; }


        //public UserInfoModel UserInfo { get; set; }

    }
}
