using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.WareHouse;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.wareHouse
{
    public class WareHouseStockModel
    {
        public int Id { get; set; }
        public int WareHouseId { get; set; }
        public WareHouseModel WareHouse { get; set; }
        public int PosmProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public static class WareHouseStockExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WareHouseStock, WareHouseStockModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
            cfg.CreateMap<WareHouse, WareHouseModel>();
        }).CreateMapper();

        public static WareHouseStockModel MapToModel(this WareHouseStock source)
        {
            var result = Mapper.Map<WareHouseStockModel>(source);
            return result;
        }

        public static List<WareHouseStockModel> MapToModel(this IEnumerable<WareHouseStock> source)
        {
            var result = Mapper.Map<List<WareHouseStockModel>>(source);
            return result;
        }

    }
}
