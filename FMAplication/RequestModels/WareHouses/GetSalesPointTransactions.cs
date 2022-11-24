using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Attributes;
using FMAplication.Enumerations;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels.WareHouses
{
    public class GetSalesPointTransactions:PaginationParams
    {
        public int TransactionStatus { get; set; }
    }
}
