using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;

namespace FMAplication.Models.DailyTasks
{
    public class DailyPosmAuditTaskModel
    {
        public int Id { get; set; }
        public POSMProductModel PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public ActionType ActionType { get; set; }
        public int Result { get; set; }
        public int DailyAuditTaskId { get; set; }
    }

    public static class DailyPosmAuditTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyPosmAuditTask, DailyPosmAuditTaskModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static DailyPosmAuditTaskModel MapToModel(this DailyPosmAuditTask source)
        {
            var result = Mapper.Map<DailyPosmAuditTaskModel>(source);
            return result;
        }

        public static List<DailyPosmAuditTaskModel> MapToModel(this IEnumerable<DailyPosmAuditTask> source)
        {
            var result = Mapper.Map<List<DailyPosmAuditTaskModel>>(source);
            return result;
        }

    }
}