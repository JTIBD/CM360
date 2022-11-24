using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Controllers.Common;
using FMAplication.Domain.DailyTasks;
using FMAplication.Exceptions;
using FMAplication.Filters;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.Services.Surveys.interfaces;
using FMAplication.MobileModels.Tasks;
using FMAplication.Models.Common;
using FMAplication.Models.DailyTasks;
using FMAplication.RequestModels;
using FMAplication.Services.DailyTasks.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FMAplication.Controllers.DailyActivities
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DailyTaskController : BaseController
    {
        private readonly IDailyTaskService _dailyTaskService;
        private readonly ISurveyService _survey;

        public DailyTaskController(IDailyTaskService dailyTaskService, ISurveyService survey)
        {
            _dailyTaskService = dailyTaskService;
            _survey = survey;
        }
        [JwtAuthorize]
        [HttpGet("GetDailyTasks")]
        public async Task<ActionResult<DailyTaskAssignMBModel>> GetDailyTasks()
        {
            var loggedinUser = AppIdentity.AppUser;
            DailyTaskAssignMBModel result = await _dailyTaskService.GetDailyTasks(loggedinUser.UserId);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("SubmitDailyTask")]
        public async Task<ActionResult<bool>> SubmitDailyTask(DailyTaskSubmitModel model)
        {
            try
            {
                var loggedinUser = AppIdentity.AppUser;
                Console.WriteLine($"SubmitDailyTask execution started. DailyTaskId: {model.DailyTaskId}");
                var result = await _dailyTaskService.SubmitDailyTask(loggedinUser.UserId, model.DailyTaskId);
                Console.WriteLine($"SubmitDailyTask execution ended. DailyTaskId: {model.DailyTaskId}");
                return Ok(result);
            }
            catch (DailyTaskAlreadySubmittedException exception)
            {
                return Problem(statusCode: 508, title: exception.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }


        [JwtAuthorize]
        [HttpPost("UploadFile")]
        public async Task<ActionResult<UploadImageResponseModel>> UploadFile([FromForm] IFormFile file, [FromForm] string type)
        {
            string result = await _dailyTaskService.UploadFile(file, type);
            return Ok(new UploadImageResponseModel { Url = result});
        }

        [JwtAuthorize]
        [HttpPost("UploadDailyPosmTask")]
        public async Task<ActionResult<DailyPosmTaskMBModel>> UploadDailyPosmTask(DailyPosmTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
            DailyPosmTaskMBModel result = await _dailyTaskService.UploadDailyPosmTask(loggedinUser.UserId, model);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("UploadDailySurvey")]
        public async Task<ActionResult<DailySurveyTaskMBModel> > UploadDailySurvey(DailySurveyTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
        
            DailySurveyTaskMBModel result = await _dailyTaskService.UploadDailySurveryTask(loggedinUser.UserId, model);
            return Ok(result);
        }
        
        
        [JwtAuthorize]
        [HttpPost("UploadConsumerSurvey")]
        public async Task<ActionResult<DailySurveyTaskMBModel> > UploadConsumerSurvey(List<DailyConsumerSurveyTaskMBModel> model)
        {
            try
            {
                var loggedinUser = AppIdentity.AppUser;
                if (model == null || model.Count == 0)
                {
                    Console.WriteLine($"UploadConsumerSurvey called with no data.");
                    return Ok();
                }
                Console.WriteLine($"UploadConsumerSurvey Execution Started. Total Survey: {model.Count}");
                await _dailyTaskService.UploadConsumerSurvey(loggedinUser.UserId, model);
                Console.WriteLine($"UploadConsumerSurvey Execution Ended.");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UploadConsumerSurvey. Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [JwtAuthorize]
        [HttpPost("UploadDailyAv")]
        public async Task<ActionResult<DailyAvTaskMBModel>> UploadDailyAv(DailyAvTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
        
            DailyAvTaskMBModel result = await _dailyTaskService.UploadDailyAvTask(loggedinUser.UserId, model);
            return Ok(result);
        
        }

        [JwtAuthorize]
        [HttpPost("UploadDailyCommunication")]
        public async Task<ActionResult<DailyCommunicationTaskMBModel>> UploadDailyCommunication(DailyCommunicationTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
        
            DailyCommunicationTaskMBModel result = await _dailyTaskService.UploadDailyCommunicationTask(loggedinUser.UserId, model);
            return Ok(result);
        }

        [JwtAuthorize]
        [HttpPost("UploadDailyInformation")]
        public async Task<ActionResult<DailyInformationTaskMBModel>> UploadDailyInformation(DailyInformationTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
        
            DailyInformationTaskMBModel result = await _dailyTaskService.UploadDailyInformation(loggedinUser.UserId, model);
            return Ok(result);
        
        }

        [JwtAuthorize]
        [HttpPost("UploadDailyAudit")]
        public async Task<ActionResult<DailyAuditTaskMBModel>> UploadDailyAudit(DailyAuditTaskMBModel model)
        {
            var loggedinUser = AppIdentity.AppUser;
        
            DailyAuditTaskMBModel result = await _dailyTaskService.UploadDailyAudits(loggedinUser.UserId, model);
            return Ok(result);
        
        }


        [JwtAuthorize]
        [HttpPost("SubmitCompleteTask")]
        public async Task<ActionResult<SubmitDailyTaskViewModel>> SubmitCompleteTask(SubmitDailyTaskViewModel model)
        {
            try
            {
                var loggedinUser = AppIdentity.AppUser;
                Console.WriteLine($"SubmitCompleteTask execution started. DailyTaskId: {model.DailyTaskId}");
                bool result = await _dailyTaskService.SubmitTask(loggedinUser.UserId, model);
                Console.WriteLine($"SubmitCompleteTask execution ended. DailyTaskId: {model.DailyTaskId}");
                return Ok(result);
            }
            catch (DailyTaskAlreadySubmittedException exception)
            {
                return Problem(statusCode: 508, title: exception.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
