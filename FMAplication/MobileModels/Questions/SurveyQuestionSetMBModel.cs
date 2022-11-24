using FMAplication.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.Questions
{
    public class SurveyQuestionSetMBModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionMBModel> Questions { get; set; }
    }
}
