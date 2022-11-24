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
    public class WDistributionRecieveTransactionModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public int Quantity { get; set; }
        public int RecievedQuantity { get; set; }
    }

    public static class WDistributionRecieveTransactionExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.WDistributionRecieveTransaction, WDistributionRecieveTransactionModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static WDistributionRecieveTransactionModel MapToModel(this Domain.Transaction.WDistributionRecieveTransaction source)
        {
            var result = Mapper.Map<WDistributionRecieveTransactionModel>(source);
            return result;
        }

        public static List<WDistributionRecieveTransactionModel> MapToModel(this IEnumerable<Domain.Transaction.WDistributionRecieveTransaction> source)
        {
            var result = Mapper.Map<List<WDistributionRecieveTransactionModel>>(source);
            return result;
        }
    }

}
