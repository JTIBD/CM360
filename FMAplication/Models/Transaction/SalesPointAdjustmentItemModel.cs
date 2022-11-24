using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.Transaction;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class SalesPointAdjustmentItemModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }        
        public int PosmProductId { get; set; }
        public POSMProductModel PosmProduct { get; set; }
        public int SystemQuantity { get; set; }
        public int AdjustedQuantity { get; set; }
    }

    public static class SalesPointAdjustmentItemModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointAdjustmentItem, SalesPointAdjustmentItemModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static SalesPointAdjustmentItemModel MapToModel(this SalesPointAdjustmentItem source)
        {
            var result = Mapper.Map<SalesPointAdjustmentItemModel>(source);
            return result;
        }

        public static List<SalesPointAdjustmentItemModel> MapToModel(this IEnumerable<SalesPointAdjustmentItem> source)
        {
            var result = Mapper.Map<List<SalesPointAdjustmentItemModel>>(source);
            return result;
        }

    }
}
