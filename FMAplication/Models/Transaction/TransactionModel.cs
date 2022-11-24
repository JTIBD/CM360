using AutoMapper;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Sales;
using FMAplication.Models.Users;
using FMAplication.Models.wareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.Bases;
using FMAplication.Models.TransactionNotifications;
using FMAplication.Models.TransactionWorkflow;

namespace FMAplication.Models.Transaction
{
    public class TransactionModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int TransactionSerial { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr { get; set; }
        public string TransactionNumber { get; set; }
        public string ChalanNumber { get; set; }
        public WareHouseModel WareHouseModel { get; set; }
        public int? WarehouseId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public int SalesPointId { get; set; }
        public int ReferenceTransactionId { get; set; }
        public TransactionModel ReferenceTransaction { get; set; }
        public UserInfoModel CreatedByUser { get; set; }

        public TransactionWorkflowModel TransactionWorkflow { get; set; }
        public TransactionNotificationModel TransactionNotification { get; set; }


        public List<StockAddTransactionModel> StockAddTransactions { get; set; }
        public List<WDistributionTransactionModel> WDistributionTransactions { get; set; }
        public List<WDistributionRecieveTransactionModel> WDistributionRecieveTransactions { get; set; }
        public List<SalesPointAdjustmentItemModel> SalesPointAdjustmentItems { get; set; } = new List<SalesPointAdjustmentItemModel>();
    }

    public static class TransactionExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.Transaction, TransactionModel>().ForMember(x => x.WareHouseModel, m => m.MapFrom(u => u.WareHouse.ToMap<WareHouse, WareHouseModel>()))
                .ForMember(x => x.SalesPointAdjustmentItems, m => m.MapFrom(u => u.SalesPointAdjustmentItems.MapToModel()))
                .ForMember(x => x.WDistributionRecieveTransactions, m => m.MapFrom(u => u.WDistributionRecieveTransactions.MapToModel()))
                .ForMember(x => x.WDistributionTransactions, m => m.MapFrom(u => u.WDistributionTransactions.MapToModel()));
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<StockAddTransaction, StockAddTransactionModel>();
        }).CreateMapper();

        public static TransactionModel MapToModel(this Domain.Transaction.Transaction source)
        {            
            var result = Mapper.Map<TransactionModel>(source);
            return result;
        }

        public static List<TransactionModel> MapToModel(this IEnumerable<Domain.Transaction.Transaction> source)
        {
            var result = Mapper.Map<List<TransactionModel>>(source);
            return result;
        }

    }
}
