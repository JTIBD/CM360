using FMAplication.common;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Surveys;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.MobileModels.Questions;
using FMAplication.MobileModels.Surveys;
using FMAplication.Models.Common;
using FMAplication.Models.Questions;
using FMAplication.Models.Survey;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.Surveys.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FMAplication.Services.Surveys.implementation
{
    public class ServeyService : ISurveyService
    {
        private readonly IRepository<Survey> _survey;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<SurveyQuestionCollection> _surveyQuestionCollection;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<QuestionOption> _questionOption;
        private readonly ICommonService _common;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailySurveyTask> _dailySurveyTask;
        private static readonly List<string> questionTypesHavingMinimumOneActiveOption = new List<string>()
        {
            QuestionTypes.MultipleChoice.GetDescription(),QuestionTypes.SingleChoice.GetDescription(),QuestionTypes.Dropdown.GetDescription(),
            QuestionTypes.Emo.GetDescription(), QuestionTypes.Rating.GetDescription(),QuestionTypes.Slider.GetDescription()
        };

        public ServeyService(IRepository<Survey> survey, IRepository<SalesPoint> salesPoint, IRepository<CMUser> cmUser,
            IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping, IRepository<SurveyQuestionCollection> surveyQuestionCollection,
            IRepository<QuestionOption> questionOption, ICommonService common,
            IRepository<DailyTask> dailyTask, IRepository<DailySurveyTask> dailySurveyTask)
        {
            _survey = survey;
            _salesPoint = salesPoint;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _surveyQuestionCollection = surveyQuestionCollection;
            _questionOption = questionOption;
            _common = common;
            _dailyTask = dailyTask;
            _dailySurveyTask = dailySurveyTask;
        }

        public async Task<bool> IsSurveyActive(SurveyModel model)
        {
            return _survey.IsExist(s => s.Id == model.Id && s.Code == model.Code && s.Status == Status.Active);
        }

        public async Task<List<SurveyModel>> Create(List<SurveyModel> payload)
        {
            if (payload is null || payload.Count == 0) throw new AppException("No survey provided");
            List<SurveyModel> list = new List<SurveyModel>();

            DateTime fromDate = payload[0].FromDate;
            DateTime toDate = payload[0].ToDate;

            _common.ValidateDateRange(fromDate, toDate);

            var salesPointIds = payload.Select(x => x.SalesPointId).ToList();

            var salespoints = (await _salesPoint.FindAllAsync(x => salesPointIds.Contains(x.SalesPointId))).ToList();

            List<Survey> surveys = new List<Survey>();

            var user = AppIdentity.AppUser;

            foreach (var s in payload)
            {
                var sp = salespoints.Find(x => x.SalesPointId == s.SalesPointId);
                Survey sv = new Survey()
                {
                    Code = $"SV_{sp.Code}",
                    CreatedBy = user.UserId,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate,
                    SalesPointId = sp.SalesPointId,
                    SurveyQuestionSetId = s.SurveyQuestionSetId,
                    UserType = s.UserType,
                    IsConsumerSurvey = s.IsConsumerSurvey,
                };
                surveys.Add(sv);
            }

            //var existingSurveys = _survey.FindAll(x => salesPointIds.Contains(x.SalesPointId) && x.ToDate > FromDate).ToList();

            var existingSurveyModels = await GetExistingSurveys(payload);
            var existingSurveyIds = existingSurveyModels.Select(x => x.Id).ToList();
            var existingSurveys = _survey.FindAll(x => existingSurveyIds.Contains(x.Id)).ToList();

            if (existingSurveys.Count > 0)
            {
                var dateRangeViolatedSurveys = existingSurveys.FindAll(x => x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var conflictingSps = salespoints.FindAll(x => dateRangeViolatedSurveys.Any(v => x.SalesPointId == v.SalesPointId)).ToList();
                    var spNames = conflictingSps.Select(x => x.Name).ToList();
                    throw new AppException($"The date range conflicts with existing survey in {string.Join(",", spNames)}");
                }

                foreach (var srv in existingSurveys)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _survey.UpdateListAsync(existingSurveys);
            }

            await _survey.CreateListAsync(surveys);

            list = surveys.MapToModel();

            return list;
        }

        public async Task<List<SurveyModel>> GetExistingSurveys(List<SurveyModel> surveyes)
        {

            var isConsumerSurvey = surveyes[0].IsConsumerSurvey;
            var existingSurveys = await _common.GetExistingSetups(_survey, surveyes.Cast<BaseSetupModel>().ToList(), false);
            existingSurveys = existingSurveys.Where(x => x.IsConsumerSurvey == isConsumerSurvey).ToList();

            var salesPointIdOfExistingSetups = existingSurveys.Select(x => x.SalesPointId).ToList();
            var salesPointsOfExistingSetups = _salesPoint.GetAllActive()
                .Where(x => salesPointIdOfExistingSetups.Contains(x.SalesPointId)).ToList();
            if (existingSurveys.Any()) _common.HandleConflictingCase(surveyes[0].FromDate, surveyes[0].ToDate, existingSurveys, salesPointsOfExistingSetups);
            //_common.HandleUserTypeValidation(surveyes[0].UserType, existingSurveys, salesPointsOfExistingSetups);

            return existingSurveys.MapToModel();

        }

        public async Task<SurveyModel> GetSurveyById(int id)
        {
            var survey = await _survey.FindIncludeAsync(x => x.Id == id, i2 => i2.SurveyQuestionSet);
            if (survey is null) throw new AppException("Survey not found");
            var model = survey.MapToModel();
            model.SalesPoint = _salesPoint.GetAllActive().FirstOrDefault(x => x.SalesPointId == model.SalesPointId)
                ?.ToMap<SalesPoint, SalesPointModel>();
            return model;
        }

        public async Task<Pagination<SurveyModel>> GetSurveys(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salesPointId)
        {
            if (fromDateTime > toDateTime) (fromDateTime, toDateTime) = (toDateTime, fromDateTime);
            var query = _survey.GetAll().Where(x => x.FromDate >= fromDateTime && x.ToDate <= toDateTime)
                                            .Include(i => i.SurveyQuestionSet).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var sIds = _salesPoint.GetAllActive().Where(x => x.Code.Contains(search) || x.Name.Contains(search)).Select(x => x.SalesPointId)
                    .ToList();
                query = query.Where(x => sIds.Contains(x.SalesPointId) || x.SurveyQuestionSet.Name.Contains(search));
            }
            if (salesPointId != 0) query = query.Where(x => x.SalesPointId == salesPointId);

            query = query.OrderByDescending(x => x.CreatedTime);
            var surveys = await query.ToPagedListAsync(pageIndex, pageSize);
            var surveyModels = surveys.ToList().MapToModel();
            _common.InsertSalesPoints(surveyModels);
            Pagination<SurveyModel> paginatedList = new Pagination<SurveyModel>(pageIndex, pageSize, surveys.TotalItemCount, surveyModels);

            return paginatedList;
        }

        public async Task<List<SurveyMBModel>> GetSurveysOfTodayByUser(int userId)
        {
            var user = _cmUser.Find(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.GetAllActive().Where(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var surveys = _survey.FindAllInclude(x => x.Status == Status.Active && 
                                                      x.IsConsumerSurvey == false && sIds.Contains(x.SalesPointId) && 
                                                      x.SurveyQuestionSet.Status == Status.Active &&
                                                      x.FromDate <= todayDate && todayDate <= x.ToDate && 
                                                      (x.UserType == AssignedUserType.BOTH || x.UserType == userType),
                                                      i2 => i2.SurveyQuestionSet).ToList();
            var surveyModels = surveys.MapToMBModel();

            var srvSetIds = surveys.Select(x => x.SurveyQuestionSetId).ToList();
            var surveyQuestionMaps = _surveyQuestionCollection.FindAllInclude(x => x.Status == Status.Active && x.Question.Status == Status.Active && srvSetIds.Contains(x.SurveyId), i1 => i1.Question).ToList().MapToMBModel();
            var qIds = surveyQuestionMaps.Select(q => q.QuestionId).ToList();
            var qOptions = _questionOption.FindAll(x => qIds.Contains(x.QuestionId)  && x.Status == Status.Active).ToList();
            var questions = surveyQuestionMaps.Select(x => x.Question).ToList();
            foreach (var q in questions)
            {
                q.QuestionOptions = await qOptions.FindAll(x => x.Status == Status.Active &&  x.QuestionId == q.Id).ToMap<QuestionOption, QuestionOptionMBModel>().ToListAsync();
                if (q.QuestionType == QuestionTypes.Emo.GetDescription())
                {
                    q.QuestionOptions = q.QuestionOptions.OrderBy(x => x.Sequence).ToList();
                }
            }

            foreach (var srv in surveyModels)
            {
                srv.SurveyQuestionSet.Questions = await surveyQuestionMaps.FindAll(x =>  x.SurveyId == srv.SurveyQuestionSet.Id).Select(x => x.Question)
                    .Where(x=> !questionTypesHavingMinimumOneActiveOption.Contains(x.QuestionType) || x.QuestionOptions.Any()).ToListAsync();

                var result = await GetOrCreateDailyTask(userId, new DailyTaskMBModel { DateTime = todayDate, SalesPointId = srv.SalesPointId });
                srv.DailyTaskId = result.Id;
            }

            var dailyTaskIds = surveyModels.Select(x => x.DailyTaskId).ToList();
            var submittedDailySurveyTask = _dailySurveyTask.GetAllActive().Include(inc => inc.DailyTask)
                .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

            var submittedDailyTaskIds = submittedDailySurveyTask.Select(x => x.DailyTaskId).ToList();

            if (submittedDailyTaskIds.Count > 0)
                surveyModels = surveyModels.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

            return surveyModels;
        }

        public async Task<List<SurveyMBModel>> GetSurveysByTask(DailyTask task)
        {
            try
            {
                var user = await _cmUser.FindAsync(x => x.Id == task.CmUserId);
                AssignedUserType userType = (AssignedUserType)user.UserType;
                var surveys = _survey.FindAllInclude(x => x.Status == Status.Active &&
                                                          x.IsConsumerSurvey == false && x.SalesPointId == task.SalesPointId &&
                                                          x.SurveyQuestionSet.Status == Status.Active &&
                                                          x.FromDate <= task.DateTime && task.DateTime <= x.ToDate &&
                                                          (x.UserType == AssignedUserType.BOTH || x.UserType == userType),
                                                          i2 => i2.SurveyQuestionSet).ToList();
                var surveyModels = surveys.MapToMBModel();

                var srvSetIds = surveys.Select(x => x.SurveyQuestionSetId).ToList();
                var surveyQuestionMaps = _surveyQuestionCollection.FindAllInclude(x => x.Status == Status.Active && x.Question.Status == Status.Active && srvSetIds.Contains(x.SurveyId), i1 => i1.Question).ToList().MapToMBModel();
                var qIds = surveyQuestionMaps.Select(q => q.QuestionId).ToList();
                var qOptions = _questionOption.FindAll(x => qIds.Contains(x.QuestionId) && x.Status == Status.Active).ToList();
                var questions = surveyQuestionMaps.Select(x => x.Question).ToList();
                foreach (var q in questions)
                {
                    q.QuestionOptions = await qOptions.FindAll(x => x.Status == Status.Active && x.QuestionId == q.Id).ToMap<QuestionOption, QuestionOptionMBModel>().ToListAsync();
                    if (q.QuestionType == QuestionTypes.Emo.GetDescription())
                    {
                        q.QuestionOptions = q.QuestionOptions.OrderBy(x => x.Sequence).ToList();
                    }
                }
                var dailyTaskIds = surveyModels.Select(x => x.DailyTaskId).ToList();
                var submittedDailySurveyTask = _dailySurveyTask.GetAllActive().Include(inc => inc.DailyTask)
                    .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

                var submittedDailyTaskIds = submittedDailySurveyTask.Select(x => x.DailyTaskId).ToList();

                if (submittedDailyTaskIds.Count > 0)
                    surveyModels = surveyModels.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();

                return surveyModels;
            }
            catch (Exception e)
            {
                return new List<SurveyMBModel>();
            }
        }

        public async Task<List<ConsumerSurveyMBModel>> GetConsumerSurveysOfTodayByUser(int userId)
        {
            var user = _cmUser.Find(x => x.Id == userId);
            var salespoints = _cmsUserSalesPointMapping.GetAll().Where(x => x.CmUserId == userId).ToList();
            var sIds = salespoints.Select(x => x.SalesPointId).ToList();
            var todayDate = DateTime.UtcNow.BangladeshDateInUtc();
            AssignedUserType userType = (AssignedUserType)user.UserType;
            var surveys = _survey.FindAllInclude(x => x.IsConsumerSurvey && x.Status == Status.Active && x.SurveyQuestionSet.Status == Status.Active  && sIds.Contains(x.SalesPointId) && x.FromDate <= todayDate && todayDate <= x.ToDate &&
                (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.SurveyQuestionSet).ToList();
            var surveyModels = surveys.MapToConsumerMBModel();

            var srvSetIds = surveys.Select(x => x.SurveyQuestionSetId).ToList();
            var surveyQuestionMaps = _surveyQuestionCollection.FindAllInclude(x => srvSetIds.Contains(x.SurveyId) && x.Status == Status.Active && x.Question.Status == Status.Active,
                i1 => i1.Question).ToList().MapToMBModel();
            var qIds = surveyQuestionMaps.Select(q => q.QuestionId).ToList();
            var qOptions = _questionOption.FindAll(x => qIds.Contains(x.QuestionId) && x.Status == Status.Active).ToList();
            var questions = surveyQuestionMaps.Select(x => x.Question).ToList();
            foreach (var q in questions)
            {
                q.QuestionOptions = await qOptions.FindAll(x => x.QuestionId == q.Id).ToMap<QuestionOption, QuestionOptionMBModel>().ToListAsync();
                if (q.QuestionType == QuestionTypes.Emo.GetDescription())
                {
                    q.QuestionOptions = q.QuestionOptions.OrderBy(x => x.Sequence).ToList();
                }
            }

            foreach (var srv in surveyModels)
            {
                srv.SurveyQuestionSet.Questions = await surveyQuestionMaps.FindAll(x => x.SurveyId == srv.SurveyQuestionSet.Id)
                    .Select(x => x.Question).Where(x => !questionTypesHavingMinimumOneActiveOption.Contains(x.QuestionType) || x.QuestionOptions.Any()).ToListAsync();
            }

            foreach (var srv in surveyModels)
            {
                srv.SurveyQuestionSet.Questions = await surveyQuestionMaps.FindAll(x => x.SurveyId == srv.SurveyQuestionSet.Id).Select(x => x.Question).ToListAsync();

                var result = await GetOrCreateDailyTask(userId, new DailyTaskMBModel { DateTime = todayDate, SalesPointId = srv.SalesPointId });
                srv.DailyTaskId = result.Id;
            }

            return surveyModels;
        }

        public async Task<List<ConsumerSurveyMBModel>> GetConsumerSurveysByTask(DailyTask task)
        {
            try
            {
                var user = await _cmUser.FindAsync(x => x.Id == task.CmUserId);
                AssignedUserType userType = (AssignedUserType)user.UserType;
                var surveys = _survey.FindAllInclude(x => x.IsConsumerSurvey && x.Status == Status.Active && x.SurveyQuestionSet.Status == Status.Active &&
                                                          x.SalesPointId == task.SalesPointId && x.FromDate <= task.DateTime && task.DateTime <= x.ToDate &&
                    (x.UserType == AssignedUserType.BOTH || x.UserType == userType), i2 => i2.SurveyQuestionSet).ToList();
                var surveyModels = surveys.MapToConsumerMBModel();

                var srvSetIds = surveys.Select(x => x.SurveyQuestionSetId).ToList();
                var surveyQuestionMaps = _surveyQuestionCollection.FindAllInclude(x => srvSetIds.Contains(x.SurveyId) && x.Status == Status.Active && x.Question.Status == Status.Active,
                    i1 => i1.Question).ToList().MapToMBModel();
                var qIds = surveyQuestionMaps.Select(q => q.QuestionId).ToList();
                var qOptions = _questionOption.FindAll(x => qIds.Contains(x.QuestionId) && x.Status == Status.Active).ToList();
                var questions = surveyQuestionMaps.Select(x => x.Question).ToList();
                foreach (var q in questions)
                {
                    q.QuestionOptions = await qOptions.FindAll(x => x.QuestionId == q.Id).ToMap<QuestionOption, QuestionOptionMBModel>().ToListAsync();
                    if (q.QuestionType == QuestionTypes.Emo.GetDescription())
                    {
                        q.QuestionOptions = q.QuestionOptions.OrderBy(x => x.Sequence).ToList();
                    }
                }

                foreach (var srv in surveyModels)
                {
                    srv.SurveyQuestionSet.Questions = await surveyQuestionMaps.FindAll(x => x.SurveyId == srv.SurveyQuestionSet.Id)
                        .Select(x => x.Question).Where(x => !questionTypesHavingMinimumOneActiveOption.Contains(x.QuestionType) || x.QuestionOptions.Any()).ToListAsync();
                }

                foreach (var srv in surveyModels)
                {
                    srv.SurveyQuestionSet.Questions = await surveyQuestionMaps.FindAll(x => x.SurveyId == srv.SurveyQuestionSet.Id).Select(x => x.Question).ToListAsync();

                    var result = await GetOrCreateDailyTask(task.CmUserId, new DailyTaskMBModel { DateTime = task.DateTime, SalesPointId = srv.SalesPointId });
                    srv.DailyTaskId = result.Id;
                }

                return surveyModels;
            }
            catch (Exception e)
            {
                return new List<ConsumerSurveyMBModel>();
            }
        }

        public async Task<SurveyModel> UpdateSurvey(SurveyModel payload)
        {
            Survey survey = await _survey.FindAsync(x => x.Id == payload.Id);
            if (survey is null) throw new AppException("Survey not found");
            var existingSurveys = await this.GetExistingSurveys(new List<SurveyModel>() { payload });
            existingSurveys = existingSurveys.FindAll(x => x.Id != payload.Id);
            var fromDate = payload.FromDate;
            var toDate = payload.ToDate;
            if (existingSurveys.Count > 0)
            {
                var dateRangeViolatedSurveys = existingSurveys.FindAll(x => x.IsConsumerSurvey == payload.IsConsumerSurvey &&
                    x.ToDate > fromDate && fromDate < x.FromDate && x.ToDate >= toDate && toDate >= x.FromDate);

                if (dateRangeViolatedSurveys.Count > 0)
                {
                    var spName = "";
                    if (payload.SalesPoint is object) spName = payload.SalesPoint.Name;
                    else spName = _salesPoint.Find(x => x.SalesPointId == payload.SalesPointId).Name;
                    throw new AppException($"The date range conflicts with existing survey in {spName}");
                }

                var srvIds = existingSurveys.Select(x => x.Id).ToList();
                var serveys = _survey.FindAll(x => srvIds.Contains(x.Id)).ToList();

                foreach (var srv in serveys)
                {
                    srv.ToDate = fromDate.AddSeconds(-1);
                }
                await _survey.UpdateListAsync(serveys);
            }
            if (survey.ToDate != payload.ToDate)
            {
                survey.ToDate = payload.ToDate;
            }
            if (survey.FromDate != payload.FromDate)
            {
                survey.FromDate = payload.FromDate;
            }

            survey.Status = payload.Status;

            if (survey.SurveyQuestionSetId != payload.SurveyQuestionSetId) survey.SurveyQuestionSetId = payload.SurveyQuestionSetId;
            await _survey.UpdateAsync(survey);
            return survey.MapToModel();
        }

        public async Task<DailyTaskMBModel> GetOrCreateDailyTask(int cmUserId, DailyTaskMBModel model)
        {
            var checkExisting = await _dailyTask.FindAsync(x =>
                x.SalesPointId == model.SalesPointId && x.CmUserId == cmUserId &&
                x.DateTime.Date == model.DateTime.Date);

            if (checkExisting != null)
                return checkExisting.ToMap<DailyTask, DailyTaskMBModel>();

            var dailyTaskModel = new DailyTask();
            dailyTaskModel.SalesPointId = model.SalesPointId;
            dailyTaskModel.CmUserId = cmUserId;
            dailyTaskModel.DateTime = model.DateTime;

            await _dailyTask.CreateAsync(dailyTaskModel);
            var dailyTask = dailyTaskModel.ToMap<DailyTask, DailyTaskMBModel>();

            return dailyTask;
        }


    }
}
