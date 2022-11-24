using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Questions;
using FMAplication.Models.Questions;

namespace FMAplication.Models.DailyTasks
{
    public class DailyConsumerSurveyTaskAnswerModel
    {
        public int Id { get; set; }
        public int DailyConsumerSurveyTaskId { get; set; }
        public QuestionModel Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }

    public static class DailyConsumerSurveyTaskAnswerModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyConsumerSurveyTaskAnswer, DailyConsumerSurveyTaskAnswerModel>();
            cfg.CreateMap<Question, QuestionModel>();
        }).CreateMapper();

        public static DailyConsumerSurveyTaskAnswerModel MapToModel(this DailyConsumerSurveyTaskAnswer source)
        {
            var result = Mapper.Map<DailyConsumerSurveyTaskAnswerModel>(source);
            return result;
        }

        public static List<DailyConsumerSurveyTaskAnswerModel> MapToModel(this IEnumerable<DailyConsumerSurveyTaskAnswer> source)
        {
            var result = Mapper.Map<List<DailyConsumerSurveyTaskAnswerModel>>(source);
            return result;
        }

    }
}