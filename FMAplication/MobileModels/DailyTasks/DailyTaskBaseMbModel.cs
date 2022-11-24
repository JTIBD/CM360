using System;
using FMAplication.Attributes;
using FMAplication.MobileModels.Reasons;
using FMAplication.MobileModels.Sales;


namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyTaskBaseMbModel
    {

        public int Id { get; set; }
        public int DailyTaskId { get; set; }
        public int OutletId { get; set; }
        public OutletMBModel Outlet { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOutletOpen { get; set; }
        public ReasonsMBModel Reason { get; set; }
        public int? ReasonId { get; set; }

       
    }
}
