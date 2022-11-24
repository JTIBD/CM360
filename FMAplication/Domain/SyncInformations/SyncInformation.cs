using FMAplication.Core;
using FMAplication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.SyncInformations
{
    public class SyncInformation : AuditableEntity<int>
    {
        public DateTime LastSyncTime { get; set; }
        public string LastSyncTimeStr => LastSyncTime.ToIsoString();        
    }
}
