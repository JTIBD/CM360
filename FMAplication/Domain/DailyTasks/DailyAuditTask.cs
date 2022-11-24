using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Audit;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyAuditTask : DailyBaseTask
    {
        public int AuditSetupId { get; set; }
        public ICollection<DailyProductsAuditTask> DailyProductsAuditTask { get; set; }

        public DailyAuditTask()
        {
            DailyProductsAuditTask = new List<DailyProductsAuditTask>();
        }
    }
}
