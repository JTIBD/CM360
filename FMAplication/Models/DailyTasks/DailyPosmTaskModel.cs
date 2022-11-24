using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Helpers;
using FMAplication.Models.Bases;
using FMAplication.Models.Products;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;
using Newtonsoft.Json;

namespace FMAplication.Models.DailyTasks
{
    public class DailyPosmTaskModel:DailyBaseTaskModel
    {
        public string ExistingImage { get; set; }
        public string NewImage { get; set; }
        public List<DailyPosmTaskItemsModel> DailyPosmTaskItems { get; set; }
    }

    public static class DailyPosmTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyPosmTask, DailyPosmTaskModel>().ForMember(m => m.DailyPosmTaskItems,
                c => c.MapFrom(s => s.DailyPosmTaskItems.MapToModel()));
            cfg.CreateMap<Outlet, OutletModel>();
            cfg.CreateMap<Reason, ReasonModel>();
        }).CreateMapper();

        public static DailyPosmTaskModel MapToModel(this DailyPosmTask source)
        {
            var result = Mapper.Map<DailyPosmTaskModel>(source);
            return result;
        }

        public static List<DailyPosmTaskModel> MapToModel(this IEnumerable<DailyPosmTask> source)
        {
            var result = Mapper.Map<List<DailyPosmTaskModel>>(source);
            return result;
        }

    }
}