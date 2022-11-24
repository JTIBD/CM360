using FMAplication.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels
{
    public class GetSalesPointAdjustmentTransactions:PaginationParams
    {
        public int TransactionStatus { get; set; }=-1;

    }
}
