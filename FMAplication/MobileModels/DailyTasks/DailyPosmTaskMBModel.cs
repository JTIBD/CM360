using System;
using System.Collections.Generic;
using System.Xml;
using FMAplication.Attributes;
using FMAplication.Enumerations;
using FMAplication.Helpers;
using FMAplication.MobileModels.Reasons;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyPosmTaskMBModel : DailyTaskBaseMbModel
    {
        public DailyPosmTaskMBModel()
        {
            DailyPosmTaskItems = new List<DailyPosmTaskItemsMBModel>();
        }
        public DailyTaskMBModel DailyTask { get; set; }

        public string ExistingImage { get; set; }

        public string NewImage { get; set; }

        [ValidDate]
        public string CheckInStr { get; set; }
        [ValidDate]
        public string CheckOutStr { get; set; }


        public List<DailyPosmTaskItemsMBModel> DailyPosmTaskItems { get; set; }

        
    }
}
