using FMAplication.MobileModels.Products;

namespace FMAplication.Models.PosmAssign
{
    public class PosmTaskMBModel
    {
        public POSMProductMBModel PosmProduct { get; set; }
        public int Quantity { get; set; }
        public int DailyTaskId { get; set; }
        public int SalesPointId { get; set; }
    }
}