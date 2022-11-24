using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.Questions
{
    public class QuestionOptionMBModel
    {
        public int Id { get; set; }
        public string OptionTitle { get; set; }
        public string OptionEmoticon { get; set; }
        public int Sequence { get; set; }
    }
}
