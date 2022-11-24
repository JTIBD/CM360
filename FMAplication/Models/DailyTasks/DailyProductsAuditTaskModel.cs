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
    public class DailyProductsAuditTaskModel
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int ProductId { get; set; }
        public ActionType ActionType { get; set; }
        public double Result { get; set; }
        public int DailyAuditTaskId { get; set; }
    }

    public static class DailyProductsAuditTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyProductsAuditTask, DailyProductsAuditTaskModel>();
            cfg.CreateMap<Product, ProductModel>();
        }).CreateMapper();

        public static DailyProductsAuditTaskModel MapToModel(this DailyProductsAuditTask source)
        {
            var result = Mapper.Map<DailyProductsAuditTaskModel>(source);
            return result;
        }

        public static List<DailyProductsAuditTaskModel> MapToModel(this IEnumerable<DailyProductsAuditTask> source)
        {
            var result = Mapper.Map<List<DailyProductsAuditTaskModel>>(source);
            return result;
        }

    }
}