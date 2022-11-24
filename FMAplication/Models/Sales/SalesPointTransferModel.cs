using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Sales;
using FMAplication.Domain.WareHouse;
using FMAplication.Extensions;
using FMAplication.Models.Transaction;
using FMAplication.Models.TransactionNotifications;
using FMAplication.Models.TransactionWorkflow;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.Sales
{
    public class SalesPointTransferModel
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
        public List<SalesPointTransferItemModel> Items { get; set; }

        public TransactionWorkflowModel TransactionWorkflow { get; set; }
        public TransactionNotificationModel TransactionNotification { get; set; }

    }

    public static class SalesPointTransferModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPointTransfer, SalesPointTransferModel>().ForMember(m => m.Items,
                c => c.MapFrom(x => x.SalesPointTransferItems.MapToModel()));
            cfg.CreateMap<SalesPoint, SalesPointModel>();
        }).CreateMapper();

        public static SalesPointTransferModel MapToModel(this SalesPointTransfer source)
        {
            var result = Mapper.Map<SalesPointTransferModel>(source);
            return result;
        }

        public static List<SalesPointTransferModel> MapToModel(this IEnumerable<SalesPointTransfer> source)
        {
            var result = Mapper.Map<List<SalesPointTransferModel>>(source);
            return result;
        }

    }
}
