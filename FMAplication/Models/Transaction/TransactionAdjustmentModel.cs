using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Enumerations;
using FMAplication.Models.Sales;
using FMAplication.Models.TransactionNotifications;
using FMAplication.Models.TransactionWorkflow;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.Transaction
{
    public class TransactionAdjustmentModel 
    {

        public TransactionAdjustmentModel()
        {
            StockAdjustmentItems = new List<StockAdjustmentItemsModel>();
            TransactionDate = DateTime.UtcNow;
            Remarks = "";
        }

        public int Id { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr { get; set; }
        public string TransactionNumber { get; set; }
        public WareHouseModel WareHouseModel { get; set; }
        public int WarehouseId { get; set; }
        public string Remarks { get; set; }

        public int Lines { get; set; }
        public int TotalIncrease { get; set; }
        public int TotalDecrease { get; set; }

        public TransactionWorkflowModel TransactionWorkflow { get; set; }
        public TransactionNotificationModel TransactionNotification { get; set; }
        public ICollection<StockAdjustmentItemsModel> StockAdjustmentItems { get; set; }
    }


    

}
