using FMAplication.Domain.Menus;
using FMAplication.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Models.Menus
{
    public class ActivityPermissionModel
    {
        public int MenuId { get; set; }
        public string RoleName { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string Url { get; set; }

        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanInsert { get; set; }
        public bool CanDelete { get; set; }
    }
}
