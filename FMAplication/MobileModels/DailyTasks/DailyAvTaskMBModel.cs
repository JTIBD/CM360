using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Reasons;
using TaskStatus = FMAplication.Enumerations.TaskStatus;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyAvTaskMBModel : DailyTaskBaseMbModel
    {
       
        public DailyTaskMBModel DailyTask { get; set; }
       
        public int AvSetupId { get; set; }
    }
}
