using FMAplication.Domain.WorkFlows;
using FMAplication.Extensions;
using FMAplication.Models.WorkFlows;
using FMAplication.Repositories;
using FMAplication.Services.WorkFlows.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using X.PagedList;

namespace FMAplication.Services.WorkFlows.Implementation
{
    public class WorkFlowConfigurationService : IWorkFlowConfigurationService
    {
        private readonly IRepository<WorkFlowConfiguration> _workFlowConfiguration;
        private readonly IRepository<WorkFlowType> _workflowtype;
        private readonly IRepository<WorkFlow> _workFlow;

        public WorkFlowConfigurationService(IRepository<WorkFlowConfiguration> workFlowConfiguration, 
            IRepository<WorkFlowType> workflowtype, IRepository<WorkFlow> workFlow)
        {
            _workFlowConfiguration = workFlowConfiguration;
            _workflowtype = workflowtype;
            _workFlow = workFlow;
        }


        public async Task<int> DeleteAsync(int id)
        {
            var configList = await _workFlowConfiguration.FindAll(x => x.WorkFlowId == id).ToListAsync();
            var result = await _workFlowConfiguration.DeleteListAsync(configList);
            return result;
        }
        
        public async Task<WorkFlowModel> GetWorkFlowConfigurationAsync(int id)
        {
            var workflow = await _workFlow.FindAsync(x => x.Id == id);

            var workflowModel = workflow.ToMap<WorkFlow, WorkFlowModel>();
            var configList = await _workFlowConfiguration.FindAll(x => x.WorkFlowId == workflowModel.Id)
                .Include(x => x.Role)
                .Include(x => x.User).ToListAsync();
            workflowModel.ConfigList = configList.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
            return workflowModel;
        }

        public async Task<IEnumerable<WorkFlowConfigurationModel>> GetWorkFlowConfigurationsByWorkflowIdAsync(int id)
        {
            var result = await _workFlowConfiguration.FindAllAsync(s => s.WorkFlowId == id);
            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }

        public async Task<List<WorkFlowModel>>  GetPagedQueryWorkFlowConfigurationsAsync()
        {
            List<WorkFlowModel> list = new List<WorkFlowModel>();
            var configurations = await _workFlowConfiguration.GetAllActive()
                .GroupBy(x => x.WorkFlowId)
                .Select(x => new {workFlowId = x.Key }).ToListAsync();

            foreach (var configuration in configurations)
            {
                var workflow = await _workFlow.FindAsync(x => x.Id == configuration.workFlowId);
                
                var workflowModel = workflow.ToMap<WorkFlow,WorkFlowModel>();
                var configList = await _workFlowConfiguration.FindAll(x => x.WorkFlowId == workflowModel.Id)
                    .Include(x => x.Role)
                    .Include(x => x.User).ToListAsync();
                workflowModel.ConfigList = configList.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();

                list.Add(workflowModel);
                
            }
            return list;

        }

        public async Task<List<WorkFlowConfigurationModel>> SaveAsync(WorkFlowConfigurationModel model)
        {
            var workflow = await _workFlow.FindAsync(x => x.Id == model.MasterWorkFlowId);
            if (workflow == null) throw new AppException("Workflow not found");

            if (workflow.WorkflowStep != model.TypeIds.Length)
                throw new AppException("Workflow configuration steps doesn't match");

            var configs = await _workFlowConfiguration.Where(x => x.WorkFlowId == model.MasterWorkFlowId)
                .ToListAsync();

            if(configs.Any())
                await _workFlowConfiguration.DeleteListAsync(configs);

            var list = new List<WorkFlowConfiguration>();
            int sequence = 1;
            foreach (var typeId in model.TypeIds)
            {
                var config = new WorkFlowConfiguration
                {
                    WorkFlowId = model.MasterWorkFlowId, Sequence = sequence, Status = model.Status
                };
                if (workflow.WorkflowConfigType == WorkflowConfigType.User)
                    config.UserId = typeId;
                else
                    config.RoleId = typeId;

                list.Add(config);
                sequence++;
            }
            var result = await _workFlowConfiguration.CreateListAsync(list);
            return result.MapToModel();
        }

        public async Task<WorkFlowConfigurationModel> UpdateAsync(WorkFlowConfigurationModel model)
        {
            var example = model.ToMap<WorkFlowConfigurationModel, WorkFlowConfiguration>();
            var result = await _workFlowConfiguration.UpdateAsync(example);
            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }


    }
}

