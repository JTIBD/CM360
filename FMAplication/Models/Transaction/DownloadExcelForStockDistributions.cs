using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class DownloadExcelForStockDistributions
    {
        [Required]
        public int WareHouseId { get; set; }
        [Required]
        public List<int> SalesPointIds { get; set; }
    }
}
