using FMAplication.Core;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Domain.Reports
{
    public class POSMReport : AuditableEntity<int>
    {
        public int DailyPOSMId { get; set; }
        public int DailyCMActivityId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public POSMActionType ActionType { get; set; }
        public POSMProductType ProductType { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl1 { get; set; }
        //[StringLength(512)]
        public string UploadedImageUrl2 { get; set; }
        public POSMProduct Product { get; set; }

        public DailyCMActivity DailyCMActivity { get; set; }
    }
}
