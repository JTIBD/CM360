using System.Collections.Generic;

namespace FMAplication.MobileModels.Questions
{
    public class QuestionMBModel
    {
        public int Id { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionType { get; set; }

        public ICollection<QuestionOptionMBModel> QuestionOptions { get; set; }
            = new List<QuestionOptionMBModel>();
    }
}