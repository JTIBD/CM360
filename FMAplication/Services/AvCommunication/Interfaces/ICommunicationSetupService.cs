using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.DailyTasks;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.Models.AvCommunications;

namespace FMAplication.Services.AvCommunication.Interfaces
{
    public interface ICommunicationSetupService
    {
        Task<List<CommunicationSetupModel>> Save(List<CommunicationSetupModel> payload);
        Task<Pagination<CommunicationSetupModel>> GetAll(int pageSize, int pageIndex, DateTime fromDateTime, DateTime toDateTime, string search, int salesPointId);
        Task<CommunicationSetupModel> GetById(int id);
        Task<CommunicationSetupModel> Update(CommunicationSetupModel payload);
        Task<List<CommunicationSetupModel>> GetExistingCommunicationSetups(List<CommunicationSetupModel> payload);
        Task<List<CommunicationSetupMBModel>> GetCommunicationSetupOfTodayByUser(int userId);
        Task<List<CommunicationSetupMBModel>> GetCommunicationSetupByTask(DailyTask task);
        Task<bool> IsCommunicationSetUpActive(CommunicationSetupModel model);
    }
}
