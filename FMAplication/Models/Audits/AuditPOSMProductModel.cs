using AutoMapper;
using FMAplication.Domain.Audit;
using FMAplication.Domain.Products;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Audits
{
    public class AuditPOSMProductModel
    {
        public int Id { get; set; }
        public int AuditSetupId { get; set; }        
        public int POSMProductId { get; set; }
        public POSMProductModel POSMProduct { get; set; }
        public ActionType ActionType { get; set; }

    }

    public static class AuditPOSMProductExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AuditPOSMProduct, AuditPOSMProductModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static AuditPOSMProductModel MapToModel(this AuditPOSMProduct source)
        {
            var result = Mapper.Map<AuditPOSMProductModel>(source);
            return result;
        }

        public static List<AuditPOSMProductModel> MapToModel(this IEnumerable<AuditPOSMProduct> source)
        {
            var result = Mapper.Map<List<AuditPOSMProductModel>>(source);
            return result;
        }

    }
}
