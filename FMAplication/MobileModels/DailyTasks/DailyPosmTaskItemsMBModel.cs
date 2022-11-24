
using FMAplication.Enumerations;
using FMAplication.Helpers;
using FMAplication.MobileModels.Products;
using Newtonsoft.Json;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailyPosmTaskItemsMBModel
    {
        public int Id { get; set; }
        public DailyPosmTaskItemsMBModel DailyPosmTask { get; set; }
        public int DailyPosmTaskId { get; set; }


        public POSMProductMBModel PosmProduct { get; set; }
        public int PosmProductId { get; set; }


        public int Quantity { get; set; }
        public PosmWorkType ExecutionType { get; set; }
        public string Image { get; set; }
    }
}
