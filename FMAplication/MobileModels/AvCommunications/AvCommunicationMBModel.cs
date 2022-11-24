using System.Collections.Generic;
using AutoMapper;
using FMAplication.Core;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Brand;
using FMAplication.Helpers;
using FMAplication.MobileModels.Brands;
using Newtonsoft.Json;

namespace FMAplication.MobileModels.AvCommunications
{
    public class AvCommunicationMBModel
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public BrandMBModel Brand { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
    }

    public static class AvCommunicationMBExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AvCommunication, AvCommunicationMBModel>();
            cfg.CreateMap<Brand, BrandMBModel>();
        }).CreateMapper();

        public static AvCommunicationMBModel MapToMBModel(this AvCommunication source)
        {
            var result = Mapper.Map<AvCommunicationMBModel>(source);
            return result;
        }

        public static List<AvCommunicationMBModel> MapToMBModel(this IEnumerable<AvCommunication> source)
        {
            var result = Mapper.Map<List<AvCommunicationMBModel>>(source);
            return result;
        }
    }
}