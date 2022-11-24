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
using FMAplication.Enumerations;
using FMAplication.Domain.Surveys;
using FMAplication.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services.QuestionDetails.Implementation
{
    public class QuestionService : IQuestionService

    {
        private readonly IRepository<Question> _question;
        private readonly IRepository<SurveyQuestionCollection> _surveyQuestionCollection;
        private readonly IRepository<Survey> _survey;

        private readonly IRepository<QuestionOption> _questionOption;
        private readonly IRepository<DailyConsumerSurveyTaskAnswer> _dailyConsumerSurveyTaskAnswer;
        private readonly IRepository<DailyConsumerSurveyTaskAnswer> _dailySurveyTaskAnser;
        private readonly IRepository<SurveyQuestionSet> _questionSet;

        public QuestionService(
            IRepository<Question> question,
            IRepository<QuestionOption> questionOption,
            IRepository<SurveyQuestionCollection> surveyQuestionCollection,
            IRepository<Survey> survey, IRepository<DailyConsumerSurveyTaskAnswer> dailyConsumerSurveyTaskAnswer, IRepository<DailyConsumerSurveyTaskAnswer> dailySurveyTaskAnser, IRepository<SurveyQuestionSet> questionSet)
        {
            _question = question;
            _questionOption = questionOption;
            _surveyQuestionCollection = surveyQuestionCollection;
            _survey = survey;
            _dailyConsumerSurveyTaskAnswer = dailyConsumerSurveyTaskAnswer;
            _dailySurveyTaskAnser = dailySurveyTaskAnser;
            _questionSet = questionSet;
        }

        #region Question
        public async Task<QuestionModel> CreateAsync(QuestionModel model)
        {
            if (model.QuestionOptions?.Any() == true)
            {
                if (model.QuestionOptions.All(o => o.Status == (int)Status.InActive))
                    throw new AppException("All options cannot be inactive");
            }

            //var question = model.ToMap<QuestionModel, Question>();
            var mapper = new MapperConfiguration(cfg =>
                    cfg.CreateMap<QuestionModel, Question>()
                    .ForMember(x => x.QuestionOptions, opt => opt.Ignore())
                ).CreateMapper();

            var question = mapper.Map<Question>(model);

            var result = await _question.CreateAsync(question);

            question.QuestionOptions = model.QuestionOptions.Count != 0 ? new List<QuestionOption>() : null;
            foreach (var optionModel in model.QuestionOptions)
            {
                var questionOption = new QuestionOption();
                questionOption.QuestionId = result.Id;
                questionOption.OptionTitle = optionModel.OptionTitle;
                questionOption.Sequence = optionModel.Sequence;
                questionOption.Status = optionModel.Status == 1 ? Status.Active : Status.InActive;
                await _questionOption.CreateAsync(questionOption);
            }

            //return result.ToMap<Question, QuestionModel>();

            var mapperToModel = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Question, QuestionModel>()
                    .ForMember(x => x.QuestionOptions, opt => opt.Ignore())
                ).CreateMapper();

            return mapperToModel.Map<QuestionModel>(result);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await CheckReferences(id);
            var result = await _question.DeleteAsync(s => s.Id == id);
            return result;

        }

        private async Task CheckReferences(int questionId)
        {
            var questionSets = await _surveyQuestionCollection.GetAll().Where(x => x.QuestionId == questionId).ToListAsync();
            if (questionSets.Any())
            {
                var qSetIds = questionSets.Select(x => x.SurveyId).ToList();
                var qSets = await _questionSet.GetAll().Where(x => qSetIds.Contains(x.Id)).ToListAsync();
                var setNames = qSets.Select(x => x.Name).ToList();
                throw new AppException($"The question has been used in question set {string.Join(", ", setNames)}");
            }
            
        }

        public async Task<bool> IsQuestionExistAsync(string questionTitle, int id)
        {
            var result = id <= 0
                ? await _question.IsExistAsync(s => s.QuestionTitle == questionTitle)
                : await _question.IsExistAsync(s => s.QuestionTitle == questionTitle && s.Id != id);

            return result;
        }
        public async Task<QuestionModel> GetQuestionAsync(int id)
        {
            // var result = await _question.FindAsync(s => s.Id == id);
            var result = await _question.FindIncludeAsync(q => q.Id == id, q => q.QuestionOptions);

            result.QuestionOptions = result.QuestionOptions.OrderBy(o => o.Sequence).ToList();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionModel>();
                cfg.CreateMap<QuestionOption, QuestionOptionModel>();
            }).CreateMapper();

            return mapper.Map<QuestionModel>(result);
            // return result.ToMap<Question, QuestionModel>();
        }

        public async Task<IEnumerable<QuestionModel>> GetQuestionsAsync()
        {
            try
            {
                var result = await _question.GetAllAsync();
                result = result.OrderByDescending(q => q.CreatedTime);                

                var qModels = result.ToMap<Question, QuestionModel>();
                await SetEditablity(qModels);
                return qModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SetEditablity(List<QuestionModel> questions)
        {
            var qIds = questions.Select(x => x.Id).ToList();
            var questionSets = (await _surveyQuestionCollection.FindAllAsync(x => qIds.Contains(x.QuestionId))).ToList();
            var setIds = questionSets.Select(x => x.Id).ToList();
            var surveys = _survey.FindAll(x => setIds.Contains(x.SurveyQuestionSetId)).ToList();
            questions.ForEach(q =>
            {
                var sets = questionSets.FindAll(x => x.QuestionId == q.Id).ToList();
                q.IsEditable = !surveys.Any(x => sets.Any(st => st.SurveyId == x.SurveyQuestionSetId) && x.FromDate > DateTime.UtcNow);
            });
        }

        public async Task<IEnumerable<QuestionModel>> GetActiveQuestionsAsync()
        {
            try
            {
                var result = (await _question.FindAllAsync(q => q.Status == Status.Active)).OrderByDescending(q => q.Id);
                return result.ToMap<Question, QuestionModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IPagedList<QuestionModel>> GetPagedQuestionsAsync(int pageNumber, int pageSize)
        {
            var result = await _question.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<Question, QuestionModel>();

        }

        public async Task<IEnumerable<QuestionModel>> GetQueryQuestionsAsync()
        {
            var result = await _question.ExecuteQueryAsyc<QuestionModel>("SELECT * FROM Question");
            return result;
        }

        public async Task<QuestionModel> SaveAsync(QuestionModel model)
        {
            var question = model.ToMap<QuestionModel, Question>();
            var result = await _question.CreateOrUpdateAsync(question);
            return result.ToMap<Question, QuestionModel>();
        }

        public async Task<QuestionModel> UpdateAsync(QuestionModel model, QuestionModel existingQuestionModel)
        {

            if (model.QuestionOptions?.Any() == true)
            {
                if (model.QuestionOptions.All(o => o.Status == (int) Status.InActive))
                    throw new AppException("All options cannot be inactive");
            }
            //var question = model.ToMap<QuestionModel, Question>();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<QuestionModel, Question>();
                cfg.CreateMap<QuestionOptionModel, QuestionOption>();
            }).CreateMapper();

            var question = mapper.Map<Question>(model);

            // Delete children
            foreach (var optionModel in existingQuestionModel.QuestionOptions)
            {
                if (!model.QuestionOptions.Any(o => o.Id == optionModel.Id))
                    await _questionOption.DeleteAsync(o => o.Id == optionModel.Id);
            }

            foreach (var option in question.QuestionOptions)
            {
                if (option.Id != 0)
                {
                    await _questionOption.UpdateAsync(option);
                }
                else
                {
                    option.QuestionId = question.Id;
                    await _questionOption.CreateAsync(option);
                }
            }

            question.QuestionOptions = null;//new List<QuestionOption>();

            var result = await _question.UpdateAsync(question);

            var mapperToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionModel>();
                cfg.CreateMap<QuestionOption, QuestionOptionModel>();
            }).CreateMapper();

            return mapperToModel.Map<QuestionModel>(result);

            //return result.ToMap<Question, QuestionModel>();
        }
        #endregion

        #region QuestionOption
        public async Task<QuestionOptionModel> CreateQuestionOptionAsync(QuestionOptionModel model)
        {
            var question = model.ToMap<QuestionOptionModel, QuestionOption>();
            var result = await _questionOption.CreateAsync(question);
            return result.ToMap<QuestionOption, QuestionOptionModel>();
        }

        public async Task<IEnumerable<QuestionOptionModel>> GetQuestionOptionsAsync(int questionId)
        {
            var result = await _questionOption.FindAllAsync(qo => qo.QuestionId == questionId);
            return result.ToMap<QuestionOption, QuestionOptionModel>();
        }

        public async Task<int> DeleteQuestionOptionAsync(int id)
        {
            var result = await _questionOption.DeleteAsync(s => s.Id == id);
            return result;
        }

        #endregion

    }
}
