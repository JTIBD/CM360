using AutoMapper;
using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class StockAddTransactionModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }        
        public int PosmProductId { get; set; }
        public string PosmProductCode { get; set; }
        public POSMProductModel PosmProductModel { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; }
    }

    public static class StockAddTransactionExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.StockAddTransaction, StockAddTransactionModel>().ForMember(x => x.PosmProductModel, m => m.MapFrom(u => u.PosmProduct.ToMap<POSMProduct, POSMProductModel>()));           
        }).CreateMapper();

        public static StockAddTransactionModel MapToModel(this Domain.Transaction.StockAddTransaction source)
        {
            var result = Mapper.Map<StockAddTransactionModel>(source);
            return result;
        }

        public static List<StockAddTransactionModel> MapToModel(this IEnumerable<Domain.Transaction.StockAddTransaction> source)
        {
            var result = Mapper.Map<List<StockAddTransactionModel>>(source);
            return result;
        }

    }
}
