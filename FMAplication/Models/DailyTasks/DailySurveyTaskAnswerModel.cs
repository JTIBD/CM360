using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Questions;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Models.Questions;

namespace FMAplication.Models.DailyTasks
{
    public class DailySurveyTaskAnswerModel
    {
        public int Id { get; set; }
        public int DailySurveyTaskId { get; set; }
        public QuestionModel Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }

    public static class DailySurveyTaskAnswerModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailySurveyTaskAnswer, DailySurveyTaskAnswerModel>();
            cfg.CreateMap<Question, QuestionModel>();
        }).CreateMapper();

        public static DailySurveyTaskAnswerModel MapToModel(this DailySurveyTaskAnswer source)
        {
            var result = Mapper.Map<DailySurveyTaskAnswerModel>(source);
            return result;
        }

        public static List<DailySurveyTaskAnswerModel> MapToModel(this IEnumerable<DailySurveyTaskAnswer> source)
        {
            var result = Mapper.Map<List<DailySurveyTaskAnswerModel>>(source);
            return result;
        }

    }
}