using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Extensions;

namespace FMAplication.Models.Sales
{
    public class SalesPointReceivedTransferModel
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr => TransactionDate.ToIsoString();
        public int FromSalesPointId { get; set; }
        public SalesPointModel FromSalesPoint { get; set; }
        public int ToSalesPointId { get; set; }
        public SalesPointModel ToSalesPoint { get; set; }
        public int SourceTransferId { get; set; }
        public List<SalesPointReceivedTransferItemModel> Items { get; set; }
        public SalesPointTransferModel SourceTransfer { get; set; }
    }

    public static class SalesPointReceivedTransferModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointReceivedTransfer, SalesPointReceivedTransferModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
        }).CreateMapper();

        public static SalesPointReceivedTransferModel MapToModel(this SalesPointReceivedTransfer source)
        {
            var result = Mapper.Map<SalesPointReceivedTransferModel>(source);
            return result;
        }

        public static List<SalesPointReceivedTransferModel> MapToModel(this IEnumerable<SalesPointReceivedTransfer> source)
        {
            var result = Mapper.Map<List<SalesPointReceivedTransferModel>>(source);
            return result;
        }

    }
}
