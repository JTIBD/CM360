using AutoMapper;
using FMAplication.Domain.Audit;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Audits;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.MobileModels.Audits
{
    public class AuditSetupMBModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SalesPointId { get; set; }        
        public string FromDateStr { get; set; }
        public string ToDateStr { get; set; }
        public int DailyTaskId { get; set; }
        public List<AuditProductModel> AuditProducts { get; set; }
        public List<AuditPOSMProductModel> AuditPOSMProducts { get; set; }
    }

    public static class AuditSetupMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AuditSetup, AuditSetupMBModel>();
            cfg.CreateMap<SalesPoint, SalesPointModel>();
        }).CreateMapper();

        public static AuditSetupMBModel MapToMBModel(this AuditSetup source)
        {
            var result = Mapper.Map<AuditSetupMBModel>(source);
            return result;
        }

        public static List<AuditSetupMBModel> MapToMBModel(this IEnumerable<AuditSetup> source)
        {
            var result = Mapper.Map<List<AuditSetupMBModel>>(source);
            return result;
        }

    }
}
