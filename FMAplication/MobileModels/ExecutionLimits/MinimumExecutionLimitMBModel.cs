using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.ExecutionLimits;
using FMAplication.Domain.Sales;
using FMAplication.MobileModels.Sales;
using FMAplication.Models.Sales;

namespace FMAplication.MobileModels.ExecutionLimits
{
    public class MinimumExecutionLimitMBModel
    { 
        public int SalesPointId { get; set; }
        public int TargetVisitedOutlet{ get; set; }
    }

    public static class MinimumExecutionMBLimitExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MinimumExecutionLimit, MinimumExecutionLimitMBModel>();
        }).CreateMapper();

        public static List<MinimumExecutionLimitMBModel> MapToModel(this IEnumerable<MinimumExecutionLimit> source)
        {
            var result = Mapper.Map<List<MinimumExecutionLimitMBModel>>(source);
            return result;
        }

        public static MinimumExecutionLimitMBModel MapToModel(this MinimumExecutionLimit source)
        {
            var result = Mapper.Map<MinimumExecutionLimitMBModel>(source);
            return result;
        }
    }
}
