using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Task;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.MobileModels.Tasks;
using FMAplication.Models.DailyTasks;
using FMAplication.RequestModels;
using Microsoft.AspNetCore.Http;

namespace FMAplication.Services.DailyTasks.Interfaces
{
    public interface IDailyTaskService
    {
        Task<DailyTaskAssignMBModel> GetDailyTasks(int userId);
        Task<DailyTaskMBModel> GetOrCreateDailyTask(int cmUserId, DailyTaskMBModel model);
        Task<DailyPosmTaskMBModel> UploadDailyPosmTask(int loggedinUserUserId, DailyPosmTaskMBModel model);
   
        Task<DailySurveyTaskMBModel> UploadDailySurveryTask(int loggedinUserUserId, DailySurveyTaskMBModel model);
        Task<DailyAvTaskMBModel> UploadDailyAvTask(int loggedinUserUserId, DailyAvTaskMBModel model);
        Task<DailyCommunicationTaskMBModel> UploadDailyCommunicationTask(int loggedinUserUserId, DailyCommunicationTaskMBModel model);
        Task<string> UploadFile(IFormFile file, string type);
        Task<bool> SubmitDailyTask(int loggedinUserUserId, int dailyTaskId);
        Task<DailyInformationTaskMBModel> UploadDailyInformation(int loggedinUserUserId, DailyInformationTaskMBModel model);
        Task<DailyAuditTaskMBModel> UploadDailyAudits(int loggedinUserUserId, DailyAuditTaskMBModel model);
        Task<bool> SubmitTask(int loggedinUserUserId, SubmitDailyTaskViewModel model);
        Task UploadConsumerSurvey(int loggedinUserUserId, List<DailyConsumerSurveyTaskMBModel> list);
    }
}
