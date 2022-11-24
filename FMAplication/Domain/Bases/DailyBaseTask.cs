using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using FMAplication.Core;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Extensions;

namespace FMAplication.Domain.Bases
{
    public class DailyBaseTask:AuditableEntity<int>
    {
        public DailyTask DailyTask { get; set; }
        public int DailyTaskId { get; set; }
        public int OutletId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOutletOpen { get; set; }
        public Reason Reason { get; set; }
        public int? ReasonId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        [NotMapped] public string CheckInStr => CheckIn.ToIsoString();
        [NotMapped] public string CheckOutStr => CheckOut.ToIsoString();


    }
}