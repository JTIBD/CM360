using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels.Reports
{
    public class CWStockUpdateRequestParams:PaginationParams
    {
        public List<int> CWIds { get; set; }
    }
}
