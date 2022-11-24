using AutoMapper;
using FMAplication.Domain.Audit;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using System.Collections.Generic;
using FMAplication.Models.Bases;

namespace FMAplication.Models.Audits
{
    public class AuditSetupModel:BaseSetupModel
    {
        public string Code { get; set; }
        public List<AuditProductModel> AuditProducts { get; set; }
        public List<AuditPOSMProductModel> AuditPOSMProducts { get; set; }
    }


    public static class AuditSetupExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AuditSetup, AuditSetupModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
            cfg.CreateMap<Product, ProductModel>();
            cfg.CreateMap<POSMProduct, POSMProductModel>();
        }).CreateMapper();

        public static AuditSetupModel MapToModel(this AuditSetup source)
        {
            var result = Mapper.Map<AuditSetupModel>(source);
            return result;
        }

        public static List<AuditSetupModel> MapToModel(this IEnumerable<AuditSetup> source)
        {
            var result = Mapper.Map<List<AuditSetupModel>>(source);
            return result;
        }

    }
}
