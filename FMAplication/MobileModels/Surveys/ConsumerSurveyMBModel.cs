using AutoMapper;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Surveys;
using FMAplication.MobileModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.Surveys
{
    public class ConsumerSurveyMBModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public SurveyQuestionSetMBModel SurveyQuestionSet { get; set; }
        public int SalesPointId { get; set; }
        public string fromDateStr { get; set; }
        public string toDateStr { get; set; }

        public int DailyTaskId { get; set; }
    }

    public static class ConsumerSurveyMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Survey, ConsumerSurveyMBModel>();
            cfg.CreateMap<SurveyQuestionSet, SurveyQuestionSetMBModel>();
        }).CreateMapper();

        public static ConsumerSurveyMBModel MapToConsumerMBModel(this Survey source)
        {
            var result = Mapper.Map<ConsumerSurveyMBModel>(source);
            return result;
        }

        public static List<ConsumerSurveyMBModel> MapToConsumerMBModel(this IEnumerable<Survey> source)
        {
            var result = Mapper.Map<List<ConsumerSurveyMBModel>>(source);
            return result;
        }
    }
}
