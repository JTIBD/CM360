using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.DailyActivities;
using FMAplication.Enumerations;
using FMAplication.Models.Products;

namespace fm_application.Models.DailyActivity
{
    public class POSMReportModel
    {
        public int Id { get; set; }
        public int DailyPOSMId { get; set; }
        public int DailyCMActivityId { get; set; }
        public int CMId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public POSMActionType ActionType { get; set; }
        public POSMProductType ProductType { get; set; }
        public string ProductPlanogramImageUrl { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl1 { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl2 { get; set; }
        public POSMProductModel Product { get; set; }

        public Status Status { get; set; }
        public DailyCMActivity DailyCMActivity { get; set; }
    }
}