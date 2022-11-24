using AutoMapper;
using FMAplication.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.Questions
{
    public class SurveyQuestionCollectionMBModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public QuestionMBModel Question { get; set; }
    }

    public static class SurveyQuestionCollectionMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SurveyQuestionCollection, SurveyQuestionCollectionMBModel>();
            cfg.CreateMap<Question, QuestionMBModel>();
        }).CreateMapper();

        public static SurveyQuestionCollectionMBModel MapToMBModel(this SurveyQuestionCollection source)
        {
            var result = Mapper.Map<SurveyQuestionCollectionMBModel>(source);
            return result;
        }

        public static List<SurveyQuestionCollectionMBModel> MapToMBModel(this IEnumerable<SurveyQuestionCollection> source)
        {
            var result = Mapper.Map<List<SurveyQuestionCollectionMBModel>>(source);
            return result;
        }

    }
}
