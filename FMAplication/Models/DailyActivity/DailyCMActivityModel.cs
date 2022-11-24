using System.Collections.Generic;
using System;
using FMAplication.Enumerations;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.DailyAudit;
using FMAplication.Models.Sales;
using System.ComponentModel.DataAnnotations;
using FMAplication.Models.Questions;
using FMAplication.Models.Users;

namespace fm_application.Models.DailyActivity
{
    public class DailyCMActivityModel
    {
        public int Id { get; set; }
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public int CMId { get; set; }
        public DateTime Date { get; set; }
        public string DateStr { get; set; }
        public int AssignedFMUserId { get; set; }
        public bool IsAudit { get; set; }
        public bool IsSurvey { get; set; }
        public bool IsPOSM { get; set; }
        public bool IsConsumerSurveyActive { get; set; }
        public Status Status { get; set; }
        public string OutletOwnerName { get; set; }
        public string OutletAddress { get; set; }
        public string OutletContactNumber { get; set; }
        public OutletModel Outlet { get; set; }
        public DailyPOSMModel DailyPOSM { get; set; } = new DailyPOSMModel();
        public DailyAuditModel DailyAudit { get; set; } = new DailyAuditModel();
        public List<SurveyReporModel> SurveyQuestions { get; set; } = new List<SurveyReporModel>();
        public List<SurveyQuestionSetModel> Surveys { get; set; } = new List<SurveyQuestionSetModel>();
        public List<SurveyQuestionSetModel> ConsumerSurveys { get; set; } = new List<SurveyQuestionSetModel>();
        public List<SurveyQuestionSetModel> AllSurveys { get; set; } = new List<SurveyQuestionSetModel>();

        public UserInfoModel AssignedFMUser { get; set; }
        public CMUserRegisterModel CM { get; set; }

        // this property used for dashboard

        public string CompletedPercentage { get; set; }
        public string POSMPercentage { get; set; }
        public string SurveyPercentage { get; set; }
        public string AuditPercentage { get; set; }


    }

}