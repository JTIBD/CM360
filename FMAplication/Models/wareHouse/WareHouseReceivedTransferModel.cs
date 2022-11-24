using AutoMapper;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Extensions;

namespace FMAplication.Models.wareHouse
{
    public class WareHouseReceivedTransferModel
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr => TransactionDate.ToIsoString();
        public int FromWarehouseId { get; set; }
        public WareHouseModel FromWarehouse { get; set; }
        public int ToWarehouseId { get; set; }
        public WareHouseModel ToWarehouse { get; set; }
        public WareHouseTransferModel SourceTransfer { get; set; }
        public int SourceTransferId { get; set; }
        public List<WareHouseReceivedTransferItemModel> Items { get; set; }
    }

    public static class WareHouseReceivedTransferModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WareHouseReceivedTransfer, WareHouseReceivedTransferModel>();
            cfg.CreateMap<WareHouse, WareHouseModel>();
        }).CreateMapper();

        public static WareHouseReceivedTransferModel MapToModel(this WareHouseReceivedTransfer source)
        {
            var result = Mapper.Map<WareHouseReceivedTransferModel>(source);
            return result;
        }

        public static List<WareHouseReceivedTransferModel> MapToModel(this IEnumerable<WareHouseReceivedTransfer> source)
        {
            var result = Mapper.Map<List<WareHouseReceivedTransferModel>>(source);
            return result;
        }

    }
}
