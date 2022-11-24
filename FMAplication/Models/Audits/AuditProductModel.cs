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
    public class AuditProductModel
    {
        public int Id { get; set; }
        public int AuditSetupId { get; set; }        
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public ActionType ActionType { get; set; }
    }

    public static class AuditProductExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AuditProduct, AuditProductModel>();
            cfg.CreateMap<Product, ProductModel>();
        }).CreateMapper();

        public static AuditProductModel MapToModel(this AuditProduct source)
        {
            var result = Mapper.Map<AuditProductModel>(source);
            return result;
        }

        public static List<AuditProductModel> MapToModel(this IEnumerable<AuditProduct> source)
        {
            var result = Mapper.Map<List<AuditProductModel>>(source);
            return result;
        }

    }
}
