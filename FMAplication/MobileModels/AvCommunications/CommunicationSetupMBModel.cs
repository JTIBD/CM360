using AutoMapper;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.MobileModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.AvCommunications
{
    public class CommunicationSetupMBModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesPointId { get; set; }        
        public AvCommunicationMBModel AvCommunication { get; set; }
        public string FromDateStr { get; set; }
        public string ToDateStr { get; set; }

        public int DailyTaskId { get; set; }
    }

    public static class CommunicationSetupMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CommunicationSetup, CommunicationSetupMBModel>().ForMember(x => x.AvCommunication, m => m.MapFrom(u => u.AvCommunication.MapToMBModel()));
            cfg.CreateMap<AvCommunication, AvCommunicationMBModel>();
        }).CreateMapper();

        public static CommunicationSetupMBModel MapToMBModel(this CommunicationSetup source)
        {
            var result = Mapper.Map<CommunicationSetupMBModel>(source);
            return result;
        }

        public static List<CommunicationSetupMBModel> MapToMBModel(this IEnumerable<CommunicationSetup> source)
        {
            var result = Mapper.Map<List<CommunicationSetupMBModel>>(source);
            return result;
        }

    }
}
