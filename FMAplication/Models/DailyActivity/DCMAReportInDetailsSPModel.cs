using System;
using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Products;

namespace fm_application.Models.DailyActivity
{
    public class DCMAReportInDetailsSPModel
    {
        public DateTime Date { get; set; }
        public string DisplayDate { get; set; }
        public int AssignedFMUserId { get; set; }
        public int CMId { get; set; }
        public string FMUserName { get; set; }
        public string CMUserName { get; set; }
        public string OutletName { get; set; }
        public string SalesPointName { get; set; }
        public string RouteName { get; set; }
        public string TerritorryName { get; set; }
        public string AreaName { get; set; }
        public string RegionName { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int ProductType { get; set; }
        public int ActionType { get; set; }
        public int DCMAStatus { get; set; }
        public int POSMReportStatus { get; set; }
        public string DisplayProductType { get; set; }
        public string DisplayActionType { get; set; }
        public string DisplayDCMAStatus { get; set; }
        public string DisplayPOSMReportStatus { get; set; }
    }
}