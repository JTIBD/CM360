using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Enumerations;
using FMAplication.Helpers;
using FMAplication.MobileModels.Reasons;
using Newtonsoft.Json;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyInformationTaskMBModel : DailyTaskBaseMbModel
    {
        public DailyInformationTaskViewModel Insight { get; set; }
        public DailyInformationTaskViewModel Request { get; set; }

    }
    public class DailyInformationTaskViewModel
    {
        public string Image { get; set; }


        public string Description { get; set; }
    }

}
