using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Core;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Models.Transaction;
using FMAplication.Models.TransactionNotifications;
using FMAplication.Models.Users;

namespace FMAplication.Models.TransactionWorkflow
{
    public class TransactionWorkflowModel
    {
        public int Id { get; set; }
        public int WorkFlowId { get; set; }

        public TWStatus TWStatus { get; set; }

        public int TransactionId { get; set; }

        public int? UserId { get; set; }
        public UserInfoModel User { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public int Sequence { get; set; }

        public int WorkflowConfigurationId { get; set; }
    }

    public static class TransactionWorkflowModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.TransactionWorkFlows.TransactionWorkflow, TransactionWorkflowModel>();
            cfg.CreateMap<UserInfo, UserInfoModel>();
        }).CreateMapper();

        public static TransactionWorkflowModel MapToModel(this Domain.TransactionWorkFlows.TransactionWorkflow source)
        {
            var result = Mapper.Map<TransactionWorkflowModel>(source);
            return result;
        }

        public static List<TransactionWorkflowModel> MapToModel(this IEnumerable<Domain.TransactionWorkFlows.TransactionWorkflow> source)
        {
            var result = Mapper.Map<List<TransactionWorkflowModel>>(source);
            return result;
        }

    }

}
