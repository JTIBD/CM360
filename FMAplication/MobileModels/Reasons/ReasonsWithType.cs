using System.Collections.Generic;

namespace FMAplication.MobileModels.Reasons
{
    public class ReasonsWithType
    {
        public string Type { get; set; }
        public List<ReasonsMBModel> ReasonInType { get; set; }
    }
}