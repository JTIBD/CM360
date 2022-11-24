using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Models.Bases;
using FMAplication.Models.Reasons;
using FMAplication.Models.Sales;

namespace FMAplication.Models.DailyTasks
{
    public class DailyAuditTaskModel:DailyBaseTaskModel
    {
        public int AuditSetupId { get; set; }
        public ICollection<DailyProductsAuditTaskModel> DailyProductsAuditTask { get; set; }

        public DailyAuditTaskModel()
        {
            DailyProductsAuditTask = new List<DailyProductsAuditTaskModel>();
        }
    }

    public static class DailyAuditTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyAuditTask, DailyAuditTaskModel>().ForMember(m => m.DailyProductsAuditTask,
                c => c.MapFrom(s => s.DailyProductsAuditTask.MapToModel()));
            cfg.CreateMap<Outlet, OutletModel>();
            cfg.CreateMap<Reason, ReasonModel>();
        }).CreateMapper();

        public static DailyAuditTaskModel MapToModel(this DailyAuditTask source)
        {
            var result = Mapper.Map<DailyAuditTaskModel>(source);
            return result;
        }

        public static List<DailyAuditTaskModel> MapToModel(this IEnumerable<DailyAuditTask> source)
        {
            var result = Mapper.Map<List<DailyAuditTaskModel>>(source);
            return result;
        }

    }
}