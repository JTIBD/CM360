using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Users;
using FMAplication.Domain.WorkFlows;
using FMAplication.Enumerations;
using FMAplication.Models.Questions;
using FMAplication.Models.Sales;
using FMAplication.Models.Survey;

namespace FMAplication.Models.WorkFlows 
{
    public class WorkFlowConfigurationModel
    {
        public int Id { get; set; }
       
        public int MasterWorkFlowId { get; set; }
        public string MasterWorkFlowName { get; set; }
        //public WorkFlow WorkFlow { get; set; }
       
        public Status Status { get; set; }

        public int[] TypeIds { get; set; }
        public int sequence { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int? UserId { get; set; }
        public UserInfo User { get; set; }
    }

    public static class WorkFlowConfigurationExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }).CreateMapper();

        public static WorkFlowConfigurationModel MapToModel(WorkFlowConfiguration source)
        {
            var result = Mapper.Map<WorkFlowConfigurationModel>(source);
            return result;
        }

        public static List<WorkFlowConfigurationModel> MapToModel(this IEnumerable<WorkFlowConfiguration> source)
        {
            var result = Mapper.Map<List<WorkFlowConfigurationModel>>(source);
            return result;
        }

    }
}
