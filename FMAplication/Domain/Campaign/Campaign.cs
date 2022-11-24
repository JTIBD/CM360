using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;

namespace FMAplication.Domain.Campaign
{
    public class Campaign: AuditableEntity<int>
    {
        //public int CampaignId { get; set; }   
        [StringLength(255)]
        public string CampaignName { get; set; }  
        [StringLength(Int32.MaxValue)]
        public string CampaignDetails { get; set; }

        public DateTime StartDate { get; set; }            
        public DateTime EndDate { get; set; }

        public IList<CampaignHistory> CampaignHistories { get; set; }
    }
}
