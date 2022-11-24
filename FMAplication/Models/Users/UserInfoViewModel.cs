using FMAplication.Attributes;
using FMAplication.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Models.Users
{
    public class UserInfoViewModel
    {

        public int Id { get; set; }
        [Required]

        public string EmployeeId { get; set; }

    
        [Required]
        [StringLength(128,MinimumLength =3)]
        public string Code { get; set; }

        [Required]
      
        public string Name { get; set; }

       
        public string PhoneNumber { get; set; }


        
        public string Designation { get; set; }

        public int SalesPointId { get; set; }
    }
}
