using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Models.Users
{
    public class  CMUserRegisterModel:CMUserModel
    {
        public CMUserRegisterModel()
        {
            SalesPointIds = new List<int>();
        }

        [Required]
        public string Password { get; set; }
    }

    public class UserDataErrorViewModel
    {
        public int Row { get; set; }
        public string ColumnName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
