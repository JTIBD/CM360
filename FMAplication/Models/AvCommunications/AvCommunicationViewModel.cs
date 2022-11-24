using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Enumerations;
using FMAplication.Helpers;
using FMAplication.Models.Brand;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FMAplication.Models.AvCommunications
{
    public class AvCommunicationViewModel
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public BrandModel BrandModel { get; set; }  
        public int BrandId { get; set; }
        public AvCommunicationCampaignType CampaignType { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        public bool IsEditable { get; set; } = true;
        public bool IsDeletable { get; set; } = true;
    }
}
