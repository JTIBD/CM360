using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.PosmAssign
{
    public class ExportPosmAssignViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool ExcludeFriday { get; set; }
        public int SalesPointId { get; set; }


    }
}
