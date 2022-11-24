using FMAplication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Domain.Campaign
{
    public class CampaignHistory : AuditableEntity<int>
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
