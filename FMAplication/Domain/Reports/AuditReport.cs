using FMAplication.Core;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Reports
{

    public class AuditReport : AuditableEntity<int>
    {
        public int DailyAuditId { get; set; }        
        public int DailyCMActivityId { get; set; }
        public int? ProductId { get; set; }
        public int? POSMProductId { get; set; }
        public int Amount { get; set; }
        public ActionType ActionType { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl1 { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl2 { get; set; }
        public Product Product { get; set; }
        public POSMProduct POSMProduct { get; set; }
        public DailyCMActivity DailyCMActivity { get; set; }
    }
}
