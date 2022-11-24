using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Models.Products;

namespace FMAplication.Models.Sales
{
    public class SalesPointReceivedTransferItemModel
    {
        public int Id { get; set; }
        public int TransferId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQuantity { get; set; }
    }

    public static class SalesPointReceivedTransferItemExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointReceivedTransferItem, SalesPointReceivedTransferItemModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static SalesPointReceivedTransferItemModel MapToModel(this SalesPointReceivedTransferItem source)
        {
            var result = Mapper.Map<SalesPointReceivedTransferItemModel>(source);
            return result;
        }

        public static List<SalesPointReceivedTransferItemModel> MapToModel(this IEnumerable<SalesPointReceivedTransferItem> source)
        {
            var result = Mapper.Map<List<SalesPointReceivedTransferItemModel>>(source);
            return result;
        }

    }
}
