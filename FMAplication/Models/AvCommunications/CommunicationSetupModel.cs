using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;

namespace FMAplication.Models.AvCommunications
{
    public class CommunicationSetupModel:BaseSetupModel
    {
        public string Code { get; set; }
        public int AvCommunicationId { get; set; }
        public AvCommunicationViewModel AvCommunication { get; set; }
    }

    public static class CommunicationSetupExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CommunicationSetup, CommunicationSetupModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<AvCommunication, AvCommunicationViewModel>();
        }).CreateMapper();

        public static CommunicationSetupModel MapToModel(this CommunicationSetup source)
        {
            var result = Mapper.Map<CommunicationSetupModel>(source);
            return result;
        }

        public static List<CommunicationSetupModel> MapToModel(this IEnumerable<CommunicationSetup> source)
        {
            var result = Mapper.Map<List<CommunicationSetupModel>>(source);
            return result;
        }

    }
}
