using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper.QueryableExtensions.Impl;
using X.PagedList;

namespace FMAplication.Extensions
{
    public static class AutoMapperExtensions
    {

        // var result = model.Map<Category, CategoryModel>();
        public static TDest ToMap<TSource, TDest>(this TSource source)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDest>();
                cfg.ForAllMaps((typeMap, mappingExpr) =>
                {
                    foreach (var map in typeMap.PropertyMaps)
                    {
                        if (map.SourceType != map.DestinationType) map.Ignored = true;
                    }
                });
            }).CreateMapper();
            var result = mapper.Map<TDest>(source);
            return result;
        }

        public static List<TDest> ToMap<TSource, TDest>(this IEnumerable<TSource> source)
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDest>();
                cfg.ForAllMaps((typeMap, mappingExpr) =>
                {
                    foreach (var map in typeMap.PropertyMaps)
                    {
                        if (map.SourceType != map.DestinationType) map.Ignored = true;
                    }
                });
            }).CreateMapper();
            var result = mapper.Map<List<TDest>>(source);
            return result;
        }

        public static IQueryable<TDest> ToMap<TSource, TDest>(this IQueryable<TSource> sources)

        {
            return sources.NewModel().ToMap<TDest>();
        }

        public static IPagedList<TDest> ToMap<TSource, TDest>(this IPagedList<TSource> list)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            var sourceList = mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(list);

            return new StaticPagedList<TDest>(sourceList, list.GetMetaData());
        }
    }
}
