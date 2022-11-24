using FMAplication.common;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.Models.AvCommunications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;

namespace FMAplication.Services.AvCommunication.Interfaces
{
    public interface IAvSetupService
    {
        Task<List<AvSetupModel>> Create(List<AvSetupModel> payload);
        Task<Pagination<AvSetupModel>> GetSetups(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salespointId);
        Task<AvSetupModel> GetAvSetupById(int id);
        Task<AvSetupModel> UpdateAvSetup(AvSetupModel payload);
        Task<List<AvSetupModel>> GetExistingAvSetups(List<AvSetupModel> payload);
        Task<List<AvSetupMBModel>> GetAvSetupsOfTodayByUser(int userId);
        Task<List<AvSetupMBModel>> GetAvSetupsOfTodayByUser(DailyTask task);
        Task<bool> IsAvSetUpActive(AvSetupModel model);
    }
}
