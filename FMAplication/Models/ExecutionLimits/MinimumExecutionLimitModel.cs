using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.ExecutionLimits;
using FMAplication.Domain.Sales;
using FMAplication.Models.Bases;
using FMAplication.Models.Guidelines;
using FMAplication.Models.Sales;

namespace FMAplication.Models.ExecutionLimits
{
    public class MinimumExecutionLimitModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public int SalesPointId { get; set; }
        public string SalesPointName { get; set; }
        public string SalesPointCode { get; set; }
        public int TargetVisitedOutlet{ get; set; }
    }

    public static class MinimumExecutionLimitExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<MinimumExecutionLimit, MinimumExecutionLimitModel>()
                .ForMember(x => x.SalesPointCode, y => y.MapFrom(z => z.Code));
        }).CreateMapper();

        public static List<MinimumExecutionLimitModel> MapToModel(this IEnumerable<MinimumExecutionLimit> source)
        {
            var result = Mapper.Map<List<MinimumExecutionLimitModel>>(source);
            return result;
        }

        public static MinimumExecutionLimitModel MapToModel(this MinimumExecutionLimit source)
        {
            var result = Mapper.Map<MinimumExecutionLimitModel>(source);
            return result;
        }
    }
}
