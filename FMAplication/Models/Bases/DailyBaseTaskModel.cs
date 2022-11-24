using System;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;

namespace FMAplication.Models.Bases
{
    public class DailyBaseTaskModel: IWithOutlet
    {
        public int Id { get; set; }        
        public int DailyTaskId { get; set; }
        public int OutletId { get; set; }
        public OutletModel Outlet { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOutletOpen { get; set; }
        public ReasonModel Reason { get; set; }
        public int? ReasonId { get; set; }

        public string CheckInStr { get; set; }
        public string CheckOutStr { get; set; }
    }
}