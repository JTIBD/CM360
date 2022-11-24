using FMAplication.Core.Params;
using FMAplication.Domain.Products;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;

namespace FMAplication.common
{
    public class AdjustmentTransactionWithIncludesSpecification : BaseSpecification<Transaction>
    {
        public AdjustmentTransactionWithIncludesSpecification(AdjustmentTransactionParams transactionParams)
            : base(x =>
                (string.IsNullOrEmpty(transactionParams.Search) || x.TransactionNumber.ToLower().Equals(transactionParams.Search.ToLower())) &&
                x.TransactionType == TransactionType.StockAdjustment && x.TransactionDate.Date >= transactionParams.FromDate.Date && x.TransactionDate.Date <= transactionParams.ToDate.Date &&
                (!transactionParams.TransactionStatus.HasValue || x.TransactionStatus == transactionParams.TransactionStatus)
            )
        {
            AddInclude(x => x.WareHouse);
            AddOrderByDescending(x => x.TransactionDate);
            ApplyPaging(transactionParams.PageSize * (transactionParams.PageIndex - 1), transactionParams.PageSize);

            if (!string.IsNullOrEmpty(transactionParams.Sort))
            {
                switch (transactionParams.Sort)
                {
                    case "TransactionStatus":
                        AddOrderBy(p => p.TransactionStatus);
                        break;
                    case "TransactionDate":
                        AddOrderByDescending(p => p.TransactionDate);
                        break;
                    default:
                        AddOrderBy(n => n.TransactionNumber);
                        break;
                }
            }
        }
    }
}