using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMAplication.Models.Sales
{

    public class ChannelModel
    {
        public int ChannelID { get; set; }
        public int ParamID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public int ParentID { get; set; }

    }
}
