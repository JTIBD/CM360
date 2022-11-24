using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.DailyActivities;
using FMAplication.Enumerations;
using FMAplication.Models.Products;

namespace fm_application.Models.DailyActivity
{
    public class AuditReportModel
    {
        public int Id { get; set; }
        public int DailyAuditId { get; set; }        
        public int DailyCMActivityId { get; set; }
        public int CMId { get; set; }
        public int? ProductId { get; set; }
        public int? POSMProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public ActionType ActionType { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl1 { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl2 { get; set; }
        public Status Status { get; set; }
        public string ProductImageUrl { get; set; }
        public ProductModel Product { get; set; }
        public POSMProductModel POSMProduct { get; set; }

        public DailyCMActivity DailyCMActivity { get; set; }
    }
}