using FMAplication.common;
using FMAplication.MobileModels.Audits;
using FMAplication.Models.Audits;
using FMAplication.Models.DailyAudit;
using FMAplication.Models.DailyPOSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Enumerations;
using X.PagedList;

namespace FMAplication.Services.DailyAudits.Interfaces
{
    public interface IDailyAuditService
    {
        Task<IEnumerable<DailyAuditModel>> GetDailyAuditAsync();
        Task<IPagedList<DailyAuditModel>> GetPagedDailyAuditAsync(int pageNumber, int pageSize);
        Task<IEnumerable<DailyAuditModel>> GetQueryDailyAuditAsync();
        Task<DailyAuditModel> GetDailyAuditAsync(int id);
        Task<DailyAuditModel> SaveAsync(DailyAuditModel model);
        Task<DailyAuditModel> CreateAsync(DailyAuditModel model);
        Task<DailyAuditModel> UpdateAsync(DailyAuditModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(int activityId, int id);
        Task<bool> IsAuditSetupActive(int id, string code);
        Task<IEnumerable<DropdownOptions>> GetDropdownValueAsync();
        Task<List<AuditSetupModel>> Create(List<AuditSetupModel> payload);
        Task<Pagination<AuditSetupModel>> GetAuditSetups(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salesPointId);
        Task<AuditSetupModel> GetAuditSetupById(int id);
        Task<AuditSetupModel> UpdateAuditSetup(AuditSetupModel payload);
        Task<List<AuditSetupModel>> GetExistingAuditSetups(List<AuditSetupModel> payload);
        Task<List<AuditSetupMBModel>> GetAuditSetupsOfTodayByUser(int userId);
        Task<List<AuditSetupMBModel>> GetAuditSetupsByTask(DailyTask task);
    }
}
