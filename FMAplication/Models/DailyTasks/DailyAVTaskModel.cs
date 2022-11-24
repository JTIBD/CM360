using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Bases;
using FMAplication.Models.Reasons;

namespace FMAplication.Models.DailyTasks
{
    public class DailyAVTaskModel:DailyBaseTaskModel
    {
        public int AvSetupId { get; set; }
        public AvSetupModel AvSetup { get; set; }
    }

    public static class DailyAVTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyAVTask, DailyAVTaskModel>();
            cfg.CreateMap<Reason, ReasonModel>();
        }).CreateMapper();

        public static DailyAVTaskModel MapToModel(this DailyAVTask source)
        {
            var result = Mapper.Map<DailyAVTaskModel>(source);
            return result;
        }

        public static List<DailyAVTaskModel> MapToModel(this IEnumerable<DailyAVTask> source)
        {
            var result = Mapper.Map<List<DailyAVTaskModel>>(source);
            return result;
        }

    }
}