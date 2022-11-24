using FMAplication.Domain.Questions;
using FMAplication.Extensions;
using FMAplication.Models.Questions;
using FMAplication.Domain.Examples;
using FMAplication.Repositories;
using FMAplication.Services.QuestionDetails.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using System;
using System.Linq;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Domain.Surveys;
using FMAplication.Exceptions;

namespace FMAplication.Services.QuestionDetails.Implementation
{
    public class QuestionSetService : IQuestionSetService
    {
        private readonly IRepository<SurveyQuestionSet> _surveyQuestionSet;
        private readonly IRepository<Survey> _survey;
        private readonly IRepository<SurveyQuestionCollection> _surveyQuestionCollection;
        private readonly IQuestionService _question;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<DailySurveyTask> _dailySurveyTask;
        private readonly IRepository<DailyConsumerSurveyTask> _dailyConsumerSurveyTask;

        public QuestionSetService(
            IQuestionService question,
            IRepository<SurveyQuestionSet> surveyQuestionSet,
            IRepository<SurveyQuestionCollection> surveyQuestionCollection,
            IRepository<Survey> survey, IRepository<SalesPoint> salesPoint, IRepository<DailySurveyTask> dailySurveyTask, IRepository<DailyConsumerSurveyTask> dailyConsumerSurveyTask)
        {
            _question = question;
            _surveyQuestionSet = surveyQuestionSet;
            _surveyQuestionCollection = surveyQuestionCollection;
            _survey = survey;
            _salesPoint = salesPoint;
            _dailySurveyTask = dailySurveyTask;
            _dailyConsumerSurveyTask = dailyConsumerSurveyTask;
        }
        #region Survey
        public async Task<SurveyQuestionSetModel> CreateSurveyAndQsCollectionAsync(SurveyQuestionSetModel model)
        {
            var result = await CreateSurveyAsync(model);

            var collectionModel = new SurveyQuestionMap
            {
                QuestionsId = model.QuestionsId,
                SurveyId = result.Id
            };
            var collectionRes = await UpdateSQsCollectionAsync(collectionModel);
            result.QuestionsId = collectionRes.Select(cr => cr.QuestionId).ToList();

            return result;
        }
        private async Task<SurveyQuestionSetModel> CreateSurveyAsync(SurveyQuestionSetModel model)
        {
            var survey = model.ToMap<SurveyQuestionSetModel, SurveyQuestionSet>();
            var result = await _surveyQuestionSet.CreateAsync(survey);
            return result.ToMap<SurveyQuestionSet, SurveyQuestionSetModel>();
        }

        public async Task<int> CascadeDeleteSurveyAndQsCollectionAsync(int surveyId)
        {
            //first delete the collection table
            var collectionModels = await GetQsCollectionBySurveyIdAsync(surveyId);
            if (collectionModels.Count() > 0)
            {
                await DeleteSurveyQsCollectionAsync(collectionModels.ToList());
            }

            var surveySetups = _survey.GetAllActive().Where(x => x.SurveyQuestionSetId == surveyId).ToList();
            if (surveySetups.Any())
            {
                var spIds = surveySetups.Select(x => x.SalesPointId).ToList();
                var sps = _salesPoint.GetAllActive().Where(x => spIds.Contains(x.SalesPointId)).Select(x=>x.Name).ToList();
                throw new AppException($"Survey exist for this question set in salespoint {string.Join(",",sps)}");
            }
            //now delete survey
            var result = await DeleteSurveyAsync(surveyId);

            return result;
        }
        private async Task<int> DeleteSurveyAsync(int id)
        {
            var result = await _surveyQuestionSet.DeleteAsync(s => s.Id == id);
            return result;
        }

        public async Task<SurveyQuestionSetModel> GetQuestionSetAsync(int sId)
        {
            var result = await GetSurveyAsync(sId);
            var collectionRes = await GetQsCollectionBySurveyIdAsync(sId);
            result.QuestionsId = collectionRes.Select(cr => cr.QuestionId).ToList();
            await SetEditAndDeletability(new List<SurveyQuestionSetModel>() { result });
            return result;
        }
        private async Task<SurveyQuestionSetModel> GetSurveyAsync(int id)
        {
            var result = await _surveyQuestionSet.FindAsync(s => s.Id == id);
            return result.ToMap<SurveyQuestionSet, SurveyQuestionSetModel>();
        }

        public async Task<IEnumerable<SurveyQuestionSetModel>> GetAllSurveyQuestionSetsWithCorrespondingQsCollectionAsync()
        {
            var results = await GetSurveysAsync();

            var groups = (await GetAllQsCollectionGrpBySurveyIdAsync()).ToList();
            groups.ForEach(group =>
            {
                results.FirstOrDefault(r => r.Id == group.Key).QuestionsId = group.Select(e => e.QuestionId).ToList();
            });

             await SetEditAndDeletability(results.ToList());

            return results;
        }

        private async Task SetEditAndDeletability(List<SurveyQuestionSetModel> qSets)
        {
            var setIds = qSets.Select(x => x.Id).ToList();

            var surveys = (await _survey.FindAllAsync(x => setIds.Contains(x.SurveyQuestionSetId))).ToList();

            foreach (var r in qSets)
            {
                r.IsDeletable = surveys.All(s => s.SurveyQuestionSetId != r.Id);
            }

            foreach (var r in qSets)
            {
                r.IsEditable = !surveys.Any(s => s.SurveyQuestionSetId == r.Id && s.FromDate < DateTime.UtcNow && DateTime.UtcNow < s.ToDate);
            }

            qSets = qSets.Where(x => x.IsEditable).ToList();
            setIds = qSets.Select(x => x.Id).ToList();
            if(!qSets.Any()) return;
            var customerSurveyTasks =
                _dailySurveyTask.GetAll().Where(x => setIds.Contains(x.SurveyQuestionSetId)).ToList();

            var consumerSurveyTasks =
                _dailyConsumerSurveyTask.GetAll().Where(x => setIds.Contains(x.SurveyQuestionSetId)).ToList();

            foreach (var qSet in qSets)
            {
                qSet.IsEditable = customerSurveyTasks.All(c => c.SurveyQuestionSetId != qSet.Id) &&
                                  consumerSurveyTasks.All(c => c.SurveyQuestionSetId != qSet.Id);
            }

        }
        private async Task<IEnumerable<SurveyQuestionSetModel>> GetSurveysAsync()
        {
            var result = (await _surveyQuestionSet.GetAllAsync()).OrderByDescending(s => s.CreatedTime);
            return result.ToMap<SurveyQuestionSet, SurveyQuestionSetModel>();
        }

        public async Task<SurveyQuestionSetModel> UpdateSurveyWithQsCollectionAsync(SurveyQuestionSetModel model)
        {
            var result = await UpdateSurveyAsync(model);
            var collectionModel = new SurveyQuestionMap
            {
                QuestionsId = model.QuestionsId,
                SurveyId = result.Id
            };
            var collectionRes = await UpdateSQsCollectionAsync(collectionModel);
            result.QuestionsId = collectionRes.Select(cr => cr.QuestionId).ToList();

            return result;
        }
        private async Task<SurveyQuestionSetModel> UpdateSurveyAsync(SurveyQuestionSetModel model)
        {
            var question = model.ToMap<SurveyQuestionSetModel, SurveyQuestionSet>();
            question.Status = model.Status;
            if (model.Status ==  Status.InActive)
            {
                var surveySetups = _survey.GetAllActive().Where(x => x.SurveyQuestionSetId == question.Id).ToList();
                if (surveySetups.Any())
                {
                    var spIds = surveySetups.Select(x => x.SalesPointId).ToList();
                    var sps = _salesPoint.GetAllActive().Where(x => spIds.Contains(x.SalesPointId)).Select(x => x.Name).ToList();
                    throw new AppException($"Survey exist for this question set in salespoint {string.Join(",", sps)}");
                }
            }
            var result = await _surveyQuestionSet.UpdateAsync(question);
            var mappedResult = result.ToMap<SurveyQuestionSet, SurveyQuestionSetModel>();
            return mappedResult;
        }

        public async Task<bool> IsQuestionSetNameExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _surveyQuestionSet.IsExistAsync(s => s.Name == name)
                : await _surveyQuestionSet.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }

        public async Task<IPagedList<SurveyQuestionSetModel>> GetPagedSurveysAsync(int pageNumber, int pageSize)
        {
            var result = await _surveyQuestionSet.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<SurveyQuestionSet, SurveyQuestionSetModel>();
        }

        #endregion

        #region SurveyQuestionCollection
        public async Task<List<SurveyQuestionCollectionModel>> UpdateSQsCollectionAsync(SurveyQuestionMap model)
        {
            //delete from existing collection
            var existingModels = (await GetQsCollectionBySurveyIdAsync(model.SurveyId)).ToList();
            if (existingModels.Count > 0)
            {
                var modelsForDelete = existingModels.FindAll(e => !model.QuestionsId.Contains(e.QuestionId));
                var deleted = (modelsForDelete.Count) > 0 ? await DeleteSurveyQsCollectionAsync(modelsForDelete) : 0;
            }

            //add new 
            var qsIdsToAdd = model.QuestionsId.FindAll(quid => !existingModels.Contains(existingModels.FirstOrDefault(e => e.QuestionId == quid)));
            var collectionModels = new List<SurveyQuestionCollectionModel>();
            var results = new List<SurveyQuestionCollectionModel>();
            if (qsIdsToAdd.Count > 0)
            {
                qsIdsToAdd.ForEach(qid =>
                {
                    collectionModels.Add(new SurveyQuestionCollectionModel
                    {
                        QuestionId = qid,
                        SurveyId = model.SurveyId
                    });
                });

                //2 things before create => check is exist and check if the questions id exist on question table
                results = await CreateSurveyQsCollectionAsync(collectionModels);
            }

            var finalCollections = (await GetQsCollectionBySurveyIdAsync(model.SurveyId)).ToList();
            return finalCollections;
        }

        public async Task<IEnumerable<SurveyQuestionCollectionModel>> GetQsCollectionBySurveyIdAsync(int surveyId)
        {
            var result = await _surveyQuestionCollection.FindAllAsync(s => s.SurveyId == surveyId);
            return result.ToMap<SurveyQuestionCollection, SurveyQuestionCollectionModel>();
        }

        public async Task<int> DeleteSurveyQsCollectionAsync(List<SurveyQuestionCollectionModel> models)
        {
            var entities = models.ToMap<SurveyQuestionCollectionModel, SurveyQuestionCollection>();
            return await _surveyQuestionCollection.DeleteListAsync(entities);
        }

        public async Task<IEnumerable<IGrouping<int, SurveyQuestionCollectionModel>>> GetAllQsCollectionGrpBySurveyIdAsync()
        {
            var entities = await _surveyQuestionCollection.GetAllAsync();
            var result = entities.ToMap<SurveyQuestionCollection, SurveyQuestionCollectionModel>();
            var groups = result.GroupBy(r => r.SurveyId);

            return groups;
        }

        private async Task<List<SurveyQuestionCollectionModel>> CreateSurveyQsCollectionAsync(List<SurveyQuestionCollectionModel> collectionModels)
        {
            var isCollectionValid = await IsSyrveyQsCollectionValid(collectionModels);
            if (isCollectionValid)
            {
                var entities = collectionModels.ToMap<SurveyQuestionCollectionModel, SurveyQuestionCollection>();
                var results = await _surveyQuestionCollection.CreateListAsync(entities);
                return results.ToMap<SurveyQuestionCollection, SurveyQuestionCollectionModel>();
            }
            else
            {
                var message = "Invalid Survey-Question Collection. No new question added to this survey";
                throw new Exception(message);
            }
        }

        private async Task<bool> IsSyrveyQsCollectionValid(List<SurveyQuestionCollectionModel> collectionModels)
        {
            //check if survey is valid
            var isSurveyValid = await _surveyQuestionSet.IsExistAsync(s => s.Id == collectionModels[0].SurveyId);
            //check if all the questions are valid and active
            var activeQsIdList = (await _question.GetActiveQuestionsAsync()).Select(q => q.Id).ToList();
            var modelQsIdList = collectionModels.Select(cm => cm.QuestionId).ToList();
            var isQsValid = !modelQsIdList.Except(activeQsIdList).Any();

            return isSurveyValid && isQsValid;
        }

        public async Task<List<SurveyQuestionCollectionModel>> GetQuestionsBySurveyIdsAsync(List<int> ids)
        {
            var predicate = PredicateBuilder.False<SurveyQuestionCollection>();

            foreach (var id in ids)
            {
                predicate = predicate.Or(q => q.SurveyId == id);
            }
            // var includeProperties = "Question";
            var result = _surveyQuestionCollection.GetAllIncludeStrFormat(predicate, includeProperties: "");

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SurveyQuestionCollection, SurveyQuestionCollectionModel>();
                // cfg.CreateMap<Question, QuestionModel>();
            }).CreateMapper();

            return mapper.Map<List<SurveyQuestionCollectionModel>>(result);
        }
        #endregion

    }
}
