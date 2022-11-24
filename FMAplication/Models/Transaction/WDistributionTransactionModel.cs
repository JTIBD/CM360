using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Extensions;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Transaction
{
    public class WDistributionTransactionModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProductModel { get; set; }
        public int Quantity { get; set; }
    }

    public static class WDistributionTransactionExtensions{
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.WDistributionTransaction, WDistributionTransactionModel>().ForMember(x => x.POSMProductModel, m => m.MapFrom(u => u.POSMProduct.ToMap<POSMProduct, POSMProductModel>()));            
        }).CreateMapper();

        public static WDistributionTransactionModel MapToModel(this Domain.Transaction.WDistributionTransaction source)
        {
            var result = Mapper.Map<WDistributionTransactionModel>(source);
            return result;
        }

        public static List<WDistributionTransactionModel> MapToModel(this IEnumerable<Domain.Transaction.WDistributionTransaction> source)
        {
            var result = Mapper.Map<List<WDistributionTransactionModel>>(source);
            return result;
        }
    }
}
