using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using FMAplication.Core;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailySurveyTask : DailyBaseTask
    {
        public SurveyQuestionSet SurveyQuestionSet { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public List<DailySurveyTaskAnswer> DailySurveyTaskAnswers { get; set; }

    }
}