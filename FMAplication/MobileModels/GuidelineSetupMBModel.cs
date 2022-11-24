using System;
using FMAplication.Extensions;
using Newtonsoft.Json;

namespace FMAplication.MobileModels
{
    public class GuidelineSetupMBModel
    {
        public string Code { get; set; }
        public string GuidelineText { get; set; }
        public int SalesPointId { get; set; }
        [JsonIgnore]
        public DateTime CreatedTime { get; set; }
        public string CreatedTimeStr => CreatedTime.ToIsoString();
        [JsonIgnore]
        public DateTime? ModifiedTime { get; set; }
        public string ModifiedTimeStr =>  ModifiedTime?.ToIsoString() ?? "";
    }
}
