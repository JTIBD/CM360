using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyInformationTask : DailyBaseTask
    {
        public string InsightImage { get; set; }
        public string InsightDescription { get; set; }

        public string RequestImage { get; set; }
        public string RequestDescription { get; set; }
    }
}
