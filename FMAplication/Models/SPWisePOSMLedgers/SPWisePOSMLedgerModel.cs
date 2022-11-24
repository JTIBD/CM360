using System;
using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Extensions;
using FMAplication.Models.Bases;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.SPWisePOSMLedgers
{
    public class SPWisePOSMLedgerModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public POSMProductModel PosmProduct { get; set; }
        public int PosmProductId { get; set; }
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public DateTime Date { get; set; }
        public int OpeningStock { get; set; }
        public int ReceivedStock { get; set; }
        public int ExecutedStock { get; set; }
        public int ClosingStock { get; set; }
    }

    public static class SPWisePOSMLedgerModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SPWisePOSMLedger, SPWisePOSMLedgerModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static SPWisePOSMLedgerModel MapToModel(this SPWisePOSMLedger source)
        {
            var result = Mapper.Map<SPWisePOSMLedgerModel>(source);
            return result;
        }

        public static List<SPWisePOSMLedgerModel> MapToModel(this IEnumerable<SPWisePOSMLedger> source)
        {
            var result = Mapper.Map<List<SPWisePOSMLedgerModel>>(source);
            return result;
        }

    }
}