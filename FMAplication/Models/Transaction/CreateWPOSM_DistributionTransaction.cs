using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class CreateWPOSM_DistributionTransaction
    {
        public CreateWPOSM_DistributionTransaction()
        {
            WDistributionTransactionProducts = new List<WDistributionTransactionProduct>();
            WareHouseCode = "";
        }

        [Required]
        public string WareHouseCode { get; set; }
        public List<WDistributionTransactionProduct> WDistributionTransactionProducts { get; set; }
    }


    public class WDistributionTransactionProduct
    {
        public string POSM_Name { get; set; }
        public string SalesPointCode { get; set; }
        public int Quantity { get; set; }
    }
}
