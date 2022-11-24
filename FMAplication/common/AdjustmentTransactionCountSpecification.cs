using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core.Params;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;

namespace FMAplication.common
{
    public class AdjustmentTransactionCountSpecification : BaseSpecification<Transaction>
    {
        public AdjustmentTransactionCountSpecification(AdjustmentTransactionParams transactionParams)
            : base(x =>
                (string.IsNullOrEmpty(transactionParams.Search) || x.TransactionNumber.ToLower().Equals(transactionParams.Search.ToLower())) &&
                x.TransactionType == TransactionType.StockAdjustment && x.TransactionDate >= transactionParams.FromDate && x.TransactionDate <= transactionParams.ToDate &&
                (!transactionParams.TransactionStatus.HasValue || x.TransactionStatus == transactionParams.TransactionStatus)
            )



        {
        }
    }
}
