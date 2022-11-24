using System;
using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Products;

namespace fm_application.Models.DailyActivity
{
    public class SurveyReportSPModel
    {
        public DateTime Date { get; set; }
        public string DisplayDate { get; set; }
        public int AssignedFMUserId { get; set; }
        public int CMId { get; set; }
        public string FMUserName { get; set; }
        public string CMUserName { get; set; }
        public string OutletName { get; set; }
        public string SalesPointName { get; set; }
        public string SurveyName { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public bool IsConsumerSurvey { get; set; }
        public int Status { get; set; }
        public string DisplayIsConsumerSurvey { get; set; }
        public string DisplayStatus { get; set; }
    }
}