
using AutoMapper;
using FMAplication.Enumerations;
using FMAplication.Models.Questions;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Bases;

namespace FMAplication.Models.Survey
{
    public class SurveyModel:BaseSetupModel
    {
        public string Code { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public SurveyQuestionSetModel SurveyQuestionSet { get; set; }
        public bool IsConsumerSurvey { get; set; }
    }

    public static class SurveyExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Surveys.Survey, SurveyModel>();
            cfg.CreateMap<Domain.Sales.SalesPoint, SalesPointModel>();
            cfg.CreateMap<Domain.Questions.SurveyQuestionSet, SurveyQuestionSetModel>();
        }).CreateMapper();

        public static SurveyModel MapToModel(this Domain.Surveys.Survey source)
        {
            var result = Mapper.Map<SurveyModel>(source);
            return result;
        }

        public static List<SurveyModel> MapToModel(this IEnumerable<Domain.Surveys.Survey> source)
        {
            var result = Mapper.Map<List<SurveyModel>>(source);
            return result;
        }

    }
}
