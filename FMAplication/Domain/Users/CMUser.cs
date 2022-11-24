using System;
using System.Collections.Generic;
using FMAplication.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Enumerations;
using Microsoft.VisualBasic;

namespace FMAplication.Domain.Users
{

    public class CMUser : AuditableEntity<int>
    {
        public CMUser()
        {
            CmsUserSalesPointMappings = new List<CmsUserSalesPointMapping>();
        }
        public CMUserType UserType { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string AltCode { get; set; }
        [StringLength(128)]
        public string PhoneNumber { get; set; }
        [StringLength(128)]
        public string Email { get; set; }

        public string NIdBirthCertificate { get; set; }
        public DateTime? JoiningDate { get; set; }

        [StringLength(256)]
        public string Password { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string FamilyContactNo { get; set; }

        [StringLength(256)] 
        public string Designation { get; set; }

        public ICollection<CmsUserSalesPointMapping> CmsUserSalesPointMappings { get; set; }
    }

    public class CmUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }




}
