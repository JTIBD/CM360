using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.RequestModels.WareHouses
{
    public class DownloadExcelForWareHouseDistribution
    {
        [Required]
        public int FromWareHouseId { get; set; }
        [Required]
        public int ToWareHouseId { get; set; }        
    }
}
