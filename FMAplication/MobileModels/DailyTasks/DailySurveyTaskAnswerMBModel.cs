using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Questions;
using FMAplication.MobileModels.AvCommunications;

namespace FMAplication.MobileModels.DailyTasks
{
    public class DailySurveyTaskAnswerMBModel
    {
        public int Id { get; set; }
        public DailySurveyTaskMBModel DailySurveyTask { get; set; }
        public int DailySurveyTaskId { get; set; }

        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }

    public static class DailySurveyTaskAnswerMBModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailySurveyTaskAnswer, DailySurveyTaskAnswerMBModel>();
            cfg.CreateMap<DailySurveyTask, DailySurveyTaskMBModel>();
        }).CreateMapper();

        public static DailySurveyTaskAnswerMBModel MapToMBModel(this DailySurveyTaskAnswer source)
        {
            var result = Mapper.Map<DailySurveyTaskAnswerMBModel>(source);
            return result;
        }

        public static List<DailySurveyTaskAnswerMBModel> MapToMBModel(this IEnumerable<DailySurveyTaskAnswer> source)
        {
            var result = Mapper.Map<List<DailySurveyTaskAnswerMBModel>>(source);
            return result;
        }

    }
}
