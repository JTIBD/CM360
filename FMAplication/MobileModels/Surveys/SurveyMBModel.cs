using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Surveys;
using FMAplication.MobileModels.Questions;
using FMAplication.MobileModels.Sales;

namespace FMAplication.MobileModels.Surveys
{
    public class SurveyMBModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public SurveyQuestionSetMBModel SurveyQuestionSet { get; set; }
        public int SalesPointId { get; set; }

        public string FromDateStr { get; set; }
        public string ToDateStr { get; set; }


        public int DailyTaskId { get; set; }
    }

    public static class SurveyMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Survey, SurveyMBModel>();
            cfg.CreateMap<SalesPoint, SalesPointMBModel>();
            cfg.CreateMap<SurveyQuestionSet, SurveyQuestionSetMBModel>();
        }).CreateMapper();

        public static SurveyMBModel MapToMBModel(this Survey source)
        {
            var result = Mapper.Map<SurveyMBModel>(source);
            return result;
        }

        public static List<SurveyMBModel> MapToMBModel(this IEnumerable<Survey> source)
        {
            var result = Mapper.Map<List<SurveyMBModel>>(source);
            return result;
        }
    }
}