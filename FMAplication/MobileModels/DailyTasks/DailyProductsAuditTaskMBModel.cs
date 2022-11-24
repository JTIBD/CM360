using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Products;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyProductsAuditTaskMBModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ActionType ActionType { get; set; }
        public double Result { get; set; }
        public DailyAuditTaskMBModel DailyAuditTask { get; set; }
        public int DailyAuditTaskId { get; set; }
    }
}