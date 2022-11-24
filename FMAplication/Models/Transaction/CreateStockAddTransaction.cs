using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class CreateStockAddTransaction
    {
        [Required]
        public string WareHouseCode { get; set; }
        public string Remarks { get; set; }
        [Required]
        public List<StockAddTransactionModel> POSM_Products { get; set; }
    }
}
