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
    public class DailyCMActivitySPModel
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
        public bool IsAudit { get; set; }
        public bool IsSurvey { get; set; }
        public bool IsPOSM { get; set; }
        public bool IsConsumerSurveyActive { get; set; }
        public int Status { get; set; }
        public string DisplayIsAudit { get; set; }
        public string DisplayIsSurvey { get; set; }
        public string DisplayIsPOSM { get; set; }
        public string DisplayIsConsSurAct { get; set; }
        public string DisplayStatus { get; set; }
    }
}