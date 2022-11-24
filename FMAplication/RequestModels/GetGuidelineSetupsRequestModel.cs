using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.RequestModels.Bases;

namespace FMAplication.RequestModels
{
    public class GetGuidelineSetupsRequestModel:PaginationParams
    {
        public int SalesPointId { get; set; }
    }
}
