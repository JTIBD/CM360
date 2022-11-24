using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class UpdateStockAddTransaction
    {
        public string Remarks { get; set; }
        [Required]
        public List<StockAddTransactionModel> StockAddTransactions { get; set; }
    }
}
