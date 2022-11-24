using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMAplication.Enumerations;

namespace FMAplication.Models.Users
{
    public class CMUserModel
    {
        public CMUserModel()
        {
            SalesPointIds = new List<int>();
        }

        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name must be greater than 2 letters")]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string FamilyContactNo { get; set; }
        public Status Status { get; set; }
        public string AltCode { get; set; }
        public string Designation { get; set; }
        public CMUserType UserType { get; set; }
        public string NidBirthCertificate { get; set; }
        public DateTime? JoiningDate { get; set; }
        public List<int> SalesPointIds { get; set; }
    }
}