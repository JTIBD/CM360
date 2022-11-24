using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Enumerations
{
    public enum TransactionStatus
    {
        Pending,
        WaitingForApproval,
        Completed,
        WaitingForReceive,
    }

    public enum TransactionType
    {
        StockAdd,
        StockAdjustment,
        Distribute, 
        Receive,
        SalesPointStockAdjustment,
        CW_Transfer,
        CW_Receive,
        SP_Transfer,
        SP_Receive,
    }
    
    public enum PosmTaskAssignStatus
    {
        Confirmed,
        Downloaded,
        Uploaded, 
        Completed
    }
}
