using FMAplication.common;
using FMAplication.MobileModels.Surveys;
using FMAplication.Models.Common;
using FMAplication.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;

namespace FMAplication.Services.Surveys.interfaces
{
    public interface ISurveyService
    {
        Task<List<SurveyModel>> Create(List<SurveyModel> payload);
        Task<Pagination<SurveyModel>> GetSurveys(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salesPointId);
        Task<SurveyModel> GetSurveyById(int id);
        Task<SurveyModel> UpdateSurvey(SurveyModel payload);
        Task<List<SurveyModel>> GetExistingSurveys(List<SurveyModel> payload);
        Task<List<SurveyMBModel>> GetSurveysOfTodayByUser(int userId);
        Task<List<SurveyMBModel>> GetSurveysByTask(DailyTask task);
        Task<List<ConsumerSurveyMBModel>> GetConsumerSurveysOfTodayByUser(int userId);
        Task<List<ConsumerSurveyMBModel>> GetConsumerSurveysByTask(DailyTask task);
        Task<bool> IsSurveyActive(SurveyModel model);
    }
}
