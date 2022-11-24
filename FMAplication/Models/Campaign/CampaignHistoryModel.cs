using System;

namespace FMAplication.Models.Campaign
{
    public class CampaignHistoryModel
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
