using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Guidelines;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FMAplication.Models.Guidelines
{
    public class GuidelineSetupModel:BaseSetupModel
    {
        public string Code { get; set; }
        public string GuidelineText { get; set; }

    }

    public static class GuidelineSetupExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SalesPoint, SalesPointModel>();
                cfg.CreateMap<GuidelineSetup, GuidelineSetupModel>();
            }).CreateMapper();

        public static GuidelineSetupModel MapToModel(this GuidelineSetup source)
        {
            var result = Mapper.Map<GuidelineSetupModel>(source);
            return result;
        }

        public static List<GuidelineSetupModel> MapToModel(this IEnumerable<GuidelineSetup> source)
        {
            var result = Mapper.Map<List<GuidelineSetupModel>>(source);
            return result;
        }
    }
}
