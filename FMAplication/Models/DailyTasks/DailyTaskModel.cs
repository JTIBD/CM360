using System;
using System.Collections.Generic;
using AutoMapper;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Models.Bases;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using FMAplication.Models.Users;

namespace FMAplication.Models.DailyTasks
{
    public class DailyTaskModel:IWithSalesPoint
    {
        public int Id { get; set; }
        public CMUserModel CmUser { get; set; }
        public int CmUserId { get; set; }
        public SalesPointModel SalesPoint { get; set; }
        public int SalesPointId { get; set; }
        public string DateTimeStr { get; set; }
        public bool IsSubmitted { get; set; }
        public List<DailyPosmTaskModel> DailyPosmTasks { get; set; }
        public List<DailyAuditTaskModel> DailyAuditTasks { get; set; }
        public List<DailySurveyTaskModel> DailySurveyTasks { get; set; }
        public List<DailyConsumerSurveyTaskModel> DailyConsumerSurveyTasks { get; set; }
        public List<DailyAVTaskModel> DailyAVTasks { get; set; }
        public List<DailyCommunicationTaskModel> DailyCommunicationTasks { get; set; }
        public List<DailyInformationTaskModel> DailyInformationTasks { get; set; }
    }

    public static class DailyTaskModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailyTask, DailyTaskModel>().ForMember(m => m.DailyPosmTasks,
                    c => c.MapFrom(s => s.DailyPosmTasks.MapToModel()))
                .ForMember(m => m.DailyAuditTasks, c => c.MapFrom(s => s.DailyAuditTasks.MapToModel()))
                .ForMember(m => m.DailySurveyTasks, c => c.MapFrom(s => s.DailySurveyTasks.MapToModel()))
                .ForMember(m => m.DailyConsumerSurveyTasks, c => c.MapFrom(s => s.DailyConsumerSurveyTasks.MapToModel()))
                .ForMember(m => m.DailyAVTasks, c => c.MapFrom(s => s.DailyAVTasks.MapToModel()))
                .ForMember(m => m.DailyCommunicationTasks, c => c.MapFrom(s => s.DailyCommunicationTasks.MapToModel()))
                .ForMember(m => m.DailyInformationTasks, c => c.MapFrom(s => s.DailyInformationTasks.MapToModel()));
            cfg.CreateMap<CMUser, CMUserModel>();
        }).CreateMapper();

        public static DailyTaskModel MapToModel(this DailyTask source)
        {
            var result = Mapper.Map<DailyTaskModel>(source);
            return result;
        }

        public static List<DailyTaskModel> MapToModel(this IEnumerable<DailyTask> source)
        {
            var result = Mapper.Map<List<DailyTaskModel>>(source);
            return result;
        }

    }
}