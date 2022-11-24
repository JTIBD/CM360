using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Helpers;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;
using Newtonsoft.Json;

namespace FMAplication.Models.DailyTasks
{
    public class DailyPosmTaskItemsModel
    {
        public int Id { get; set; }
        public int DailyPosmTaskId { get; set; }
        public POSMProductModel PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public int Quantity { get; set; }
        public PosmWorkType ExecutionType { get; set; }
        public string Image { get; set; }
    }

    public static class DailyPosmTaskItemsModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyPosmTaskItems, DailyPosmTaskItemsModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static DailyPosmTaskItemsModel MapToModel(this DailyPosmTaskItems source)
        {
            var result = Mapper.Map<DailyPosmTaskItemsModel>(source);
            return result;
        }

        public static List<DailyPosmTaskItemsModel> MapToModel(this IEnumerable<DailyPosmTaskItems> source)
        {
            var result = Mapper.Map<List<DailyPosmTaskItemsModel>>(source);
            return result;
        }

    }
}