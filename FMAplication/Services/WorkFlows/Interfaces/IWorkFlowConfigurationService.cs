using FMAplication.Models.WorkFlows;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.WorkFlows.Interfaces
{
    public interface IWorkFlowConfigurationService
    {
        Task<List<WorkFlowModel>> GetPagedQueryWorkFlowConfigurationsAsync();
        Task<WorkFlowModel> GetWorkFlowConfigurationAsync(int id);
        Task<List<WorkFlowConfigurationModel>> SaveAsync(WorkFlowConfigurationModel model);
        Task<WorkFlowConfigurationModel> UpdateAsync(WorkFlowConfigurationModel model);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<WorkFlowConfigurationModel>> GetWorkFlowConfigurationsByWorkflowIdAsync(int id);
    }
}

