using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Sales;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Models.Bases;
using FMAplication.Models.Questions;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;

namespace FMAplication.Models.DailyTasks
{
    public class DailySurveyTaskModel:DailyBaseTaskModel
    {

        public SurveyQuestionSetModel SurveyQuestionSet { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public List<DailySurveyTaskAnswerModel> DailySurveyTaskAnswers { get; set; }
    }

    public static class DailySurveyTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailySurveyTask, DailySurveyTaskModel>()
                .ForMember(m => m.DailySurveyTaskAnswers, c => c.MapFrom(s => s.DailySurveyTaskAnswers.MapToModel()));
            cfg.CreateMap<Reason, ReasonModel>();
            cfg.CreateMap<SurveyQuestionSet, SurveyQuestionSetModel>();
            cfg.CreateMap<Outlet, OutletModel>();
        }).CreateMapper();

        public static DailySurveyTaskModel MapToModel(this DailySurveyTask source)
        {
            var result = Mapper.Map<DailySurveyTaskModel>(source);
            return result;
        }

        public static List<DailySurveyTaskModel> MapToModel(this IEnumerable<DailySurveyTask> source)
        {
            var result = Mapper.Map<List<DailySurveyTaskModel>>(source);
            return result;
        }

    }
}