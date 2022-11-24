using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;

namespace FMAplication.Domain.Sales
{
    public class Channel:AuditableEntity<int>
    {
        public int ChannelID { get; set; }
        public int ParamID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public int ParentID { get; set; }

    }
}
