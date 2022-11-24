using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.WareHouse;
using FMAplication.Models.Products;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.Sales
{
    public class SalesPointTransferItemModel
    {
        public int Id { get; set; }
        public int TransferId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
    }

    public static class SalesPointTransferItemModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointTransferItem, SalesPointTransferItemModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static SalesPointTransferItemModel MapToModel(this SalesPointTransferItem source)
        {
            var result = Mapper.Map<SalesPointTransferItemModel>(source);
            return result;
        }

        public static List<SalesPointTransferItemModel> MapToModel(this IEnumerable<SalesPointTransferItem> source)
        {
            var result = Mapper.Map<List<SalesPointTransferItemModel>>(source);
            return result;
        }

    }
}
