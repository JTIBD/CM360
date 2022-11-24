using System.Collections.Generic;
using AutoMapper;
using FMAplication.Core;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Helpers;
using FMAplication.Models.Bases;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;
using Newtonsoft.Json;

namespace FMAplication.Models.DailyTasks
{
    public class DailyInformationTaskModel:DailyBaseTaskModel
    {
        public string InsightImage { get; set; }

        public string InsightDescription { get; set; }
        public string RequestImage { get; set; }
        public string RequestDescription { get; set; }
    }

    public static class DailyInformationTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyInformationTask, DailyInformationTaskModel>();
            cfg.CreateMap<Reason, ReasonModel>();
            cfg.CreateMap<Outlet, OutletModel>();
        }).CreateMapper();

        public static DailyInformationTaskModel MapToModel(this DailyInformationTask source)
        {
            var result = Mapper.Map<DailyInformationTaskModel>(source);
            return result;
        }

        public static List<DailyInformationTaskModel> MapToModel(this IEnumerable<DailyInformationTask> source)
        {
            var result = Mapper.Map<List<DailyInformationTaskModel>>(source);
            return result;
        }

    }
}