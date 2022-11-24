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
    public class WareHouseReceivedTransferItemModel
    {
        public int TransferId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQuantity { get; set; }
    }

    public static class WareHouseReceivedTransferItemModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WareHouseReceivedTransferItem, WareHouseReceivedTransferItemModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static WareHouseReceivedTransferItemModel MapToModel(this WareHouseReceivedTransferItem source)
        {
            var result = Mapper.Map<WareHouseReceivedTransferItemModel>(source);
            return result;
        }

        public static List<WareHouseReceivedTransferItemModel> MapToModel(this IEnumerable<WareHouseReceivedTransferItem> source)
        {
            var result = Mapper.Map<List<WareHouseReceivedTransferItemModel>>(source);
            return result;
        }

    }
}
