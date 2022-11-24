using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.DailyActivity
{
    public class TaskCreationData
    {
        public List<string> CMUserNames { get; set; }
        public List<int> CMUserIds { get; set; }
        public List<string> PosmNames { get; set; }

        public List<string> Headers { get; set; }

        public List<DateTime> Dates { get; set; }
    }
}
