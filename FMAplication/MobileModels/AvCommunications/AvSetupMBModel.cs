using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.AVCommunications;

namespace FMAplication.MobileModels.AvCommunications
{
    public class AvSetupMBModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesPointId { get; set; }
        public AvCommunicationMBModel Av { get; set; }
        public string fromDateStr { get; set; }
        public string toDateStr { get; set; }

        public int DailyTaskId { get; set; }
    }

    public static class AvSetupMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AvSetup, AvSetupMBModel>().ForMember(x => x.Av, m => m.MapFrom(u => u.Av.MapToMBModel()));
            cfg.CreateMap<AvCommunication, AvCommunicationMBModel>();
        }).CreateMapper();

        public static AvSetupMBModel MapToMBModel(this AvSetup source)
        {
            var result = Mapper.Map<AvSetupMBModel>(source);
            return result;
        }

        public static List<AvSetupMBModel> MapToMBModel(this IEnumerable<AvSetup> source)
        {
            var result = Mapper.Map<List<AvSetupMBModel>>(source);
            return result;
        }
    }
}