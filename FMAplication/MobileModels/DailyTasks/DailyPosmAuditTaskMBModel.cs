using System.Security.AccessControl;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Products;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyPosmAuditTaskMBModel
    {
        public POSMProductMBModel PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public double Result { get; set; }
        public ActionType ActionType { get; set; }

        public DailyAuditTaskMBModel DailyAuditTask { get; set; }
        public int DailyAuditTaskId { get; set; }
    }
}