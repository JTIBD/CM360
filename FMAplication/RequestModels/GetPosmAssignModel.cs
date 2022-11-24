using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Attributes;

namespace FMAplication.RequestModels
{
    public class GetPosmAssignModel
    {
        public int CmUserId { get; set; }
        public int SalesPointId { get; set; }

        [UseUtc]
        public DateTime Date { get; set; }
    }
}
