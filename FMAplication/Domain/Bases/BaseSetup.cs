using System;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Extensions;

namespace FMAplication.Domain.Bases
{
    public class BaseSetup:AuditableEntity<int>
    {
        public int SalesPointId { get; set; }
        public AssignedUserType UserType { get; set; }
        public DateTime FromDate { get; set; }
        [NotMapped]
        public string FromDateStr => FromDate.ToIsoString();
        public DateTime ToDate { get; set; }
        [NotMapped]
        public string ToDateStr => ToDate.ToIsoString();
    }
}