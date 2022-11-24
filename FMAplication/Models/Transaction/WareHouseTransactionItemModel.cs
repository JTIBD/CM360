using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class WareHouseTransactionItemModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
    }

    public static class WareHouseTransactionItemModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.WareHouseTransferItem, WareHouseTransactionItemModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static WareHouseTransactionItemModel MapToModel(this Domain.Transaction.WareHouseTransferItem source)
        {
            var result = Mapper.Map<WareHouseTransactionItemModel>(source);
            return result;
        }

        public static List<WareHouseTransactionItemModel> MapToModel(this IEnumerable<Domain.Transaction.WareHouseTransferItem> source)
        {
            var result = Mapper.Map<List<WareHouseTransactionItemModel>>(source);
            return result;
        }

    }
}
