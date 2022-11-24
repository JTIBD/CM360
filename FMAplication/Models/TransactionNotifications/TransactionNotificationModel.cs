using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.TransactionNotifications
{
    public class TransactionNotificationModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public TransactionModel Transaction { get; set; }
        public TransactionType TransactionType { get; set; }
        public int TransactionWorkFlowId { get; set; }
        public bool IsSeen { get; set; }
        public TWStatus TwStatus { get; set; }
        public string SubmittedBy { get; set; }
    }

    public static class TransactionNotificationExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TransactionNotification, TransactionNotificationModel>();
            cfg.CreateMap<Domain.Transaction.Transaction, TransactionModel>();
        }).CreateMapper();

        public static TransactionNotificationModel MapToModel(this TransactionNotification source)
        {
            var result = Mapper.Map<TransactionNotificationModel>(source);
            return result;
        }

        public static List<TransactionNotificationModel> MapToModel(this IEnumerable<TransactionNotification> source)
        {
            var result = Mapper.Map<List<TransactionNotificationModel>>(source);
            return result;
        }

    }

}
