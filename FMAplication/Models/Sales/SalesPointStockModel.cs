using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Models.Products;
using System.Collections.Generic;
using FMAplication.Models.Bases;

namespace FMAplication.Models.Sales
{
    public class SalesPointStockModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public static class SalesPointStockExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointStock, SalesPointStockModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static SalesPointStockModel MapToModel(this SalesPointStock source)
        {
            var result = Mapper.Map<SalesPointStockModel>(source);
            return result;
        }

        public static List<SalesPointStockModel> MapToModel(this IEnumerable<SalesPointStock> source)
        {
            var result = Mapper.Map<List<SalesPointStockModel>>(source);
            return result;
        }

    }
}
