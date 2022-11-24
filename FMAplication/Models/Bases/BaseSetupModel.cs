using System;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Sales;

namespace FMAplication.Models.Bases
{
    public class BaseSetupModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public AssignedUserType UserType { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateStr => FromDate.ToIsoString();
        public DateTime ToDate { get; set; }
        public string ToDateStr => ToDate.ToIsoString();
        public Status Status { get; set; }
    }
}