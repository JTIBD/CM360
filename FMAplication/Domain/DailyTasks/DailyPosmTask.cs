using System.Collections.Generic;
using FMAplication.Core;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyPosmTask : DailyBaseTask
    {
        public string ExistingImage { get; set; }
        public string NewImage { get; set; }
        public List<DailyPosmTaskItems> DailyPosmTaskItems { get; set; }
    }
}