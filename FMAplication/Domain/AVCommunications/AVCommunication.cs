using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Enumerations;

namespace FMAplication.Domain.AVCommunications
{
    public class AvCommunication : AuditableEntity<int>
    {
        public string CampaignName { get; set; }
        public AvCommunicationCampaignType CampaignType { get; set; }

        public Brand.Brand Brand { get; set; }
        public int BrandId { get; set; }

        public string Description { get; set; }
        public string FilePath { get; set; }

    }


}
