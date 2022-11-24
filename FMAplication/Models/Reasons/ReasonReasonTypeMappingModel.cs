using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Extensions;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Models.wareHouse;

namespace FMAplication.Models.Reasons
{
    public class ReasonReasonTypeMappingModel
    {
        public int Id { get; set; }
        public int ReasonId { get; set; }
        public ReasonModel Reason { get; set; }
        public int ReasonTypeId { get; set; }
        public ReasonTypeModel ReasonType { get; set; }
    }

    public static class ReasonReasonTypeMappingExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ReasonReasonTypeMapping, ReasonReasonTypeMappingModel>();
            cfg.CreateMap<ReasonType, ReasonTypeModel>();
            cfg.CreateMap<Reason, ReasonModel>();
        }).CreateMapper();

        public static ReasonReasonTypeMappingModel MapToModel(this ReasonReasonTypeMapping source)
        {
            var result = Mapper.Map<ReasonReasonTypeMappingModel>(source);
            return result;
        }

        public static List<ReasonReasonTypeMappingModel> MapToModel(this IEnumerable<ReasonReasonTypeMapping> source)
        {
            var result = Mapper.Map<List<ReasonReasonTypeMappingModel>>(source);
            return result;
        }

    }
}