using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.DailyActivity
{
    public class BatchStatusChangeModel
    {
        BatchStatusChangeModel(){
            this.CMIdList = new List<int>();
        }
        public List<int> CMIdList { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}
