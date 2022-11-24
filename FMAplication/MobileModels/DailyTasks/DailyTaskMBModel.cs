using System;
using FMAplication.Domain.Users;
using FMAplication.MobileModels.Sales;
using FMAplication.Models.Sales;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyTaskMBModel
    {
        public int Id { get; set; }

        public CmUserModel CmUser { get; set; }
        public int CmUserId { get; set; }

        public SalesPointMBModel SalesPoint { get; set; }
        public int SalesPointId { get; set; }
        public string DateTimeStr { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class DailyTaskSubmitModel
    {
        public int DailyTaskId { get; set; }
    }
}
