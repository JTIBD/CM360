using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain;
using FMAplication.Enumerations;

namespace FMAplication.Models.Reasons
{
    public class ReasonModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ReasonInEnglish { get; set; }
        public string ReasonInBangla { get; set; }

        [Required]
        public Status Status { get; set; }

        public List<ReasonReasonTypeMappingModel> ReasonReasonTypeMappings { get; set; }
    }

    public static class ReasonModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ReasonReasonTypeMapping, ReasonReasonTypeMappingModel>();
            cfg.CreateMap<ReasonType, ReasonTypeModel>();
            cfg.CreateMap<Reason, ReasonModel>().ForMember(m => m.ReasonReasonTypeMappings,
                x => x.MapFrom(c => c.ReasonReasonTypeMappings.MapToModel()));
        }).CreateMapper();

        public static ReasonModel MapToModel(this Reason source)
        {
            var result = Mapper.Map<ReasonModel>(source);
            return result;
        }

        public static List<ReasonModel> MapToModel(this IEnumerable<Reason> source)
        {
            var result = Mapper.Map<List<ReasonModel>>(source);
            return result;
        }

    }
}
