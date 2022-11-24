using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FMAplication.common;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Task;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Migrations;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.MobileModels.Tasks;
using FMAplication.Models.DailyTasks;
using FMAplication.Models.PosmAssign;
using FMAplication.Repositories;
using FMAplication.RequestModels;
using FMAplication.Services.AvCommunication.Interfaces;
using FMAplication.Services.AzureStorageService.Interfaces;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.DailyAudits.Interfaces;
using FMAplication.Services.DailyTasks.Interfaces;
using FMAplication.Services.ExecutionLimit.Interfaces;
using FMAplication.Services.Guidelines.Interfaces;
using FMAplication.Services.inventory.interfaces;
using FMAplication.Services.Reasons.Interfaces;
using FMAplication.Services.Sales.Interfaces;
using FMAplication.Services.Surveys.interfaces;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;
using TaskStatus = FMAplication.Enumerations.TaskStatus;

namespace FMAplication.Services.DailyTasks.Implementation
{
    public class DailyTaskService : IDailyTaskService
    {
        private readonly IPosmAssignService _posmAssign;
        private readonly ICMUserService _cmUserService;
        private readonly ISurveyService _survey;
        private readonly IAvSetupService _avSetup;
        private readonly ICommonService _common;
        private readonly IAvCommunicationService _avCommunication;
        private readonly ICommunicationSetupService _communicationSetupService;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<DailyPosmTaskItems> _dailyPosmTaskItem;
        private readonly IRepository<DailySurveyTask> _dailySurvey;
        private readonly IRepository<DailySurveyTaskAnswer> _dailySurveyAnswer;
        private readonly IRepository<DailyAVTask> _dailyAv;
        private readonly IRepository<DailyCommunicationTask> _dailyCommunication;
        private readonly IRepository<DailyInformationTask> _dailyInformation;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDailyAuditService _dailyAudit;
        private readonly IRepository<DailyAuditTask> _dailyAuditTask;
        private readonly IRepository<DailyPosmAuditTask> _dailyPosmAudit;
        private readonly IRepository<DailyProductsAuditTask> _dailyProductAudit;
        private readonly IReasonService _reasonService;
        private readonly IGuidelineSetupService _guidelineSetupService;
        private readonly IRepository<DailyConsumerSurveyTask> _surveyConsumer;
        private readonly IRepository<DailyConsumerSurveyTaskAnswer> _surveyConsumerAnswer;
        private readonly IMinimumExecutionLimitService _minimumExecution;
        private readonly IOutletService _outletService;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IInventoryService _inventoryService;
        private readonly ISPWisePosmLedgerService _posmLedger;


        public DailyTaskService(IPosmAssignService posmAssign, ICMUserService cmUserService, ISurveyService survey, IAvSetupService avSetup,
            IAvCommunicationService avCommunication, ICommunicationSetupService communicationSetupService,
            IRepository<DailyTask> dailyTask, IRepository<DailyPosmTask> dailyPosmTask,
            IRepository<DailyPosmTaskItems> dailyPosmTaskItem, 
            IRepository<DailySurveyTask> dailySurvey, IRepository<DailySurveyTaskAnswer> dailySurveyAnswer,
            IRepository<DailyAVTask> _dailyAV, IRepository<DailyCommunicationTask> dailyCommunication,
            IRepository<DailyInformationTask> dailyInformation, IDailyAuditService dailyAudit,
            IRepository<DailyAuditTask> dailyAuditTask, IRepository<DailyPosmAuditTask> dailyPosmAudit, 
            IRepository<DailyProductsAuditTask> dailyProductAudit, IReasonService reasonService,
            IGuidelineSetupService guidelineSetupService,
            IRepository<DailyConsumerSurveyTask> surveyConsumer, IRepository<DailyConsumerSurveyTaskAnswer> surveyConsumerAnswer,
            IMinimumExecutionLimitService minimumExecution, IOutletService outletService,
            IBlobStorageService blobStorageService, ICommonService common, 
            IRepository<SalesPoint> salesPoint, IInventoryService inventoryService, 
            ISPWisePosmLedgerService posmLedger)
        {
            _posmAssign = posmAssign;
            _cmUserService = cmUserService;
            _survey = survey;
            _avSetup = avSetup;
            _avCommunication = avCommunication;
            _communicationSetupService = communicationSetupService;
            _dailyTask = dailyTask;
            _dailyPosmTask = dailyPosmTask;
            _dailyPosmTaskItem = dailyPosmTaskItem;
            _dailySurvey = dailySurvey;
            _dailySurveyAnswer = dailySurveyAnswer;
            _dailyAv = _dailyAV;
            _dailyCommunication = dailyCommunication;
            _dailyInformation = dailyInformation;
            _blobStorageService = blobStorageService;
            _dailyAudit = dailyAudit;
            _dailyAuditTask = dailyAuditTask;
            _dailyPosmAudit = dailyPosmAudit;
            _dailyProductAudit = dailyProductAudit;
            _reasonService = reasonService;
            _guidelineSetupService = guidelineSetupService;
            _surveyConsumer = surveyConsumer;
            _surveyConsumerAnswer = surveyConsumerAnswer;
            _minimumExecution = minimumExecution;
            _outletService = outletService;
            _common = common;
            _salesPoint = salesPoint;
            _inventoryService = inventoryService;
            _posmLedger = posmLedger;
        }

        public async Task<DailyTaskAssignMBModel> GetDailyTasks(int userId)
        {
            var dailyTask = new DailyTaskAssignMBModel();
            dailyTask.TaskDate = DateTime.UtcNow.BangladeshDateInUtc();
            var salesPoint =  await _cmUserService.GetSalesPointByCmUser(userId);
            dailyTask.SalesPointModels =  await _cmUserService.GetSalesPointsByHhtUser(userId);
            dailyTask.PosmTasks = await _posmAssign.GetPosmTasks(userId, salesPoint.Id, dailyTask.TaskDate);
            dailyTask.SurveyTasks = await _survey.GetSurveysOfTodayByUser(userId);
            dailyTask.ConsumerSurveyTasks = await _survey.GetConsumerSurveysOfTodayByUser(userId);
            dailyTask.AvTasks = await _avSetup.GetAvSetupsOfTodayByUser(userId);
            dailyTask.CommunicationTasks = await _communicationSetupService.GetCommunicationSetupOfTodayByUser(userId);
            dailyTask.AuditTasks = await _dailyAudit.GetAuditSetupsOfTodayByUser(userId);
            dailyTask.Reasons = await _reasonService.GetAllReasonsWithTypes();
            dailyTask.Guidelines = await _guidelineSetupService.GetGuidelineSetupOfTodayByUser(userId);
            dailyTask.MinimumExecutionLimits = await _minimumExecution.GetAll(userId);
            return dailyTask;
        }

        public async Task<DailyTaskAssignMBModel> GetDailyTasks(DailyTask task)
        {
            var dailyTaskAssignResult = new DailyTaskAssignMBModel
            {
                PosmTasks = await _posmAssign.GetPosmTasks(task.CmUserId, task.SalesPointId, task.DateTime),
                SurveyTasks = await _survey.GetSurveysByTask(task),
                ConsumerSurveyTasks = await _survey.GetConsumerSurveysByTask(task),
                AvTasks = await _avSetup.GetAvSetupsOfTodayByUser(task),
                CommunicationTasks = await _communicationSetupService.GetCommunicationSetupByTask(task),
                AuditTasks = await _dailyAudit.GetAuditSetupsByTask(task)
            };

            return dailyTaskAssignResult;
        }

        public async Task<string> UploadFile(IFormFile file, string type)
        {
            if (file == null) throw new AppException("To upload a file, you must provide a file");
            var folderName = GetFolderByType(type);
            if (folderName == "") throw new AppException("You must provide a valid type");

            var fileInfo = await GetFileInfo(file);
            var image = await _blobStorageService.UploadFileToBlobAsync(fileInfo.fileName, fileInfo.fileData, fileInfo.mimeType, folderName);
            return image;
        }

        private string GetFolderByType(string type)
        {
            string folder = "";

            if (type.ToLower() == "posm")
                folder = "POSM"; 
            else if (type.ToLower() == "information")
                folder = "Information"; 
            else if (type.ToLower() == "signature")
                folder = "Signature";

            return folder;
        }

        public async Task<DailyTaskMBModel> GetOrCreateDailyTask(int cmUserId, DailyTaskMBModel model)
        {
            var checkExisting = await _dailyTask.FindAsync(x =>
                x.SalesPointId == model.SalesPointId && x.CmUserId == cmUserId &&
                x.DateTime.Date == model.DateTime.Date);

            if (checkExisting != null) 
                return checkExisting.ToMap<DailyTask, DailyTaskMBModel>();

            var dailyTaskModel = new DailyTask();
            dailyTaskModel.SalesPointId = model.SalesPointId;
            dailyTaskModel.CmUserId = cmUserId; 
            dailyTaskModel.DateTime = model.DateTime;

            await _dailyTask.CreateAsync(dailyTaskModel);
            var dailyTask = dailyTaskModel.ToMap<DailyTask, DailyTaskMBModel>();

            return dailyTask; 
        }

        public async Task<DailyPosmTaskMBModel> UploadDailyPosmTask(int loggedinUserUserId, DailyPosmTaskMBModel model)
        {
            var result =  await CreateDailyPosmTask(model);
            var data = result.ToMap<DailyPosmTask, DailyPosmTaskMBModel>();
            return data;
        }

        public async Task<DailySurveyTaskMBModel> UploadDailySurveryTask(int loggedinUserUserId, DailySurveyTaskMBModel model)
        {
            if (model.SurveyExecution == null) throw new AppException("Survey execution not found");
            var result = await CreateDailySurveyTask(model);
            return result;
        }

        public async Task UploadConsumerSurvey(int loggedinUserUserId, List<DailyConsumerSurveyTaskMBModel> list)
        {
            //var dailyConsumerSurveyTask = await _surveyConsumer.FindAsync(x => x.DailyTaskId == model.SurveyExecution.DailyTaskId);
            //if (dailyConsumerSurveyTask != null)
            //    await _surveyConsumer.DeleteAsync(dailyConsumerSurveyTask);
            int consumerSurveyIndex = 1;
            foreach (var model in list)
            {
                if (model.SurveyExecution == null) throw new AppException("Survey execution not found");
                Console.WriteLine($"Uploading ConsumerSurvey for dailytaskId: {model.SurveyExecution.DailyTaskId}. survey no: {consumerSurveyIndex}");
                var surveyConsumerTask = new DailyConsumerSurveyTask();
                surveyConsumerTask.IsCompleted = model.SurveyExecution.IsCompleted;
                surveyConsumerTask.DailyTaskId = model.SurveyExecution.DailyTaskId;
                surveyConsumerTask.SurveyQuestionSetId = model.SurveyExecution.SurveyQuestionSetId;
                if (model.QuestionAnswers.Count == 0)
                    surveyConsumerTask.IsCompleted = false;
                 
                await _surveyConsumer.CreateAsync(surveyConsumerTask);

                var dailySurveyTaskAnswers = new List<DailyConsumerSurveyTaskAnswer>();
                foreach (var answer in model.QuestionAnswers)
                {
                    dailySurveyTaskAnswers.Add(new DailyConsumerSurveyTaskAnswer
                    {
                        Answer = answer.Answer,
                        QuestionId = answer.QuestionId,
                        DailyConsumerSurveyTaskId = surveyConsumerTask.Id
                    });
                }
                await _surveyConsumerAnswer.CreateListAsync(dailySurveyTaskAnswers);
                Console.WriteLine($"Upload ConsumerSurvey for dailytaskId: {model.SurveyExecution.DailyTaskId}. survey no: {consumerSurveyIndex ++} Completed");
            }
        }

        public async Task<DailyAvTaskMBModel> UploadDailyAvTask(int loggedinUserUserId, DailyAvTaskMBModel model)
        {
            var result = await CreateDailyAvTask(model);
            var  data = result.ToMap<DailyAVTask, DailyAvTaskMBModel>();
            return data;
        }

        public async Task<DailyCommunicationTaskMBModel> UploadDailyCommunicationTask(int loggedinUserUserId, DailyCommunicationTaskMBModel model)
        {
            var result =  await CreateDailyCommunicationTask(model);
            var data = result.ToMap<DailyCommunicationTask, DailyCommunicationTaskMBModel>();
            return data;
        }

        public async Task<bool> SubmitDailyTask(int loggedinUserUserId, int dailyTaskId)
        {
            

            var task = await _dailyTask.FindAsync(x => x.Id == dailyTaskId && x.CmUserId == loggedinUserUserId);
            if (task == null) throw new AppException("Daily  task not found");
            if (task.IsSubmitted) throw new DailyTaskAlreadySubmittedException("Daily task already submitted");

            task.IsSubmitted = true;
            var result =await _dailyTask.UpdateAsync(task);

            var posmTaskItems = await GetDailyPosmItems(result);
            if (posmTaskItems.Count > 0)
            {
                await _inventoryService.SPStockRemove(posmTaskItems, result.SalesPointId);
                await _posmLedger.SPWisePOSMLedgerExecutedStock(posmTaskItems, result.SalesPointId);
            }
            return result != null;
        }

        

        public async Task<DailyInformationTaskMBModel> UploadDailyInformation(int loggedinUserUserId, DailyInformationTaskMBModel model)
        {
            var dailyInfoTask = new DailyInformationTask();
            dailyInfoTask.DailyTaskId = model.DailyTaskId;
            await OutletIdGuard(model.OutletId);
            dailyInfoTask.OutletId = model.OutletId;
            dailyInfoTask.IsCompleted = model.IsCompleted;
            dailyInfoTask.IsOutletOpen = model.IsOutletOpen;
           


            if (!dailyInfoTask.IsOutletOpen)
                dailyInfoTask.IsCompleted = false;
            

            if ( !dailyInfoTask.IsCompleted)
            {
                dailyInfoTask.ReasonId = model.ReasonId;
            }
            else
                GetInfoInsightAndRequestData(model, dailyInfoTask);
            
            var dailyInformation = await _dailyInformation.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.OutletId == model.OutletId);
            if (dailyInformation != null)
                await _dailyInformation.DeleteAsync(dailyInformation);

            var result = await _dailyInformation.CreateAsync(dailyInfoTask);
            var data = result.ToMap<DailyInformationTask, DailyInformationTaskMBModel>();

            data.Request = new DailyInformationTaskViewModel {Description = result.RequestDescription, Image = result.RequestImage};
            data.Insight = new DailyInformationTaskViewModel { Description = result.InsightDescription, Image = result.InsightImage};

            return data;
        }

        public async Task<DailyAuditTaskMBModel> UploadDailyAudits(int loggedinUserUserId, DailyAuditTaskMBModel model)
        {
            var dailyAuditTask = new DailyAuditTask();
            dailyAuditTask.DailyTaskId = model.DailyTaskId;
            dailyAuditTask.AuditSetupId = model.AuditSetupId;
            await OutletIdGuard(model.OutletId);
            dailyAuditTask.OutletId = model.OutletId;

           
            dailyAuditTask.IsOutletOpen = model.IsOutletOpen;
            dailyAuditTask.IsCompleted = model.IsCompleted;
            if (!dailyAuditTask.IsOutletOpen)
                dailyAuditTask.IsCompleted = false;

            if (!dailyAuditTask.IsCompleted)
                dailyAuditTask.ReasonId = model.ReasonId;
            
            var dailyAudit = await _dailyAuditTask.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.OutletId == model.OutletId);
            if (dailyAudit != null)
                await _dailyAuditTask.DeleteAsync(dailyAudit);
            var result = await _dailyAuditTask.CreateAsync(dailyAuditTask);

            List<DailyProductsAuditTask> dailyProductsAuditTasks = new List<DailyProductsAuditTask>();

            if (model.DailyProductsAudit != null)
            {
                foreach (var dailyProduct in model.DailyProductsAudit)
                {
                    dailyProductsAuditTasks.Add(new DailyProductsAuditTask
                    {
                        DailyAuditTaskId = result.Id, 
                        ActionType = dailyProduct.ActionType,
                        ProductId = dailyProduct.ProductId,
                        Result = dailyProduct.Result
                    });
                }
            }
            if (model.DailyPosmAudits != null)
            {
                foreach (var dailyProduct in model.DailyPosmAudits)
                {
                    if (Math.Abs(dailyProduct.Result) < 0.001 || (dailyProduct.ActionType == ActionType.PlanogramCheckProduct && ( Math.Abs(dailyProduct.Result) < 1.000 || Math.Abs(dailyProduct.Result) > 2.000)))
                        continue;
                    dailyProductsAuditTasks.Add(new DailyProductsAuditTask
                    {
                        DailyAuditTaskId = result.Id,
                        ActionType = dailyProduct.ActionType,
                        ProductId = dailyProduct.PosmProductId,
                        Result = dailyProduct.Result
                    });
                }
            }
            if(dailyProductsAuditTasks.Any()) await _dailyProductAudit.CreateListAsync(dailyProductsAuditTasks);

            return result.ToMap<DailyAuditTask, DailyAuditTaskMBModel>();
        }

        public async Task<bool> SubmitTask(int loggedinUserUserId, SubmitDailyTaskViewModel model)
        {

            var task = await _dailyTask.FindAsync(x => x.Id == model.DailyTaskId && x.CmUserId == loggedinUserUserId);
            if (task == null) throw new AppException("Daily task not found");
            
            if (task.IsSubmitted)
                throw new DailyTaskAlreadySubmittedException("Daily task already submitted");
            
            var dailyTaskAssignResult = await GetDailyTasks(task);

            if (model.DailyPosmTask != null && dailyTaskAssignResult.PosmTasks.Any())
                await UploadDailyPosmTask(loggedinUserUserId, model.DailyPosmTask);
            if(model.DailyAuditTask != null && dailyTaskAssignResult.AuditTasks.Any())
                await UploadDailyAudits(loggedinUserUserId, model.DailyAuditTask);
            if(model.DailyAvTask != null && dailyTaskAssignResult.AvTasks.Any())
                await UploadDailyAvTask(loggedinUserUserId, model.DailyAvTask);
            if(model.DailyInformationTask != null)
                await UploadDailyInformation(loggedinUserUserId, model.DailyInformationTask);
            if(model.DailyCommunicationTask != null && dailyTaskAssignResult.CommunicationTasks.Any())
                await UploadDailyCommunicationTask(loggedinUserUserId, model.DailyCommunicationTask);
            if (model.DailySurveyTask != null && dailyTaskAssignResult.SurveyTasks.Any())
            {
                if (model.DailySurveyTask.SurveyExecution != null &&
                    model.DailySurveyTask.SurveyExecution.DailyTaskId == 0)
                    model.DailySurveyTask.SurveyExecution.DailyTaskId = model.DailyTaskId;
                await UploadDailySurveryTask(loggedinUserUserId, model.DailySurveyTask);

            }
            return true;
        }

        #region private methods


      

        private async Task<List<DailyPosmTaskItems>> GetDailyPosmItems(DailyTask dailyTask)
        {
            var posmTask = await _dailyPosmTask.FindAsync(x => x.DailyTaskId == dailyTask.Id && x.IsCompleted);
            if (posmTask == null) return new List<DailyPosmTaskItems>();

            var items = await _dailyPosmTaskItem.FindAll(x => x.DailyPosmTaskId == posmTask.Id &&
                                                              (x.ExecutionType == PosmWorkType.Installation ||
                                                               x.ExecutionType == PosmWorkType.RemovalAndReInstallation))
                .ToListAsync();

            return items;
        }


        private void GetInfoInsightAndRequestData(DailyInformationTaskMBModel model, DailyInformationTask dailyInfoTask)
        {
            if (model.Insight != null)
            {
                dailyInfoTask.InsightImage = model.Insight.Image;
                dailyInfoTask.InsightDescription = model.Insight.Description;
            }

            if (model.Request != null)
            {
                dailyInfoTask.RequestDescription = model.Request.Description;
                dailyInfoTask.RequestImage = model.Request.Image;
            }
        }

        private async Task<(string fileName, string mimeType, byte[] fileData)> GetFileInfo(IFormFile file)
        {
            if (file == null) throw new AppException("File not found");
            var fileName = Path.GetFileName(file.FileName);
            string mimeType = "application/octet-stream";//model.File.ContentType;


            await using var target = new MemoryStream();
            await file.CopyToAsync(target);
            var fileData = target.ToArray();
            return (fileName, mimeType, fileData);
        }



        private async Task<DailyAVTask> CreateDailyAvTask(DailyAvTaskMBModel model)
        {
            var dailyAvTask = new DailyAVTask();
            dailyAvTask.IsCompleted = model.IsCompleted;
            dailyAvTask.DailyTaskId = model.DailyTaskId;
            await OutletIdGuard(model.OutletId);
            dailyAvTask.OutletId = model.OutletId;
            dailyAvTask.IsOutletOpen = model.IsOutletOpen;
           

            if (!dailyAvTask.IsOutletOpen)
                model.IsCompleted = false;

            if (!model.IsCompleted)
            {
                dailyAvTask.ReasonId = model.ReasonId;
            }
            else
                dailyAvTask.AvSetupId = model.AvSetupId;

            var dailyAv = await _dailyAv.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.OutletId == model.OutletId);
            if (dailyAv != null)
                await _dailyAv.DeleteAsync(dailyAv);

            var result = await _dailyAv.CreateAsync(dailyAvTask);
            return result;
        }
        private async Task<DailyAVTask> UpdateDailyAvTask(DailyAvTaskMBModel model)
        {
            var dailyAvTask = await _dailyAv.FindAsync(x=>x.Id == model.Id && x.DailyTaskId == model.DailyTaskId);
            dailyAvTask.IsCompleted = model.IsCompleted;
            dailyAvTask.DailyTaskId = model.DailyTaskId;
            dailyAvTask.AvSetupId = model.AvSetupId;

           
            var result = await _dailyAv.UpdateAsync(dailyAvTask);
            return result;
        }


        private async Task<DailyCommunicationTask> CreateDailyCommunicationTask(DailyCommunicationTaskMBModel model)
        {
            var dailyAvTask = new DailyCommunicationTask();
            dailyAvTask.IsCompleted = model.IsCompleted;
            dailyAvTask.DailyTaskId = model.DailyTaskId;
            await OutletIdGuard(model.OutletId);
            dailyAvTask.OutletId = model.OutletId;
            dailyAvTask.IsOutletOpen = model.IsOutletOpen;
            if (!dailyAvTask.IsOutletOpen)
                dailyAvTask.IsCompleted = false;
           
            if (!dailyAvTask.IsCompleted)
            {
                dailyAvTask.ReasonId = model.ReasonId;
            }
            else
                dailyAvTask.CommunicationSetupId = model.CommunicationSetupId;

            var dailyCommunication = await _dailyCommunication.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.OutletId == model.OutletId);
            if (dailyCommunication != null)
                await _dailyCommunication.DeleteAsync(dailyCommunication);
            var result = await _dailyCommunication.CreateAsync(dailyAvTask);
            return result;
        }
        private async Task<DailyCommunicationTask> UpdatDailyCommunicationTask(DailyCommunicationTaskMBModel model)
        {
            var dailyAvTask = await _dailyCommunication.FindAsync(x => x.Id == model.Id && x.DailyTaskId == model.DailyTaskId);
            dailyAvTask.IsCompleted = model.IsCompleted;
            dailyAvTask.DailyTaskId = model.DailyTaskId;
            dailyAvTask.CommunicationSetupId = model.CommunicationSetupId;


            var result = await _dailyCommunication.UpdateAsync(dailyAvTask);
            return result;
        }

        private async Task<DailySurveyTaskMBModel> CreateDailySurveyTask(DailySurveyTaskMBModel model)
        {
            var dailySurveyTask = new DailySurveyTask();
            dailySurveyTask.IsCompleted = model.SurveyExecution.IsCompleted;
            dailySurveyTask.DailyTaskId = model.SurveyExecution.DailyTaskId;
            await OutletIdGuard(model.SurveyExecution.OutletId);
            dailySurveyTask.OutletId = model.SurveyExecution.OutletId;
            
            dailySurveyTask.IsOutletOpen = model.SurveyExecution.IsOutletOpen;
            if (!dailySurveyTask.IsOutletOpen)
                dailySurveyTask.IsCompleted = false;



        
            dailySurveyTask.SurveyQuestionSetId = model.SurveyExecution.SurveyQuestionSetId;

            if (!dailySurveyTask.IsCompleted)
            {
                //if (model.SurveyExecution.ReasonId == null) throw new AppException("To submit as incomplete, You must provide a reason");
                dailySurveyTask.ReasonId = model.SurveyExecution.ReasonId;
            }
            var surveyTaskFromDb = await _dailySurvey.FindAsync(x => x.DailyTaskId == model.SurveyExecution.DailyTaskId && x.OutletId == model.SurveyExecution.OutletId);
            if (surveyTaskFromDb != null)
                await _dailySurvey.DeleteAsync(surveyTaskFromDb);
            var result = await _dailySurvey.CreateAsync(dailySurveyTask);

            if (result.IsCompleted)
            {
                var dailySurveyTaskAnswers = new List<DailySurveyTaskAnswer>();
                foreach (var answer in model.QuestionAnswers)
                {
                    dailySurveyTaskAnswers.Add(new DailySurveyTaskAnswer { Answer = answer.Answer, QuestionId = answer.QuestionId, DailySurveyTaskId = dailySurveyTask.Id });
                }
                await _dailySurveyAnswer.CreateListAsync(dailySurveyTaskAnswers);
            }
            var data = result.ToMap<DailySurveyTask, DailySurveyTaskMBModel>();
            return data;
        }

        private async Task<DailyPosmTask> CreateDailyPosmTask(DailyPosmTaskMBModel model)
        {
            var dailyPosmTask = new DailyPosmTask();
            dailyPosmTask.IsCompleted = model.IsCompleted;
            dailyPosmTask.DailyTaskId = model.DailyTaskId;
            dailyPosmTask.IsOutletOpen = model.IsOutletOpen;
            if (model.CheckInStr != null)
                dailyPosmTask.CheckIn = DateTime.Parse(model.CheckInStr).ToUniversalTime();
            if (model.CheckOutStr != null)
                dailyPosmTask.CheckOut = DateTime.Parse(model.CheckOutStr).ToUniversalTime();
            await OutletIdGuard(model.OutletId);
            dailyPosmTask.OutletId = model.OutletId;
            if (!dailyPosmTask.IsOutletOpen)
                dailyPosmTask.IsCompleted = false;

            if (!dailyPosmTask.IsCompleted)
                dailyPosmTask.ReasonId = model.ReasonId;
            else
            {
                if (model.ExistingImage != null) dailyPosmTask.ExistingImage = model.ExistingImage;
                if (model.NewImage != null) dailyPosmTask.NewImage = model.NewImage;
            }

            var posmTaskFromDb = await _dailyPosmTask.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.OutletId == model.OutletId);
            if (posmTaskFromDb != null)
                await _dailyPosmTask.DeleteAsync(posmTaskFromDb);
            var result = await _dailyPosmTask.CreateAsync(dailyPosmTask);

            if (result.IsCompleted)
            {
                var items = CreateDailyPosmTaskItems(model.DailyPosmTaskItems, result.Id);
                await _dailyPosmTaskItem.CreateListAsync(items);
            }

            return result;
        }
        private async Task<DailyPosmTask> UpdateDailyPosmTask(DailyPosmTaskMBModel model)
        {
            var dailyPosmTask = await  _dailyPosmTask.FindAsync(x => x.DailyTaskId == model.DailyTaskId && x.Id == model.Id);
            dailyPosmTask.IsCompleted = model.IsCompleted;
            dailyPosmTask.DailyTaskId = model.DailyTaskId;

            if (model.ExistingImage != dailyPosmTask.ExistingImage)  dailyPosmTask.ExistingImage = model.ExistingImage;
            if (model.NewImage != dailyPosmTask.NewImage) dailyPosmTask.NewImage = model.NewImage;

            //if (model.Reason.HasValue && model.IsCompleted)
            //{
            //    dailyPosmTask.Reason = model.Reason.Value;
            //    dailyPosmTask.ReasonDetails = model.ReasonDetails;
            //}
            var result = await _dailyPosmTask.UpdateAsync(dailyPosmTask);

            //remove existing items 
            var existingPosmItems =  _dailyPosmTaskItem.FindAll(x => x.DailyPosmTaskId == model.Id).ToList();
            await _dailyPosmTaskItem.DeleteListAsync(existingPosmItems);

            var items =  CreateDailyPosmTaskItems(model.DailyPosmTaskItems, model.Id);
            await _dailyPosmTaskItem.CreateListAsync(items);
            return result;
        }
        private  List<DailyPosmTaskItems> CreateDailyPosmTaskItems(List<DailyPosmTaskItemsMBModel> model, int dailyPosmTaskId)
        {
            var list = new List<DailyPosmTaskItems>();

            foreach (var m in model)
            {
                var dailyPosmTaskItem = new DailyPosmTaskItems();
                dailyPosmTaskItem.DailyPosmTaskId = dailyPosmTaskId;
                dailyPosmTaskItem.PosmProductId = m.PosmProductId;
                dailyPosmTaskItem.Quantity = m.Quantity;
                dailyPosmTaskItem.ExecutionType = m.ExecutionType;

                if (m.Image != null) dailyPosmTaskItem.Image = m.Image;

                list.Add(dailyPosmTaskItem);
            }

            return list;
        }

        private async Task OutletIdGuard(int outletId)
        {
            if (outletId  == 0)
                throw new AppException("Outlet is required");

            var outlet = await _outletService.GetOutletsByOutletId(outletId);
            if (outlet == null) throw new AppException("Outlet not found");
        } 
        
        #endregion
    }
}
