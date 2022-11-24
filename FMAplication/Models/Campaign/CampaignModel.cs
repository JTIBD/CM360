using FMAplication.Domain.Campaign;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Campaign
{
    public class CampaignModel
    {
        public int Id { get; set; }
        //public int CampaignId { get; set; }
        [StringLength(255)]
        public string CampaignName { get; set; }
        [StringLength(Int32.MaxValue)]
        public string CampaignDetails { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IList<CampaignHistoryModel> CampaignHistories { get; set; }
    }
}
