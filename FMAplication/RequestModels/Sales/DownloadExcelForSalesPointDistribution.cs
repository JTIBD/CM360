using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.RequestModels.Sales
{
    public class DownloadExcelForSalesPointDistribution
    {
        [Required]
        public int FromSalesPointId { get; set; }
        [Required]
        public List<int> ToSalesPointIds { get; set; }
    }
}
