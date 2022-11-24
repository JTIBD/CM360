using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Reasons;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyAuditTaskMBModel : DailyTaskBaseMbModel
    {
        public DailyTaskMBModel DailyTask { get; set; }
        public int AuditSetupId { get; set; }
        

        public List<DailyProductsAuditTaskMBModel> DailyProductsAudit { get; set; }
        public List<DailyPosmAuditTaskMBModel> DailyPosmAudits { get; set; }

        public DailyAuditTaskMBModel()
        {
            DailyProductsAudit = new List<DailyProductsAuditTaskMBModel>();
            DailyPosmAudits = new List<DailyPosmAuditTaskMBModel>();
        }
    }
}
