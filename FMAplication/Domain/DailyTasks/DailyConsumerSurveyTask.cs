using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Core;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.DailyTasks;
using FMAplication.Models.Questions;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyConsumerSurveyTask : DailyBaseTask
    {
        public SurveyQuestionSet SurveyQuestionSet { get; set; }
        public int SurveyQuestionSetId { get; set; }
        public List<DailyConsumerSurveyTaskAnswer> DailyConsumerSurveyTaskAnswers { get; set; }
    }

    public static class DailyConsumerSurveyTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyConsumerSurveyTask, DailyConsumerSurveyTaskModel>()
                .ForMember(m => m.DailyConsumerSurveyTaskAnswers,
                    c => c.MapFrom(s => s.DailyConsumerSurveyTaskAnswers.MapToModel()));
            cfg.CreateMap<Reason, ReasonModel>();
            cfg.CreateMap<SurveyQuestionSet, SurveyQuestionSetModel>();
        }).CreateMapper();

        public static DailyConsumerSurveyTaskModel MapToModel(this DailyConsumerSurveyTask source)
        {
            var result = Mapper.Map<DailyConsumerSurveyTaskModel>(source);
            return result;
        }

        public static List<DailyConsumerSurveyTaskModel> MapToModel(this IEnumerable<DailyConsumerSurveyTask> source)
        {
            var result = Mapper.Map<List<DailyConsumerSurveyTaskModel>>(source);
            return result;
        }

    }
}
