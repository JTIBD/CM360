using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Bases;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;

namespace FMAplication.Models.DailyTasks
{
    public class DailyCommunicationTaskModel:DailyBaseTaskModel
    {
        public int CommunicationSetupId { get; set; }
        public CommunicationSetupModel CommunicationSetup { get; set; }
    }

    public static class DailyCommunicationTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyCommunicationTask, DailyCommunicationTaskModel>();
            cfg.CreateMap<Reason, ReasonModel>();
            cfg.CreateMap<Outlet, OutletModel>();
        }).CreateMapper();

        public static DailyCommunicationTaskModel MapToModel(this DailyCommunicationTask source)
        {
            var result = Mapper.Map<DailyCommunicationTaskModel>(source);
            return result;
        }

        public static List<DailyCommunicationTaskModel> MapToModel(this IEnumerable<DailyCommunicationTask> source)
        {
            var result = Mapper.Map<List<DailyCommunicationTaskModel>>(source);
            return result;
        }

    }
}