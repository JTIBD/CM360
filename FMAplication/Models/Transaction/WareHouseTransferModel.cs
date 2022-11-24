using AutoMapper;
using FMAplication.Domain.WareHouse;
using FMAplication.Enumerations;
using FMAplication.Models.wareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FMAplication.Extensions;

namespace FMAplication.Models.Transaction
{
    public class WareHouseTransferModel
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
        public List<WareHouseTransactionItemModel> Items { get; set; }
    }

    public static class WareHouseTransferModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Transaction.WareHouseTransfer, WareHouseTransferModel>();
            cfg.CreateMap<WareHouse, WareHouseModel>();
            //.ForMember(x => x.WareHouseModel, m => m.MapFrom(u => u.WareHouse.ToMap<WareHouse, WareHouseModel>()));

        }).CreateMapper();

        public static WareHouseTransferModel MapToModel(this Domain.Transaction.WareHouseTransfer source)
        {
            var result = Mapper.Map<WareHouseTransferModel>(source);
            return result;
        }

        public static List<WareHouseTransferModel> MapToModel(this IEnumerable<Domain.Transaction.WareHouseTransfer> source)
        {
            var result = Mapper.Map<List<WareHouseTransferModel>>(source);
            return result;
        }

    }
}
