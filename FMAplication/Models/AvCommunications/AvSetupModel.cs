using AutoMapper;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using FMAplication.Models.Bases;

namespace FMAplication.Models.AvCommunications
{
    public class AvSetupModel:BaseSetupModel
    {
        public string Code { get; set; }
        public int AvId { get; set; }
        public AvCommunicationViewModel Av { get; set; }
    }

    public static class AvSetupExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AvSetup, AvSetupModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<AvCommunication, AvCommunicationViewModel>();
        }).CreateMapper();

        public static AvSetupModel MapToModel(this AvSetup source)
        {
            var result = Mapper.Map<AvSetupModel>(source);
            return result;
        }

        public static List<AvSetupModel> MapToModel(this IEnumerable<AvSetup> source)
        {
            var result = Mapper.Map<List<AvSetupModel>>(source);
            return result;
        }

    }

}
