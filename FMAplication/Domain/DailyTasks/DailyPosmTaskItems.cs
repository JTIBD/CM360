 using FMAplication.Core;
using FMAplication.Domain.Products;
 using FMAplication.Enumerations;

 namespace FMAplication.Domain.DailyTasks
{
    public class DailyPosmTaskItems : AuditableEntity<int>
    {
        public DailyPosmTask DailyPosmTask { get; set; }
        public int DailyPosmTaskId { get; set; }
        public POSMProduct PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public int Quantity { get; set; }
        public PosmWorkType ExecutionType { get; set; }
        public string Image { get; set; }
    }
}