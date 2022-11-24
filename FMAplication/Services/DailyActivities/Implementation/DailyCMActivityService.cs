using AutoMapper;
using fm_application.Models.DailyActivity;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Examples;
using FMAplication.Domain.Products;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Reports;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.DailyAudit;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Products;
using FMAplication.Models.Questions;
using FMAplication.Models.Sales;
using FMAplication.Models.Users;
using FMAplication.Repositories;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.QuestionDetails.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList;
using System.Linq;
using FMAplication.Models.DailyActivity;
using Microsoft.Data.SqlClient;
using FMAplication.Services.FileUploads.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using FMAplication.Domain.DailyTasks;
using FMAplication.Models.DailyTasks;
using FMAplication.Models.Reports;
using FMAplication.RequestModels.Reports;
using FMAplication.Services.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using FMAplication.Services.FileUtility.Interfaces;
using FMAplication.Services.FileUtility.Implementation;

namespace FMAplication.Services.DailyActivities.Implementation
{
    public class DailyCMActivityService : IDailyCMActivityService
    {
        private const int V = 0;
        private readonly IRepository<DailyCMActivity> _repo;
        private readonly IRepository<Outlet> _outeltRepo;
        private readonly IQuestionSetService _survey;
        private readonly IRepository<DailyPOSM> _dailyPosm;
        private readonly IRepository<DailyAudit> _dailyAudit;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;


        public Dictionary<string, string> OptionEmoticonDict
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"VeryBad", @"U+1F601"},
                    {"Bad", @"U+1F601"},
                    {"Okay", @"U+1F601"},
                    {"Good", @"U+1F601"},
                    {"VeryGood", @"U+1F601"}
                };
            }
        }


        private readonly IRepository<POSMReport> _posmReport;
        private readonly IRepository<AuditReport> _auditReport;
        private readonly IRepository<SurveyReport> _surveyReport;
        private readonly IFileUploadService _fileUploadService;


        private readonly string _tempPath;
        private readonly ICommonService _common;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNodeMapping;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<Route> _route;
        private readonly IRepository<Channel> _channel;

        public DailyCMActivityService(IRepository<DailyCMActivity> repository, IQuestionSetService survey,
            IRepository<POSMReport> posmReport,
            IRepository<AuditReport> auditReport,
            IRepository<SurveyReport> surveyReport,
            IFileUploadService fileUploadService,
            IRepository<DailyPOSM> dailyPosm,
            IRepository<DailyAudit> dailyAudit,
            IRepository<CMUser> cmUser,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService, ICommonService common, IRepository<DailyPosmTask> dailyPosmTask, IRepository<SalesPoint> salesPoint, IRepository<SalesPointNodeMapping> salesPointNodeMapping, IRepository<DailyTask> dailyTask, IRepository<Route> route, IRepository<Channel> channel)
        {
            _repo = repository;
            _survey = survey;
            _posmReport = posmReport;
            _auditReport = auditReport;
            _surveyReport = surveyReport;
            _fileUploadService = fileUploadService;
            _dailyPosm = dailyPosm;
            _dailyAudit = dailyAudit;
            _cmUser = cmUser;
            _webHostEnvironment = webHostEnvironment;

            _tempPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp");
            _fileService = fileService;
            _common = common;
            _dailyPosmTask = dailyPosmTask;
            _salesPoint = salesPoint;
            _salesPointNodeMapping = salesPointNodeMapping;
            _dailyTask = dailyTask;
            _route = route;
            _channel = channel;
        }

        public async Task<(DailyCMActivityModel Data, bool IsExists)> SaveCMTask(DailyCMActivityModel model)
        {
            var result = new DailyCMActivity();

            if (model.DailyPOSM != null)
            {
                model = StoreProductsToPOSMProducts(model);
                foreach (var item in model.DailyPOSM.POSMProducts)
                {
                    if (!string.IsNullOrWhiteSpace(item.UploadedImageUrl1))
                    {
                        var fileName = item.DailyCMActivityId + "_" + model.CMId + "_" + model.Date.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString();
                        item.UploadedImageUrl1 = await _fileUploadService.SaveImageAsync(item.UploadedImageUrl1, fileName, FileUploadCode.POSMReport, 1200, 800);
                    }
                    if (!string.IsNullOrWhiteSpace(item.UploadedImageUrl2))
                    {
                        var fileName = item.DailyCMActivityId + "_" + model.CMId + "_" + model.Date.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString();
                        item.UploadedImageUrl2 = await _fileUploadService.SaveImageAsync(item.UploadedImageUrl2, fileName, FileUploadCode.POSMReport, 1200, 800);
                    }
                }
            }
            if (model.DailyAudit != null)
            {
                model = StoreProductsToAllProducts(model);
                foreach (var item in model.DailyAudit.AllProducts)
                {
                    if (!string.IsNullOrWhiteSpace(item.UploadedImageUrl1))
                    {
                        var fileName = item.DailyCMActivityId + "_" + model.CMId + "_" + model.Date.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString();
                        item.UploadedImageUrl1 = await _fileUploadService.SaveImageAsync(item.UploadedImageUrl1, fileName, FileUploadCode.AuditReport, 1200, 800);
                    }
                    if (!string.IsNullOrWhiteSpace(item.UploadedImageUrl2))
                    {
                        var fileName = item.DailyCMActivityId + "_" + model.CMId + "_" + model.Date.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString();
                        item.UploadedImageUrl2 = await _fileUploadService.SaveImageAsync(item.UploadedImageUrl2, fileName, FileUploadCode.AuditReport, 1200, 800);
                    }
                }
            }

            // model = mapSurveyReportQuestions(model);

            // foreach (var item in model.Surveys)
            // {
            //     model.SurveyQuestions.AddRange(item.SurveyReportQuestions);
            // }           

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyCMActivityModel, DailyCMActivity>()
                    //.ForMember(src => src.Outlet, opt => opt.Ignore())
                    .ForMember(src => src.AssignedFMUser, opt => opt.Ignore())
                    .ForMember(src => src.CM, opt => opt.Ignore());
                cfg.CreateMap<OutletModel, Outlet>();
                cfg.CreateMap<SurveyReporModel, SurveyReport>();
                cfg.CreateMap<DailyPOSMModel, DailyPOSM>();
                cfg.CreateMap<POSMProductModel, POSMProduct>();
                cfg.CreateMap<POSMReportModel, POSMReport>();
                cfg.CreateMap<DailyAuditModel, DailyAudit>();
                cfg.CreateMap<ProductModel, Product>();
                cfg.CreateMap<AuditReportModel, AuditReport>();
            }).CreateMapper();

            var dailyCMActivityEntity = mapper.Map<DailyCMActivity>(model);
            var existingData = _repo.GetAllIncludeStrFormat(e => e.Id == model.Id,
                                    includeProperties: "DailyPOSM,DailyPOSM.POSMProducts,DailyAudit,DailyAudit.AllProducts,SurveyQuestions");
            var existingDailyCMActivity = new DailyCMActivity();
            existingDailyCMActivity = existingData.ToListAsync().Result.Find(e => e.Id == model.Id);
            var res = await _repo.UpdateDailyCMActivityAsync(dailyCMActivityEntity, existingDailyCMActivity);

            // check duplicate
            if (res.IsExists) return (null, true);

            var mapperToModel = new MapperConfiguration(cfg =>
                                    {
                                        cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>();
                                        cfg.CreateMap<Outlet, OutletModel>();
                                        cfg.CreateMap<SurveyReport, SurveyReporModel>();
                                        cfg.CreateMap<DailyPOSM, DailyPOSMModel>();
                                        cfg.CreateMap<POSMProduct, POSMProductModel>();
                                        cfg.CreateMap<POSMReport, POSMReportModel>();
                                        cfg.CreateMap<DailyAudit, DailyAuditModel>();
                                        cfg.CreateMap<Product, ProductModel>();
                                        cfg.CreateMap<AuditReport, AuditReportModel>();
                                    }).CreateMapper();

            var savedModel = mapperToModel.Map<DailyCMActivityModel>(res.Data);

            if (model.IsPOSM && model.DailyPOSM != null)
            {
                savedModel = MapPOSMProducts(savedModel);
                savedModel.DailyPOSM.POSMProducts.Clear();
            }

            if (model.IsAudit && model.DailyAudit != null)
            {
                savedModel = MapAllProducts(savedModel);
                savedModel.DailyAudit.AllProducts.Clear();
            }

            return (savedModel, false);
        }

        private DailyCMActivityModel StoreProductsToAllProducts(DailyCMActivityModel model)
        {
            model.DailyAudit.AllProducts.AddRange(model.DailyAudit.DistributionCheckProducts);
            model.DailyAudit.AllProducts.AddRange(model.DailyAudit.FacingCountProducts);
            model.DailyAudit.AllProducts.AddRange(model.DailyAudit.PlanogramCheckProducts);
            model.DailyAudit.AllProducts.AddRange(model.DailyAudit.PriceAuditProducts);
            return model;
        }

        private DailyCMActivityModel StoreProductsToPOSMProducts(DailyCMActivityModel model)
        {
            model.DailyPOSM.POSMProducts.AddRange(model.DailyPOSM.POSMInstallationProducts);
            model.DailyPOSM.POSMProducts.AddRange(model.DailyPOSM.POSMRepairProducts);
            model.DailyPOSM.POSMProducts.AddRange(model.DailyPOSM.POSMRemovalProducts);
            return model;
        }

        public async Task<DailyCMActivityModel> GetCMTaskById(int id)
        {
            var data = _repo.GetAllIncludeStrFormat(e => e.Id == id,
                                    includeProperties: "DailyPOSM,DailyPOSM.POSMProducts,DailyAudit,DailyAudit.AllProducts,SurveyQuestions");
            var dailyCMActivity = new DailyCMActivity();
            dailyCMActivity = data.ToListAsync().Result.Find(e => e.Id == id);

            var mapper = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>();
                            cfg.CreateMap<SurveyReport, SurveyReporModel>();
                            cfg.CreateMap<DailyPOSM, DailyPOSMModel>();
                            cfg.CreateMap<POSMReport, POSMReportModel>();
                            cfg.CreateMap<DailyAudit, DailyAuditModel>();
                            cfg.CreateMap<AuditReport, AuditReportModel>();
                        }).CreateMapper();

            var model = mapper.Map<DailyCMActivityModel>(dailyCMActivity);
            if (model.DailyAudit != null)
            {
                model = MapAllProducts(model);
                model.DailyAudit.AllProducts.Clear();
            }

            return model;
        }

        private DailyCMActivityModel MapAllProducts(DailyCMActivityModel model)
        {
            foreach (var item in model.DailyAudit.AllProducts)
            {
                if (item.ActionType == ActionType.DistributionCheckProduct)
                {
                    model.DailyAudit.DistributionCheckProducts.Add(item);
                }
                else if (item.ActionType == ActionType.FacingCountProduct)
                {
                    model.DailyAudit.FacingCountProducts.Add(item);
                }
                else if (item.ActionType == ActionType.PlanogramCheckProduct)
                {
                    model.DailyAudit.PlanogramCheckProducts.Add(item);
                }
                else
                {
                    model.DailyAudit.PriceAuditProducts.Add(item);
                }
            }

            return model;
        }

        private DailyCMActivityModel MapPOSMProducts(DailyCMActivityModel model)
        {
            foreach (var item in model.DailyPOSM.POSMProducts)
            {
                if (item.ActionType == POSMActionType.Installation)
                {
                    model.DailyPOSM.POSMInstallationProducts.Add(item);
                }
                else if (item.ActionType == POSMActionType.Repair)
                {
                    model.DailyPOSM.POSMRepairProducts.Add(item);
                }
                else
                {
                    model.DailyPOSM.POSMRemovalProducts.Add(item);
                }
            }

            return model;
        }

        public async Task<List<DailyCMActivityModel>> mapSurveyReportQuestions(List<DailyCMActivityModel> model)
        {
            var questions = new List<SurveyQuestionCollectionModel>();

            if (model != null && model.Count != 0 &&
                (
                    model[0].Surveys.Count != 0 ||
                    model[0].ConsumerSurveys.Count != 0
                ))
            {
                var ids = new List<int>();
                foreach (var survey in model[0].Surveys)
                {
                    ids.Add(survey.Id);
                }
                foreach (var survey in model[0].ConsumerSurveys)
                {
                    ids.Add(survey.Id);
                }
                questions = await _survey.GetQuestionsBySurveyIdsAsync(ids);
            }

            foreach (var item in model)
            {
                if (item.Surveys.Count != 0)
                {
                    foreach (var survey in item.Surveys)
                    {
                        var ques = questions.FindAll(q => q.SurveyId == survey.Id);

                        foreach (var que in ques)
                        {
                            var queReportModel = new SurveyReporModel()
                            {
                                Id = 0,
                                DailyCMActivityId = 0,
                                QuestionId = que.QuestionId,
                                Answer = "",
                                SurveyId = que.SurveyId,
                                IsConsumerSurvey = false
                            };
                            survey.SurveyReportQuestions.Add(queReportModel);
                            item.SurveyQuestions.Add(queReportModel);
                        }
                    }
                }

                if (item.ConsumerSurveys.Count != 0)
                {
                    foreach (var survey in item.ConsumerSurveys)
                    {
                        var ques = questions.FindAll(q => q.SurveyId == survey.Id);

                        foreach (var que in ques)
                        {
                            var queReportModel = new SurveyReporModel()
                            {
                                Id = 0,
                                DailyCMActivityId = 0,
                                QuestionId = que.QuestionId,
                                Answer = "",
                                SurveyId = que.SurveyId,
                                IsConsumerSurvey = true
                            };
                            survey.SurveyReportQuestions.Add(queReportModel);
                            item.SurveyQuestions.Add(queReportModel);
                        }
                    }
                }

            }

            return model;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<List<DailyCMActivityModel>> GetFilteredCMTask(SearchDailyCMActivityModel searchModel)
        {
            //Check Workflow Availability

            var IsWorkflowAvailable = false;

            Expression<Func<DailyCMActivity, bool>> filter = null;

            searchModel ??= new SearchDailyCMActivityModel();

            if (searchModel.CMId != 0 && searchModel.Date != null)
            {
                filter = e => e.CMId == searchModel.CMId && e.Date.Date >= searchModel.Date && e.Status != Status.Completed;
            }
            else if (searchModel.AssignedFMUserId != 0 && searchModel.Date != null)
            {
                filter = e => e.AssignedFMUserId == searchModel.AssignedFMUserId && e.Date.Date >= searchModel.Date && e.Status != Status.Completed;
            }
            else if (searchModel.Date != null)
            {
                filter = e => e.Date.Date >= searchModel.Date && e.Status != Status.Completed;
            }
            var includeProperties = "DailyPOSM,DailyPOSM.POSMProducts,DailyPOSM.POSMProducts.Product," +
                                        "DailyAudit,DailyAudit.AllProducts,DailyAudit.AllProducts.Product," +
                                        "SurveyQuestions,SurveyQuestions.Survey," +
                                        "SurveyQuestions.Question,SurveyQuestions.Question.QuestionOptions";




            var data = _repo.GetAllIncludeStrFormat(filter, includeProperties: includeProperties);

            var cmactivityIds = string.Join(",", data.Select(a => a.Id));
            SqlParameter param = new SqlParameter("@ids", cmactivityIds);
            var outletList = await _repo.ExecuteQueryAsyc<OutletModel>("exec [dbo].[getOutLetbyDailyActivity]  @ids", param);
            //var outletList = result.ToMap<OutletModel, Outlet>();
            //await data.ForEachAsync(x => x.Outlet = outletList.FirstOrDefault(a => a.OutletId == x.OutletId));

            var mapper = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
                                .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
                            cfg.CreateMap<Outlet, OutletModel>();
                            cfg.CreateMap<SurveyReport, SurveyReporModel>();
                            cfg.CreateMap<SurveyQuestionSet, SurveyQuestionSetModel>();
                            cfg.CreateMap<Question, QuestionModel>();
                            cfg.CreateMap<QuestionOption, QuestionOptionModel>()
                                .ForMember(dest => dest.OptionEmoticon, opt => opt.MapFrom(src =>
                                    OptionEmoticonDict.Keys.Any(x => x == src.OptionTitle) ? OptionEmoticonDict[src.OptionTitle] : string.Empty
                                ));
                            cfg.CreateMap<DailyPOSM, DailyPOSMModel>();
                            cfg.CreateMap<POSMProduct, POSMProductModel>();
                            cfg.CreateMap<POSMReport, POSMReportModel>();
                            cfg.CreateMap<DailyAudit, DailyAuditModel>();
                            cfg.CreateMap<Product, ProductModel>();
                            cfg.CreateMap<AuditReport, AuditReportModel>();
                        }).CreateMapper();

            var dailyCMActivityListModel = mapper.Map<List<DailyCMActivityModel>>(data);

            foreach (var item in dailyCMActivityListModel)
            {
                item.Outlet = outletList.FirstOrDefault(a => a.OutletId == item.OutletId);
                if (item.Outlet != null)
                {
                    item.OutletName = item.Outlet.Name;
                    item.OutletAddress = item.Outlet.Address;
                    item.OutletOwnerName = item.Outlet.OwnerName;
                    item.OutletContactNumber = item.Outlet.ContactNumber;
                }
            }

            for (int i = 0; i < dailyCMActivityListModel.Count; i++)
            {
                if (dailyCMActivityListModel[i].DailyPOSM != null)
                {
                    dailyCMActivityListModel[i] = MapPOSMProducts(dailyCMActivityListModel[i]);
                    dailyCMActivityListModel[i].DailyPOSM?.POSMProducts.Clear();
                }
                if (dailyCMActivityListModel[i].DailyAudit != null)
                {
                    dailyCMActivityListModel[i] = MapAllProducts(dailyCMActivityListModel[i]);
                    dailyCMActivityListModel[i].DailyAudit?.AllProducts.Clear();
                }
                if (dailyCMActivityListModel[i].SurveyQuestions != null)
                {
                    var surveyIds = new List<int>();
                    foreach (var item in dailyCMActivityListModel[i].SurveyQuestions)
                    {
                        if (surveyIds.IndexOf(item.SurveyId) == -1)
                        {
                            surveyIds.Add(item.SurveyId);
                            var queList = dailyCMActivityListModel[i].SurveyQuestions.FindAll(q => q.SurveyId == item.SurveyId);

                            foreach (var que in queList)
                            {
                                que.Question.DailyCMActivityId = item.DailyCMActivityId;
                                que.Question.SurveyId = item.SurveyId;
                                que.Question.SurveyReportId = que.Id;


                                foreach (var queOption in que.Question.QuestionOptions)
                                {
                                    queOption.DailyCMActivityId = item.DailyCMActivityId;
                                    queOption.SurveyId = item.SurveyId;
                                    queOption.SurveyReportId = que.Id;
                                }
                            }
                            // item.Survey.IsConsumerSurvey = queList.Count != 0 ? queList[0].IsConsumerSurvey : false;
                            // item.Survey.SurveyReportQuestions = queList;                            
                            // dailyCMActivityListModel[i].Surveys.Add(item.Survey);

                            var tempSurvey = new SurveyQuestionSetModel();
                            tempSurvey.Id = queList[0].Survey.Id;
                            tempSurvey.Name = queList[0].Survey.Name;
                            tempSurvey.Status = queList[0].Survey.Status;
                            tempSurvey.DailyCMActivityId = item.DailyCMActivityId;
                            tempSurvey.IsConsumerSurvey = queList[0].IsConsumerSurvey;
                            tempSurvey.SurveyReportQuestions = queList;

                            foreach (var que in queList)
                            {
                                que.Survey = null;
                            }

                            // if (tempSurvey.IsConsumerSurvey == false)
                            // {
                            //     dailyCMActivityListModel[i].Surveys.Add(tempSurvey);
                            // }
                            // else
                            // {
                            //     dailyCMActivityListModel[i].ConsumerSurveys.Add(tempSurvey);
                            // }
                            dailyCMActivityListModel[i].AllSurveys.Add(tempSurvey);
                        }
                    }

                    dailyCMActivityListModel[i].SurveyQuestions.Clear();
                }
            }


            foreach (var item in dailyCMActivityListModel)
            {
                ChangeNullValue(item);
                ChangeNullValue(item.Outlet);
                if (item.DailyAudit != null)
                {
                    ChangeNullValue(item.DailyAudit);
                    foreach (var ap in item.DailyAudit.AllProducts)
                    {
                        ChangeNullValue(ap);
                        ChangeNullValue(ap.Product);
                    }
                    foreach (var ap in item.DailyAudit.DistributionCheckProducts)
                    {
                        ChangeNullValue(ap);
                        ChangeNullValue(ap.Product);
                    }
                    foreach (var ap in item.DailyAudit.FacingCountProducts)
                    {
                        ChangeNullValue(ap);
                        ChangeNullValue(ap.Product);
                    }
                    foreach (var ap in item.DailyAudit.PlanogramCheckProducts)
                    {
                        ChangeNullValue(ap);
                        ChangeNullValue(ap.POSMProduct);
                    }
                    foreach (var ap in item.DailyAudit.PriceAuditProducts)
                    {
                        ChangeNullValue(ap);
                        ChangeNullValue(ap.Product);
                    }

                    item.DailyAudit.DistributionCheckProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyAudit.FacingCountProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyAudit.PlanogramCheckProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyAudit.PriceAuditProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyAudit.AllProducts.ForEach(x => x.CMId = item.CMId);
                }

                if (item.DailyPOSM != null)
                {
                    ChangeNullValue(item.DailyPOSM);
                    foreach (var pp in item.DailyPOSM.POSMProducts)
                    {
                        ChangeNullValue(pp);
                        ChangeNullValue(pp.Product);
                    }
                    foreach (var pp in item.DailyPOSM.POSMInstallationProducts)
                    {
                        ChangeNullValue(pp);
                        ChangeNullValue(pp.Product);
                    }
                    foreach (var pp in item.DailyPOSM.POSMRemovalProducts)
                    {
                        ChangeNullValue(pp);
                        ChangeNullValue(pp.Product);
                    }
                    foreach (var pp in item.DailyPOSM.POSMRepairProducts)
                    {
                        ChangeNullValue(pp);
                        ChangeNullValue(pp.Product);
                    }

                    item.DailyPOSM.POSMInstallationProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyPOSM.POSMRepairProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyPOSM.POSMRemovalProducts.ForEach(x => x.CMId = item.CMId);
                    item.DailyPOSM.POSMProducts.ForEach(x => x.CMId = item.CMId);
                }

                foreach (var sq in item.SurveyQuestions)
                {
                    ChangeNullValue(sq);
                    ChangeNullValue(sq.Question);
                    ChangeNullValue(sq.Survey);

                    foreach (var qo in sq.Question.QuestionOptions)
                    {
                        ChangeNullValue(qo);
                    }

                    if (sq != null)
                    {
                        sq.CMId = item.CMId;
                    }
                }

                item.AllSurveys.ForEach(x => x.SurveyReportQuestions.ForEach(y => y.CMId = item.CMId));

            }

            return dailyCMActivityListModel;
        }

        private void ChangeNullValue(Object parameterObject)
        {
            if (parameterObject == null)
            {
                return;
            }


            Type type = parameterObject.GetType();
            foreach (var property in type.GetProperties())
            {

                if (property.PropertyType == typeof(string) && property.GetValue(parameterObject) == null)
                {
                    property.SetValue(parameterObject, string.Empty);

                }


            }

        }

        public async Task<IEnumerable<DailyCMActivityModel>> GetDailyCMActivityAsync()
        {
            var result = await _repo.GetAllAsync();
            return result.ToMap<DailyCMActivity, DailyCMActivityModel>();
        }

        public async Task<(IEnumerable<DailyCMActivitySPModel>, int total)> GetDailyCMActivitesByCurrentUserAsync(int pageIndex, int pageSize, string search)
        {
            #region LINQ
            //var appUserId = AppIdentity.AppUser.UserId;
            //var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            //var includeProperties = "Outlet,Outlet.Route,CM,AssignedFMUser,Outlet.SalesPoint";
            //var data = new List<DailyCMActivity>();
            //if (isAdmin)
            //{ s
            //    data = _repo.GetAllIncludeStrFormat(includeProperties: includeProperties).OrderByDescending(x => x.Date).ToList();
            //}
            //else
            //{
            //    var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            //    data = _repo.GetAllIncludeStrFormat(filter: x => userIds.Contains(x.AssignedFMUserId), includeProperties: includeProperties).OrderByDescending(x => x.Date).ToList();
            //}

            //var mapper = new MapperConfiguration(cfg =>
            //            {
            //                cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
            //                 .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
            //                cfg.CreateMap<Outlet, OutletModel>();
            //                cfg.CreateMap<SalesPoint, SalesPointModel>();
            //                cfg.CreateMap<CMUser, CMUserRegisterModel>();
            //                cfg.CreateMap<UserInfo, UserInfoModel>();
            //                cfg.CreateMap<Route, RouteModel>();
            //            }).CreateMapper();


            //var returnData = mapper.Map<IEnumerable<DailyCMActivityModel>>(data);

            //return returnData;
            #endregion

            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMATasks";

            //var parameters = isForExcel ? new List<(string, object, bool)>
            //{
            //    ("PageIndex", 0, false),
            //    ("PageSize", 5000 , false),
            //    ("SearchText", "" , false),
            //    ("OrderBy", "DCMA.Date desc", false),
            //    ("FMIds", fmUserIdsStr, false),
            //    ("TotalCount", 0, true),
            //    ("FilteredCount", 0, true)
            //} : new List<(string, object, bool)>
            //{
            //    ("PageIndex", pageIndex, false),
            //    ("PageSize", pageSize , false),
            //    ("SearchText", search , false),
            //    ("OrderBy", "DCMA.Date desc", false),
            //    ("FMIds", fmUserIdsStr, false),
            //    ("TotalCount", 0, true),
            //    ("FilteredCount", 0, true)
            //};
            var parameters = new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };
            var result = _repo.GetDataBySP<DailyCMActivitySPModel>(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<IEnumerable<DailyCMActivityModel>> GetDailyCMActivitesWithPOSMReportByCurrentUserAsync()
        {
            var appUserId = AppIdentity.AppUser.UserId;

            DateTime today = DateTime.Today;
            var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();

            var data = _repo.GetAllIncludeStrFormat(filter: m => userIds.Contains(m.AssignedFMUserId) && m.Date.Month == today.Month && m.Date.Year == today.Year, includeProperties: "DailyPOSM,DailyPOSM.POSMProducts");

            var mapper = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
                             .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
                            cfg.CreateMap<DailyPOSM, DailyPOSMModel>();
                            cfg.CreateMap<POSMReport, POSMReportModel>();
                        }).CreateMapper();


            var returnData = mapper.Map<IEnumerable<DailyCMActivityModel>>(data);

            return returnData;
        }

        public async Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesForDashboardByCurrentUserAsync(int pageIndex, int pageSize)
        {
            var appUserId = AppIdentity.AppUser.UserId;


            var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            string.Join(",", userIds.Select(n => n.ToString()).ToArray());
            string FMIdList = string.Join<int>(",", userIds);

            try
            {
                SqlParameter param1 = new SqlParameter("@FMIdList", FMIdList);
                SqlParameter param2 = new SqlParameter("@PageSize", pageSize);
                var result = await _repo.ExecuteQueryAsyc<DailyCMTaskReportModel>("exec [dbo].[sp_GetDailyActivityReportData]  @FMIdList, @PageSize", param1, param2);

                int TotalCount = 0;
                int CompletedCount = 0;

                foreach (var item in result)
                {
                    TotalCount = 0;
                    CompletedCount = 0;


                    if (item.TotalPOSM > 0)
                    {
                        var temp = (item.NumberOfRow / item.TotalPOSM);

                        if (temp > 0)
                        {
                            item.CompletedPOSM /= temp;
                        }
                    }

                    if (item.TotalAudit > 0)
                    {
                        var temp = (item.NumberOfRow / item.TotalAudit);

                        if (temp > 0)
                        {
                            item.CompletedAudit /= temp;
                        }
                    }

                    if (item.TotalSurvey > 0)
                    {
                        var temp = (item.NumberOfRow / item.TotalSurvey);

                        if (temp > 0)
                        {
                            item.CompletedSurvey /= temp;
                        }
                    }



                    item.DisplayDate = item.Date.ToString("dd/M/yyyy");
                    item.AuditPercentage = item.CompletedAudit.ToString() + '/' + item.TotalAudit.ToString();
                    item.POSMPercentage = item.CompletedPOSM.ToString() + '/' + item.TotalPOSM.ToString();
                    item.SurveyPercentage = item.CompletedSurvey.ToString() + '/' + item.TotalSurvey.ToString();

                    TotalCount = item.TotalAudit + item.TotalPOSM + item.TotalSurvey;
                    CompletedCount = item.CompletedAudit + item.CompletedPOSM + item.CompletedSurvey;

                    if (TotalCount > 0)
                    {
                        int percentage = (CompletedCount * 100) / TotalCount;
                        item.TotalPercentage = percentage.ToString() + '%';
                    }
                    else
                    {
                        item.TotalPercentage = "0" + '%';
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }




        }

        public async Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesReportsByCurrentUserAsync(int pageIndex, int pageSize)
        {
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            #region Store Procedure
            //var storeProcedure = "spGetDCMAReports";
            //var parameters = new List<(string, object, bool)>
            //{
            //    ("PageIndex", pageIndex, false),
            //    ("PageSize", pageSize , false),
            //    ("SearchText", "" , false),
            //    ("OrderBy", "DCMA.Date desc", false),
            //    ("FMIds", fmUserIdsStr, false),
            //    ("TotalCount", 0, true),
            //    ("FilteredCount", 0, true)
            //};

            //var result = _repo.GetDataBySP<DailyCMTaskReportModel>(storeProcedure, parameters);

            SqlParameter[] param = BuildReportParameter(fmUserIdsStr);
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            var result = await _repo.ExecuteQueryAsyc<DailyCMTaskReportModel>("exec sp_updatestats; exec [dbo].[spGetDCMAReports]  @PageIndex, @PageSize, @SearchText, @OrderBy, @FMIds, @TotalCount OUTPUT, @FilteredCount OUTPUT", param);

            //var time = watch.ElapsedMilliseconds;
            //watch.Stop();

            return result.ToList();
            #endregion
        }



        public async Task<DailyCMActivityModel> UpdateStatusAsync(DailyCMActivityModel model)
        {
            var data = await _repo.FindAsync(a => a.Id == model.Id);
            data.Status = model.Status;
            var res = await _repo.UpdateAsync(data);
            return res.ToMap<DailyCMActivity, DailyCMActivityModel>();
        }

        public async Task<IEnumerable<DailyCMActivityModel>> UpdateBatchStatusAsync(BatchStatusChangeModel model)
        {
            var data = await _repo.FindAllAsync(a => model.CMIdList.Contains(a.CMId) && a.Date == model.Date);

            foreach (var item in data)
            {
                item.Status = model.Status;
            }

            var res = await _repo.UpdateListAsync(data.ToList());
            return res.ToMap<DailyCMActivity, DailyCMActivityModel>();

        }


        public async Task<(IEnumerable<POSMReportSPModel>, int total)> GetPOSMReportsByCurrentUserAsync(int pageIndex, int pageSize, string search)
        {
            #region LINQ
            //var appUserId = AppIdentity.AppUser.UserId;
            //var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            //var data = new List<POSMReport>();
            //if (isAdmin)
            //{
            //    data = (await _posmReport.GetAllIncludeAsync(x => x, null,
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        .Include("DailyCMActivity.Outlet.SalesPoint")
            //                        .Include(y => y.Product),
            //                true)).ToList();
            //}
            //else
            //{
            //    var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            //    data = (await _posmReport.GetAllIncludeAsync(x => x,
            //                x => userIds.Contains(x.DailyCMActivity.AssignedFMUserId),
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        //.Include(y => y.DailyCMActivity).ThenInclude(y => y.Outlet).ThenInclude(y => y.SalesPoint)
            //                        .Include(y => y.Product),
            //                true)).ToList();
            //}

            ////var data = _posmReport.GetAllIncludeStrFormat(includeProperties:
            ////                             "Product,"+
            ////                             "DailyCMActivity,DailyCMActivity.AssignedFMUser,DailyCMActivity.CM,DailyCMActivity.Outlet,DailyCMActivity.Outlet.SalesPoint"
            ////                             ).OrderByDescending(a => a.CreatedTime);



            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<POSMReport, POSMReportModel>();
            //    cfg.CreateMap<POSMProduct, POSMProductModel>();
            //    cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
            //    .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
            //    cfg.CreateMap<Outlet, OutletModel>();
            //    cfg.CreateMap<SalesPoint, SalesPointModel>();
            //    cfg.CreateMap<CMUser, CMUserRegisterModel>();
            //    cfg.CreateMap<UserInfo, UserInfoModel>();

            //}).CreateMapper();

            //return mapper.Map<IEnumerable<POSMReportModel>>(data);
            #endregion

            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }
            SqlParameter[] param = BuildReportParameter(fmUserIdsStr);
            var storeProcedure = "spGetPOSMReports";
            var parameters = new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            var result = _repo.GetDataBySP<POSMReportSPModel>(storeProcedure, parameters);
            //var result = await _repo.ExecuteQueryAsyc<POSMReportSPModel>("exec [dbo].[spGetPOSMReports]  @PageIndex, @PageSize, @SearchText, @OrderBy, @FMIds, @TotalCount OUTPUT, @FilteredCount OUTPUT", param);

            //var time = watch.ElapsedMilliseconds;
            //watch.Stop();

            //var storeProcedure = "spGetPOSMReports";
            //var parameters = new List<(string, object, bool)>
            //{
            //     ("PageIndex", 0, false),
            //     ("PageSize", 5000 , false),
            //     ("SearchText", "" , false),
            //     ("OrderBy", "DCMA.Date desc", false),
            //     ("FMIds", fmUserIdsStr, false),
            //     ("TotalCount", 0, true),
            //     ("FilteredCount", 0, true)
            //};

            //var result = _repo.GetDataBySP<POSMReportSPModel>(storeProcedure, parameters);

            return (result.Items, result.Total);

            #endregion
        }

        private static SqlParameter[] BuildReportParameter(string fmUserIdsStr)
        {
            int val = 0;
            SqlParameter[] param = {
                new SqlParameter("@PageIndex", val),
                new SqlParameter("@PageSize", 5000),
                new SqlParameter("@SearchText", "" ),
                new SqlParameter("@OrderBy", "DCMA.Date desc"),
                new SqlParameter("@FMIds", fmUserIdsStr),
                new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                },
                 new SqlParameter
                {
                    ParameterName = "@FilteredCount",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                },
                //new SqlParameter("@FilteredCount", val)
            };
            return param;
        }

        public async Task<(IEnumerable<AuditReportSPModel>, int total)> GetAuditReportsByCurrentUserAsync(int pageIndex, int pageSize, string search)
        {
            #region LINQ
            //var appUserId = AppIdentity.AppUser.UserId;
            //var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            //var data = new List<AuditReport>();
            //if (isAdmin)
            //{
            //    data = (await _auditReport.GetAllIncludeAsync(x => x, null,
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        //.Include(y => y.DailyCMActivity).ThenInclude(y => y.Outlet).ThenInclude(y => y.SalesPoint)
            //                        .Include(y => y.Product).Include(y => y.POSMProduct),
            //                true)).ToList();
            //}
            //else
            //{
            //    var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            //    data = (await _auditReport.GetAllIncludeAsync(x => x,
            //                x => userIds.Contains(x.DailyCMActivity.AssignedFMUserId),
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        //.Include(y => y.DailyCMActivity).ThenInclude(y => y.Outlet).ThenInclude(y => y.SalesPoint)
            //                        .Include(y => y.Product).Include(y => y.POSMProduct),
            //                true)).ToList();
            //}


            ////var data = _auditReport.GetAllIncludeStrFormat(includeProperties:
            ////                            "Product,POSMProduct,"+
            ////                            "DailyCMActivity,DailyCMActivity.AssignedFMUser,DailyCMActivity.CM,DailyCMActivity.Outlet,DailyCMActivity.Outlet.SalesPoint"
            ////                             ).OrderByDescending(a => a.CreatedTime);

            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<AuditReport, AuditReportModel>()
            //    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ActionType == ActionType.PlanogramCheckProduct ? src.POSMProduct.Name : src.Product.Name));
            //    cfg.CreateMap<POSMProduct, POSMProductModel>();
            //    cfg.CreateMap<Product, ProductModel>();
            //    cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
            //    .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
            //    cfg.CreateMap<Outlet, OutletModel>();
            //    cfg.CreateMap<SalesPoint, SalesPointModel>();
            //    cfg.CreateMap<CMUser, CMUserRegisterModel>();
            //    cfg.CreateMap<UserInfo, UserInfoModel>();

            //}).CreateMapper();
            //return mapper.Map<IEnumerable<AuditReportModel>>(data);
            #endregion

            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }
            var storeProcedure = "spGetAuditReports";
            var parameters = new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };
            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            SqlParameter[] param = BuildReportParameter(fmUserIdsStr);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // var result = await _repo.ExecuteQueryAsyc<AuditReportSPModel>("exec [dbo].[spGetAuditReports]  @PageIndex, @PageSize, @SearchText, @OrderBy, @FMIds, @TotalCount OUTPUT, @FilteredCount OUTPUT", param);
            var result = _repo.GetDataBySP<AuditReportSPModel>(storeProcedure, parameters);
            var time = watch.ElapsedMilliseconds / 60;
            watch.Stop();

            //var storeProcedure = "spGetAuditReports";
            //var parameters = new List<(string, object, bool)>
            //{
            //    ("PageIndex", 0, false),
            //    ("PageSize", 5000 , false),
            //    ("SearchText", "" , false),
            //    ("OrderBy", "DCMA.Date desc", false),
            //    ("FMIds", fmUserIdsStr, false),
            //    ("TotalCount", 0, true),
            //    ("FilteredCount", 0, true)
            //};

            //var result = _repo.GetDataBySP<AuditReportSPModel>(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<SurveyReportSPModel>, int total)> GetSurveyReportsByCurrentUserAsync(int pageIndex, int pageSize, string search)
        {
            #region LINQ
            //var appUserId = AppIdentity.AppUser.UserId;
            //var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";

            //var data = new List<SurveyReport>();
            //if (isAdmin)
            //{
            //    data = (await _surveyReport.GetAllIncludeAsync(x => x, null,
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.Outlet).ThenInclude(y => y.SalesPoint)
            //                        .Include(y => y.Question).Include(y => y.Survey),
            //                true)).ToList();
            //}
            //else
            //{
            //    var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
            //    data = (await _surveyReport.GetAllIncludeAsync(x => x,
            //                x => userIds.Contains(x.DailyCMActivity.AssignedFMUserId),
            //                x => x.OrderByDescending(o => o.DailyCMActivity.Date),
            //                x => x.Include(y => y.DailyCMActivity).ThenInclude(y => y.AssignedFMUser)
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.CM)
            //                        .Include("DailyCMActivity.Outlet")
            //                        .Include(y => y.DailyCMActivity).ThenInclude(y => y.Outlet).ThenInclude(y => y.SalesPoint)
            //                        .Include(y => y.Question).Include(y => y.Survey),
            //                true)).ToList();
            //}

            ////var data = _surveyReport.GetAllIncludeStrFormat(includeProperties:
            ////                            "Question,Survey,"+
            ////                            "DailyCMActivity,DailyCMActivity.AssignedFMUser,DailyCMActivity.CM,DailyCMActivity.Outlet,DailyCMActivity.Outlet.SalesPoint"
            ////                             ).OrderByDescending(a => a.CreatedTime);



            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<SurveyReport, SurveyReporModel>();
            //    cfg.CreateMap<Question, QuestionModel>();
            //    cfg.CreateMap<Survey, SurveyModel>();
            //    cfg.CreateMap<DailyCMActivity, DailyCMActivityModel>()
            //    .ForMember(dest => dest.DateStr, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
            //    cfg.CreateMap<Outlet, OutletModel>();
            //    cfg.CreateMap<SalesPoint, SalesPointModel>();
            //    cfg.CreateMap<CMUser, CMUserRegisterModel>();
            //    cfg.CreateMap<UserInfo, UserInfoModel>();
            //}).CreateMapper();

            //return mapper.Map<IEnumerable<SurveyReporModel>>(data);
            #endregion

            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }
            var storeProcedure = "spGetSurveyReports";
            var parameters = new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };
            SqlParameter[] param = BuildReportParameter(fmUserIdsStr);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //   var result = await _repo.ExecuteQueryAsyc<SurveyReportSPModel>("exec [dbo].[spGetSurveyReports]  @PageIndex, @PageSize, @SearchText, @OrderBy, @FMIds, @TotalCount OUTPUT, @FilteredCount OUTPUT", param);
            var result = _repo.GetDataBySP<SurveyReportSPModel>(storeProcedure, parameters);
            var time = watch.ElapsedMilliseconds / 60;
            watch.Stop();

            //var storeProcedure = "spGetSurveyReports";
            //var parameters = new List<(string, object, bool)>
            //{
            //    ("PageIndex", 0, false),
            //    ("PageSize", 5000 , false),
            //    ("SearchText", "" , false),
            //    ("OrderBy", "DCMA.Date desc", false),
            //    ("FMIds", fmUserIdsStr, false),
            //    ("TotalCount", 0, true),
            //    ("FilteredCount", 0, true)
            //};

            //var result = _repo.GetDataBySP<SurveyReportSPModel>(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<object>, int total)> GetDCMAReportsInDetailsByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll)
        {
            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMAReportsInDetails";
            var parameters = isAll ? new List<(string, object, bool)>
            {
                ("PageIndex", 0, false),
                ("PageSize", 5000 , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            } : new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

            var result = _repo.DynamicListFromSql(storeProcedure, parameters);
            //var result = _repo.GetDataBySP<dynamic>(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<object>, int total)> GetDCMAReportsSalesPointWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll)
        {
            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMAReportsSalesPointWise";
            var parameters = isAll ? new List<(string, object, bool)>
            {
                ("PageIndex", 0, false),
                ("PageSize", 5000 , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            } : new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

            var result = _repo.DynamicListFromSql(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<object>, int total)> GetDCMAReportsTerritoryWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll)
        {
            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMAReportsTerritoryWise";
            var parameters = isAll ? new List<(string, object, bool)>
            {
                ("PageIndex", 0, false),
                ("PageSize", 5000 , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            } : new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

            var result = _repo.DynamicListFromSql(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<object>, int total)> GetDCMAReportsAreaWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll)
        {
            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMAReportsAreaWise";
            var parameters = isAll ? new List<(string, object, bool)>
            {
                ("PageIndex", 0, false),
                ("PageSize", 5000 , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            } : new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

            var result = _repo.DynamicListFromSql(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public async Task<(IEnumerable<object>, int total)> GetDCMAReportsRegionWiseByCurrentUserAsync(int pageIndex, int pageSize, string search, bool isAll)
        {
            #region Store Procedure
            var appUserId = AppIdentity.AppUser.UserId;
            var isAdmin = AppIdentity.AppUser.ActiveRoleName == "Admin";
            var fmUserIdsStr = "-1";
            var fmUserIds = new List<int>();

            if (!isAdmin)
            {
                var userIds = _repo.GetNodeWiseUsersByUserId(appUserId).Select(x => x.Id).ToList();
                fmUserIdsStr = string.Join<int>(",", userIds);
            }

            var storeProcedure = "spGetDCMAReportsRegionWise";
            var parameters = isAll ? new List<(string, object, bool)>
            {
                ("PageIndex", 0, false),
                ("PageSize", 5000 , false),
                ("SearchText", "" , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            } : new List<(string, object, bool)>
            {
                ("PageIndex", pageIndex, false),
                ("PageSize", pageSize , false),
                ("SearchText", search , false),
                ("OrderBy", "DCMA.Date desc", false),
                ("FMIds", fmUserIdsStr, false),
                ("TotalCount", 0, true),
                ("FilteredCount", 0, true)
            };

            var result = _repo.DynamicListFromSql(storeProcedure, parameters);

            return (result.Items.ToList(), result.Total);
            #endregion
        }

        public Task<List<DailyCMActivityModel>> GetDailyCMActivitesByCMUserAsync(int userId)
        {
            List<DailyCMActivity> activities = _repo.FindAll(x => x.CMId == userId && x.Date.Date == DateTime.Today).ToList();
            List<DailyCMActivityModel> result = activities.ToMap<DailyCMActivity, DailyCMActivityModel>();
            List<int> activityIds = result.Select(x => x.Id).ToList();
            var posms = _dailyPosm.FindAll(x => activityIds.Any(r => r == x.DailyCMActivityId)).ToList();
            List<DailyPOSMModel> posmModels = posms.ToMap<DailyPOSM, DailyPOSMModel>();

            var audits = _dailyAudit.FindAll(x => activityIds.Any(r => r == x.DailyCMActivityId)).ToList();
            List<DailyAuditModel> auditModels = audits.ToMap<DailyAudit, DailyAuditModel>();

            foreach (var r in result)
            {
                r.DailyPOSM = posmModels.Find(x => x.DailyCMActivityId == r.Id);
                r.DailyAudit = auditModels.Find(x => x.DailyCMActivityId == r.Id);
            }
            return Task.FromResult(result);
        }

        public FileData DownloadExcelFormatOfTask()
        {
            TaskCreationData taskCreationData = GetTaskCreationData();

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            SetTableStyle(workSheet, taskCreationData.Headers.Count);
            SetHeaderStyle(workSheet);
            InsertHeaders(taskCreationData.Headers, workSheet);
            InsertRows(taskCreationData.Dates, taskCreationData.CMUserIds, taskCreationData.CMUserNames, workSheet);
            AutoFitColumns(taskCreationData.Headers, workSheet);

            FileData fileData = GetFileData(excel);
            return fileData;
        }

        public async Task<DCMAReportsInDetailsResponse> GetDCMAReportsInDetails2(int pageIndex, int pageSize, string search)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var query = _dailyPosmTask.GetAllActive().Where(x => x.DailyTask.IsSubmitted &&
                                                                 salesPointIds.Contains(x.DailyTask.SalesPointId) && x.DailyPosmTaskItems.Any(x=>x.ExecutionType == PosmWorkType.Installation || x.ExecutionType == PosmWorkType.RemovalAndReInstallation));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.DailyTask.SalesPointId));
            }
            query = query.Include(x => x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser)
                .Include(x => x.Reason);

            var result = await query.OrderByDescending(x => x.DailyTask.DateTime).ToPagedListAsync(pageIndex,pageSize);
            var resultList = result.ToList();

            
            var dailyTasks = resultList.Select(x => x.DailyTask).DistinctBy(x=>x.Id).MapToModel();
            foreach (var dailyTask in dailyTasks)
            {
                var posmTasks = resultList.Where(x => x.DailyTaskId == dailyTask.Id).MapToModel();
                posmTasks.ForEach(x => x.DailyPosmTaskItems = x.DailyPosmTaskItems.Where(x =>
                    x.ExecutionType == PosmWorkType.Installation || x.ExecutionType == PosmWorkType.RemovalAndReInstallation).ToList());
                dailyTask.DailyPosmTasks = posmTasks;
            }
            _common.InsertSalesPoints(dailyTasks);
            _common.InsertOutlets(dailyTasks.SelectMany(x=>x.DailyPosmTasks).ToList());
            


            DCMAReportsInDetailsResponse dcmaReportsInDetailsResponse = new DCMAReportsInDetailsResponse()
            {
                Item1 = new List<Dictionary<string, dynamic>>(),
                Item2 = result.TotalItemCount
            };


            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIds);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();
            foreach (var dailyTask in dailyTasks)
            {
                foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                {
                    var dict = new Dictionary<string, dynamic>();
                    var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == dailyTask.SalesPointId)?.NodeId;
                    var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                    var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                    var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);


                    dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), DateTime.Parse(dailyTask.DateTimeStr).ToBangladeshTime().ToDisplayString());
                    dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), teritory?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.SalesPoint.GetDescription(), dailyTask.SalesPoint?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.CmrName.GetDescription(), dailyTask.CmUser?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.OutletCode.GetDescription(), dailyPosmTask.Outlet?.Code);
                    dict.Add(DCMAReportsInDetailsExceColumn.OutletName.GetDescription(), dailyPosmTask.Outlet?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription(), dailyPosmTask.Id);
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(),dailyPosmTask.DailyPosmTaskItems.Select(x=>x.Quantity).Sum());

                    posmNames.ForEach(x => dict.Add(x, 0));


                    foreach (var dailyPosmTaskItem in dailyPosmTask.DailyPosmTaskItems)
                    {
                        dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
                    }

                    dcmaReportsInDetailsResponse.Item1.Add(dict);
                }
            }

            return dcmaReportsInDetailsResponse;

        }

        public async Task<DCMAReportsInDetailsResponse> GetDCMAReportsSalesPointWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var workTypes = new List<PosmWorkType>()
                {PosmWorkType.Installation, PosmWorkType.RemovalAndReInstallation};

            var query = _dailyTask.GetAllActive().Where(x => x.IsSubmitted && 
                     salesPointIds.Contains(x.SalesPointId) && x.DailyPosmTasks.Any(dailyPosmTask =>
                                  dailyPosmTask.DailyPosmTaskItems.Any(item => workTypes.Contains(item.ExecutionType))));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId));
            }
            query = query.Include(x => x.DailyPosmTasks)
                .ThenInclude(x=>x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.CmUser)
                .Include(x=>x.DailyPosmTasks)
                .ThenInclude(x => x.Reason);

            var result = await query.OrderByDescending(x => x.DateTime).ToPagedListAsync(pageIndex, pageSize);
            var resultList = result.ToList();

            var dailyTasks = resultList.MapToModel();
            foreach (var dailyTask in dailyTasks)
            {
                var posmTaks = dailyTask.DailyPosmTasks
                    .Where(x => x.DailyPosmTaskItems.Any(item => workTypes.Contains(item.ExecutionType))).ToList();
                posmTaks.ForEach(x =>
                    x.DailyPosmTaskItems = x.DailyPosmTaskItems.Where(item => workTypes.Contains(item.ExecutionType))
                        .ToList());
                dailyTask.DailyPosmTasks = posmTaks;
            }
            _common.InsertSalesPoints(dailyTasks);
            _common.InsertOutlets(dailyTasks.SelectMany(x => x.DailyPosmTasks).ToList());

            DCMAReportsInDetailsResponse dcmaReportsInDetailsResponse = new DCMAReportsInDetailsResponse()
            {
                Item1 = new List<Dictionary<string, dynamic>>(),
                Item2 = result.TotalItemCount
            };


            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIds);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var dailyTask in dailyTasks)
            {
                foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                {
                    var dict = new Dictionary<string, dynamic>();
                    var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == dailyTask.SalesPointId)?.NodeId;
                    var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                    var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                    var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);


                    dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), DateTime.Parse(dailyTask.DateTimeStr).ToBangladeshTime().ToDisplayString());
                    dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.SalesPoint.GetDescription(), dailyTask.SalesPoint?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.CmrName.GetDescription(), dailyTask.CmUser?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription(), dailyPosmTask.Id);
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum());
                    posmNames.ForEach(x => dict.Add(x, 0));

                    foreach (var dailyPosmTaskItem in dailyPosmTask.DailyPosmTaskItems)
                    {
                        dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
                    }

                    dcmaReportsInDetailsResponse.Item1.Add(dict);
                }
            }

            return dcmaReportsInDetailsResponse;
        }

        public async Task<DCMAReportsInDetailsResponse> GetDCMAReportsTerritoryWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var workTypes = new List<PosmWorkType>()
                {PosmWorkType.Installation, PosmWorkType.RemovalAndReInstallation};

            var query = _dailyTask.GetAllActive().Where(x => x.IsSubmitted &&
                     salesPointIds.Contains(x.SalesPointId) && x.DailyPosmTasks.Any(dailyPosmTask =>
                                  dailyPosmTask.DailyPosmTaskItems.Any(item => workTypes.Contains(item.ExecutionType))));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId));
            }
            query = query.Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.CmUser)
                .Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.Reason);



            var result = await query.OrderByDescending(x => x.DateTime).ToListAsync();

            var salesPointIdsOfResult = result.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var totalCount = 0;

            var groupedByTerritory = result.Select(x =>
            {
                var teritoryId = salesPointNodeMapps.FirstOrDefault(g => x.SalesPointId == g.SalesPointId)?.NodeId;

                return new
                {
                    dTask = x,
                    territoryId = $"{teritoryId ?? 0}_{x.DateTime.ToBangladeshTime().Date.ToDisplayString()}"
                };
            }).GroupBy(x=>x.territoryId);

            totalCount = groupedByTerritory.Count();
            groupedByTerritory = groupedByTerritory.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            DCMAReportsInDetailsResponse dcmaReportsInDetailsResponse = new DCMAReportsInDetailsResponse()
            {
                Item1 = new List<Dictionary<string, dynamic>>(),
                Item2 = totalCount
            };

            var posmNames = groupedByTerritory.SelectMany(x => x).Select(x => x.dTask).SelectMany(x => x.DailyPosmTasks)
                .SelectMany(x => x.DailyPosmTaskItems).Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var group in groupedByTerritory)
            {
                var dailyTasksTemp = group.ToList().Select(x=>x.dTask).ToList();

                var firstItem = dailyTasksTemp[0];
                if(firstItem == null) continue;
                var dict = new Dictionary<string, dynamic>();


                var date = firstItem.DateTime.ToIsoString();


                var territoryId = salesPointNodeMapps.FirstOrDefault(x=> x.SalesPointId == firstItem.SalesPointId)?.NodeId;

                if(territoryId is null) continue;

                var teritory = parentNodes.FirstOrDefault(x => x.NodeId == territoryId);
                var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);


                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);

                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), DateTime.Parse(date).ToBangladeshTime().ToDisplayString());

                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), 0);

                posmNames.ForEach(x => dict.Add(x, 0));

                dict.Add(DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription(), firstItem.Id);

                foreach (var dailyTask in dailyTasksTemp)
                {

                        foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                        {

                            dict[DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription()] += dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum();

                            foreach (var dailyPosmTaskItem in dailyPosmTask.DailyPosmTaskItems)
                            {
                                if (dict.Keys.Contains(dailyPosmTaskItem.PosmProduct.Name))
                                {
                                    dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
                                }
                            }

                        }

                        dcmaReportsInDetailsResponse.Item1.Add(dict);

                }

            }

            dcmaReportsInDetailsResponse.Item1 = dcmaReportsInDetailsResponse.Item1
                .OrderByDescending(x => x[DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription()]).ToList();

            return dcmaReportsInDetailsResponse;
        }

        public async Task<DCMAReportsInDetailsResponse> GetDCMAReportsAreaWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var workTypes = new List<PosmWorkType>()
                {PosmWorkType.Installation, PosmWorkType.RemovalAndReInstallation};

            var query = _dailyTask.GetAllActive().Where(x => x.IsSubmitted &&
                     salesPointIds.Contains(x.SalesPointId) && x.DailyPosmTasks.Any(dailyPosmTask =>
                                  dailyPosmTask.DailyPosmTaskItems.Any(item => workTypes.Contains(item.ExecutionType))));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId));
            }
            query = query.Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.CmUser)
                .Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.Reason);



            var result = await query.OrderByDescending(x => x.DateTime).ToListAsync();

            var salesPointIdsOfResult = result.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var totalCount = 0;

            var groupedByArea = result.Select(x =>
            {
                var teritoryId = salesPointNodeMapps.FirstOrDefault(g => x.SalesPointId == g.SalesPointId)?.NodeId;
                var areaId = parentNodes.FirstOrDefault(ar => ar.NodeId == teritoryId)?.ParentId;

                return new
                {
                    dTask = x,
                    key = $"{areaId ?? 0}{x.DateTime.ToBangladeshTime().Date.ToDisplayString()}"
                };
            }).GroupBy(x => x.key);

            totalCount = groupedByArea.Count();
            groupedByArea = groupedByArea.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            DCMAReportsInDetailsResponse dcmaReportsInDetailsResponse = new DCMAReportsInDetailsResponse()
            {
                Item1 = new List<Dictionary<string, dynamic>>(),
                Item2 = totalCount
            };

            var posmNames = groupedByArea.SelectMany(x => x).Select(x => x.dTask).SelectMany(x => x.DailyPosmTasks)
                .SelectMany(x => x.DailyPosmTaskItems).Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var group in groupedByArea)
            {
                    var dailyTasksTemp = group.ToList().Select(x => x.dTask).ToList();
                    var firtItem = dailyTasksTemp[0];
                    if(firtItem is null) continue;

                    var dict = new Dictionary<string, dynamic>();
                    var achievement = 0;

                    var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == firtItem.SalesPointId)
                        ?.NodeId;
                    var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                    var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                    var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);

                    dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);

                    dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), DateTime.Parse(firtItem.DateTimeStr).ToBangladeshTime().ToDisplayString());
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), achievement);

                    posmNames.ForEach(x => dict.Add(x, 0));

                    dict.Add(DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription(), firtItem.Id);

                    foreach (var dailyTask in dailyTasksTemp)
                    {
                        foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                        {

                            dict[DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription()] += dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum();

                            foreach (var dailyPosmTaskItem in dailyPosmTask.DailyPosmTaskItems)
                            {
                                if (dict.Keys.Contains(dailyPosmTaskItem.PosmProduct.Name))
                                {
                                    dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
                                }
                            }

                        }

                    }

                    dcmaReportsInDetailsResponse.Item1.Add(dict);

            }

            dcmaReportsInDetailsResponse.Item1 = dcmaReportsInDetailsResponse.Item1
                .OrderByDescending(x => x[DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription()]).ToList();

            return dcmaReportsInDetailsResponse;
        }

        public async Task<DCMAReportsInDetailsResponse> GetDCMAReportsRegionWiseByCurrentUserAsync2(int pageIndex, int pageSize, string search)
        {
            var user = AppIdentity.AppUser;
            var salesPoints = _common.GetSalesPointsByFMUser(user.UserId);
            var salesPointIds = salesPoints.Select(x => x.SalesPointId).ToList();

            var workTypes = new List<PosmWorkType>()
                {PosmWorkType.Installation, PosmWorkType.RemovalAndReInstallation};

            var query = _dailyTask.GetAllActive().Where(x => x.IsSubmitted &&
                     salesPointIds.Contains(x.SalesPointId) && x.DailyPosmTasks.Any(dailyPosmTask =>
                                  dailyPosmTask.DailyPosmTaskItems.Any(item => workTypes.Contains(item.ExecutionType))));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Name.Contains(search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId));
            }
            query = query.Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.CmUser)
                .Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.Reason);



            var result = await query.OrderByDescending(x => x.DateTime).ToListAsync();

            var salesPointIdsOfResult = result.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var totalCount = 0;

            var groupedByRegion = result.Select(x =>
            {
                var teritoryId = salesPointNodeMapps.FirstOrDefault(g => x.SalesPointId == g.SalesPointId)?.NodeId;
                var areaId = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId)?.ParentId;
                var regionId = parentNodes.FirstOrDefault(x => x.NodeId == areaId)?.ParentId;

                return new
                {
                    dTask = x,
                    regionId = regionId,
                    key = $"{regionId ?? 0}_{x.DateTime.ToBangladeshTime().Date.ToDisplayString()}"
                };
            }).GroupBy(x => x.key);

            totalCount = groupedByRegion.Count();
            groupedByRegion = groupedByRegion.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            DCMAReportsInDetailsResponse dcmaReportsInDetailsResponse = new DCMAReportsInDetailsResponse()
            {
                Item1 = new List<Dictionary<string, dynamic>>(),
                Item2 = totalCount
            };

            var posmNames = groupedByRegion.SelectMany(x => x).Select(x => x.dTask).SelectMany(x => x.DailyPosmTasks)
                .SelectMany(x => x.DailyPosmTaskItems).Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var group in groupedByRegion)
            {
                var dailyTasksTemp = group.ToList().Select(x => x.dTask).ToList();
                var regionId = group.First().regionId;

                if(regionId is null) continue;

                var dict = new Dictionary<string, dynamic>();
                var achievement = 0;
                var firstItem = dailyTasksTemp.First();

                var region = parentNodes.FirstOrDefault(x => x.NodeId == regionId);

                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);

                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), DateTime.Parse(firstItem.DateTimeStr).ToBangladeshTime().ToDisplayString());

                posmNames.ForEach(x => dict.Add(x, 0));

                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), achievement);

                foreach (var dailyTask in dailyTasksTemp)
                {
                    dict.Add(DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription(), dailyTask.Id);

                    foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                    {
                        achievement += dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum();

                        foreach (var dailyPosmTaskItem in dailyPosmTask.DailyPosmTaskItems)
                        {
                            dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
                        }

                    }
                    dict[DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription()] += achievement;

                }

                dcmaReportsInDetailsResponse.Item1.Add(dict);

            }

            dcmaReportsInDetailsResponse.Item1 = dcmaReportsInDetailsResponse.Item1
                .OrderByDescending(x => x[DCMAReportsInDetailsExceColumn.DailyCMActivityId.GetDescription()]).ToList();

            return dcmaReportsInDetailsResponse;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportsSpWise(GetExecutionReport payload)
        {
         


            var dailyTasks = await GetDailyTasksAll(payload);// resultList.Select(x => x.DailyTask).DistinctBy(x => x.Id).MapToModel();
            _common.InsertSalesPoints(dailyTasks);
            _common.InsertOutlets(dailyTasks.SelectMany(x => x.DailyPosmTasks).ToList());

            var result = new List<Dictionary<string, dynamic>>();

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var dailyTask in dailyTasks)
            {
                foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                {
                    var dict = new Dictionary<string, dynamic>();
                    var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == dailyTask.SalesPointId)?.NodeId;
                    var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                    var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory?.ParentId);
                    var region = parentNodes.FirstOrDefault(x => x.NodeId == area?.ParentId);
                   

                    dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), dailyTask.DateTimeStr);
                    dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.SalesPoint.GetDescription(), dailyTask.SalesPoint?.Name);

                    dict.Add(DCMAReportsInDetailsExceColumn.CmrName.GetDescription(), dailyTask.CmUser?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.CmrCode.GetDescription(), dailyTask.CmUser?.Code);
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum());

                    var tcmTotal = dailyPosmTask.DailyPosmTaskItems
                        .Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                    dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                    var pcmTotal= dailyPosmTask.DailyPosmTaskItems
                        .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                    dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                    posmNames.ForEach(x => dict.Add(x, 0));

                    AddPosms(dailyPosmTask.DailyPosmTaskItems, dict);

                    result.Add(dict);
                }
            }

            return result;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportTeritoryWise(GetExecutionReport payload)
        {

            var result = new List<Dictionary<string, dynamic>>();

            var dailyTasks = await GetDailyTasksAll(payload);

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var dailyTask in dailyTasks)
            {
                var dailyPosmTaskItems = dailyTask.DailyPosmTasks.SelectMany(x => x.DailyPosmTaskItems).ToList();

                var dict = new Dictionary<string, dynamic>();
                var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == dailyTask.SalesPointId)?.NodeId;
                var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);


                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), dailyTask.DateTimeStr);
                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.SalesPoint.GetDescription(), dailyTask.SalesPoint?.Name);

                dict.Add(DCMAReportsInDetailsExceColumn.CmrCount.GetDescription(), dailyTask.DailyPosmTasks.Count);

                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTaskItems.Select(x => x.Quantity).Sum());

                var tcmTotal = dailyPosmTaskItems.Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                var pcmTotal = dailyPosmTaskItems
                    .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                
                posmNames.ForEach(x => dict.Add(x, 0));

                AddPosms(dailyPosmTaskItems, dict);

                result.Add(dict);
            }

            return result;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportAreaWise(GetExecutionReport payload)
        {
            
            var result = new List<Dictionary<string, dynamic>>();
            var dailyTasks = await GetDailyTasksAll(payload);

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            var dailyTasksGroupedByAreayAndDate = dailyTasks.Select(x =>
            {
                var territoryId = salesPointNodeMapps.FirstOrDefault(map => x.SalesPointId == map.SalesPointId)?.NodeId;
                var dateStr = DateTime.Parse(x.DateTimeStr).ToBangladeshTime().ToDisplayString();
                return new {
                    key=$"{territoryId}{dateStr}",
                    data =x,
                    territoryId
                };

            }).GroupBy(x=>x.key);

            foreach (var group in dailyTasksGroupedByAreayAndDate)
            {
                var firstItem = group.First();
                var list = group.ToList();
                var dailyPosmTaskItems = group.ToList().Select(x=>x.data).SelectMany(x=>x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems).ToList();
                    
                var dict = new Dictionary<string, dynamic>();
                var teritoryId = firstItem.territoryId;
                var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory.ParentId);
                var region = parentNodes.FirstOrDefault(x => x.NodeId == area.ParentId);


                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), firstItem.data.DateTimeStr);
                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);

                dict.Add(DCMAReportsInDetailsExceColumn.CmrCount.GetDescription(), list.Count);

                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTaskItems.Select(x => x.Quantity).Sum());

                var tcmTotal = dailyPosmTaskItems.Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                var pcmTotal = dailyPosmTaskItems
                    .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                posmNames.ForEach(x => dict.Add(x, 0));

                AddPosms(dailyPosmTaskItems, dict);

                result.Add(dict);
            }

            return result;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportRegionWise(GetExecutionReport payload)
        {
            var result = new List<Dictionary<string, dynamic>>();
            var dailyTasks = await GetDailyTasksAll(payload);

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            var dailyTasksGroupedByAreayAndDate = dailyTasks.Select(x =>
            {
                var nodeMap = salesPointNodeMapps.FirstOrDefault(map => x.SalesPointId == map.SalesPointId);
                var territory = parentNodes.FirstOrDefault(p => p.NodeId == nodeMap?.NodeId);

                var areaId = parentNodes.FirstOrDefault(p => p.NodeId == territory?.ParentId)?.NodeId;
                var dateStr = DateTime.Parse(x.DateTimeStr).ToBangladeshTime().ToDisplayString();
                return new
                {
                    key = $"{areaId}{dateStr}",
                    data = x,
                    areaId
                };

            }).GroupBy(x => x.key);

            foreach (var group in dailyTasksGroupedByAreayAndDate)
            {
                var firstItem = group.First();
                var list = group.ToList();
                var dailyPosmTaskItems = group.ToList().Select(x => x.data).SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems).ToList();

                var dict = new Dictionary<string, dynamic>();
                var areaId = firstItem.areaId;
                var area = parentNodes.FirstOrDefault(x => x.NodeId == areaId);
                var region = parentNodes.FirstOrDefault(x => x.NodeId == area?.ParentId);


                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), firstItem.data.DateTimeStr);
                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.CmrCount.GetDescription(), list.Count);
                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTaskItems.Select(x => x.Quantity).Sum());
                
                var tcmTotal = dailyPosmTaskItems.Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                var pcmTotal = dailyPosmTaskItems
                    .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                posmNames.ForEach(x => dict.Add(x, 0));

                AddPosms(dailyPosmTaskItems, dict);

                result.Add(dict);
            }

            return result;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportNational(GetExecutionReport payload)
        {
            var result = new List<Dictionary<string, dynamic>>();
            var dailyTasks = await GetDailyTasksAll(payload);

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            var dailyTasksGroupedByAreayAndDate = dailyTasks.Select(x =>
            {
                var nodeMap = salesPointNodeMapps.FirstOrDefault(map => x.SalesPointId == map.SalesPointId);
                var areaId = parentNodes.FirstOrDefault(p => p.NodeId == nodeMap?.NodeId)?.ParentId;
                var regionId = parentNodes.FirstOrDefault(p => p.NodeId == areaId)?.ParentId;
                var dateStr = DateTime.Parse(x.DateTimeStr).ToBangladeshTime().ToDisplayString();
                return new
                {
                    key = $"{regionId}{dateStr}",
                    data = x,
                    regionId
                };

            }).GroupBy(x => x.key);

            foreach (var group in dailyTasksGroupedByAreayAndDate)
            {
                var firstItem = group.First();
                var list = group.ToList();
                var dailyPosmTaskItems = group.ToList().Select(x => x.data).SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems).ToList();

                var dict = new Dictionary<string, dynamic>();
                var regionId = firstItem.regionId;
                var region = parentNodes.FirstOrDefault(x => x.NodeId == regionId);


                dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), firstItem.data.DateTimeStr);
                dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                dict.Add(DCMAReportsInDetailsExceColumn.CmrCount.GetDescription(), list.Count);
                dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTaskItems.Select(x => x.Quantity).Sum());
                
                var tcmTotal = dailyPosmTaskItems.Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                var pcmTotal = dailyPosmTaskItems
                    .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                posmNames.ForEach(x => dict.Add(x, 0));
                AddPosms(dailyPosmTaskItems, dict);
                result.Add(dict);
            }

            return result;
        }

        public async Task<List<Dictionary<string, dynamic>>> GetExecutionReportOutletWise(GetExecutionReport payload)
        {
            var dailyTasks = await GetDailyTasksAll(payload);
            var result = new List<Dictionary<string, dynamic>>();

            var salesPointIdsOfResult = dailyTasks.Select(x => x.SalesPointId).ToList();
            var salesPointNodeMapps = _salesPointNodeMapping.GetAll().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var parentNodes = _common.GetParentNodesBySalesPoint(salesPointIdsOfResult);

            var routes = _route.GetAllActive().Where(x => salesPointIdsOfResult.Contains(x.SalesPointId)).ToList();
            var channelIds = dailyTasks.SelectMany(x => x.DailyPosmTasks).Select(x => x.Outlet?.ChannelID).ToList();
            var channels = _channel.GetAllActive().Where(x => channelIds.Contains(x.ChannelID)).ToList();
            var posmNames = dailyTasks.SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                .Select(x => x.PosmProduct.Name).Distinct().ToList();

            foreach (var dailyTask in dailyTasks)
            {
                foreach (var dailyPosmTask in dailyTask.DailyPosmTasks)
                {
                    var dict = new Dictionary<string, dynamic>();
                    var teritoryId = salesPointNodeMapps.FirstOrDefault(x => x.SalesPointId == dailyTask.SalesPointId)?.NodeId;
                    var teritory = parentNodes.FirstOrDefault(x => x.NodeId == teritoryId);
                    var area = parentNodes.FirstOrDefault(x => x.NodeId == teritory?.ParentId);
                    var region = parentNodes.FirstOrDefault(x => x.NodeId == area?.ParentId);
                    var outlet = dailyPosmTask.Outlet;
                    var route = routes.FirstOrDefault(x => x.RouteId == outlet.RouteId);
                    var channel = channels.FirstOrDefault(x => x.ChannelID == outlet?.ChannelID);
                    var startTime = "";
                    var endTime = "";

                    if (DateTime.Parse(dailyPosmTask.CheckInStr).ToUniversalTime() != default(DateTime).ToUniversalTime())
                    {
                        startTime = dailyPosmTask.CheckInStr;
                    }
                    if (DateTime.Parse(dailyPosmTask.CheckOutStr).ToUniversalTime() != default(DateTime).ToUniversalTime())
                    {
                        endTime = dailyPosmTask.CheckOutStr;
                    }

                    dict.Add(DCMAReportsInDetailsExceColumn.Date.GetDescription(), dailyTask.DateTimeStr);
                    dict.Add(DCMAReportsInDetailsExceColumn.Region.GetDescription(), region?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Area.GetDescription(), area?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Territorry.GetDescription(), teritory?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.SalesPoint.GetDescription(), dailyTask.SalesPoint?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.Route.GetDescription(), route?.RouteName);
                    dict.Add(DCMAReportsInDetailsExceColumn.OutletType.GetDescription(), outlet?.OutletType);
                    dict.Add(DCMAReportsInDetailsExceColumn.Channel.GetDescription(), channel?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.OutletCode.GetDescription(), outlet?.Code);
                    dict.Add(DCMAReportsInDetailsExceColumn.OutletName.GetDescription(), outlet?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.CmrName.GetDescription(), dailyTask.CmUser?.Name);
                    dict.Add(DCMAReportsInDetailsExceColumn.CmrCode.GetDescription(), dailyTask.CmUser?.Code);
                    dict.Add(DCMAReportsInDetailsExceColumn.StartTime.GetDescription(), startTime);
                    dict.Add(DCMAReportsInDetailsExceColumn.EndTime.GetDescription(), endTime);
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalDuration.GetDescription(), "");
                    dict.Add(DCMAReportsInDetailsExceColumn.TotalExecution.GetDescription(), dailyPosmTask.DailyPosmTaskItems.Select(x => x.Quantity).Sum());

                    var tcmTotal = dailyPosmTask.DailyPosmTaskItems
                        .Where(x => x.PosmProduct.Type == (int)POSMProductType.Temporary).Select(x => x.Quantity).Sum();
                    dict.Add(DCMAReportsInDetailsExceColumn.Tcm.GetDescription(), tcmTotal);

                    var pcmTotal = dailyPosmTask.DailyPosmTaskItems
                        .Where(x => x.PosmProduct.Type == (int)POSMProductType.Permanent).Select(x => x.Quantity).Sum();
                    dict.Add(DCMAReportsInDetailsExceColumn.Pcm.GetDescription(), pcmTotal);

                    


                    posmNames.ForEach(x => dict.Add(x, 0));

                    AddPosms(dailyPosmTask.DailyPosmTaskItems, dict);

                    result.Add(dict);
                }
            }

            return result;
        }

        private async Task<List<DailyTaskModel>> GetDailyTasksAll(GetExecutionReport payload)
        {

            var query = _dailyPosmTask.GetAllActive().Where(x => x.DailyTask.IsSubmitted && 
                                 x.DailyPosmTaskItems.Any(dpi => dpi.ExecutionType == PosmWorkType.Installation || 
                                 dpi.ExecutionType == PosmWorkType.RemovalAndReInstallation) && payload.FromDateTime <= x.DailyTask.DateTime && x.DailyTask.DateTime <= payload.ToDateTime );
            if (payload.SalesPointIds.Any())
            {
                query = query.Where(x => payload.SalesPointIds.Contains(x.DailyTask.SalesPointId));
            }

            query = query.Include(x => x.DailyPosmTaskItems)
                .ThenInclude(x => x.PosmProduct)
                .Include(x => x.DailyTask)
                .ThenInclude(x => x.CmUser)
                .Include(x => x.Reason);

            var result = await query.OrderByDescending(x => x.DailyTask.DateTime).ToListAsync();
            var resultList = result;


            var dailyTasks = resultList.Select(x => x.DailyTask).DistinctBy(x => x.Id).MapToModel();
            foreach (var dailyTask in dailyTasks)
            {
                var posmTasks = resultList.Where(x => x.DailyTaskId == dailyTask.Id).MapToModel();
                posmTasks.ForEach(x => x.DailyPosmTaskItems = x.DailyPosmTaskItems.Where(x =>
                    x.ExecutionType == PosmWorkType.Installation || x.ExecutionType == PosmWorkType.RemovalAndReInstallation).ToList());
                dailyTask.DailyPosmTasks = posmTasks;
            }
            _common.InsertSalesPoints(dailyTasks);
            _common.InsertOutlets(dailyTasks.SelectMany(x => x.DailyPosmTasks).ToList());
            return dailyTasks;
        }

        private static void AddPosms(List<DailyPosmTaskItemsModel> dailyPosmTaskItems, Dictionary<string, dynamic> dict)
        {
            foreach (var dailyPosmTaskItem in dailyPosmTaskItems)
            {
                dict[dailyPosmTaskItem.PosmProduct.Name] += dailyPosmTaskItem.Quantity;
            }
        }

        private TaskCreationData GetTaskCreationData()
        {

            List<CMUser> cm_users = _cmUser.GetAll().Take(10).ToList();
            List<int> CM_UserIds = cm_users.Select(x => x.Id).ToList();
            List<string> cm_userNames = cm_users.Select(x => x.Name).ToList();
            List<string> posmNames = new List<string>() { "Item1", "Item2", "Item3", "Item4", "Item5" };
            List<string> headers = new List<string>() { "Date", "User Name", "User ID" };

            DateTime from = DateTime.Parse("2021-03-19T19:35:00.0000000Z");
            DateTime to = DateTime.Parse("2021-03-25T19:35:00.0000000Z");

            var dates = new List<DateTime>();

            for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            TaskCreationData taskCreationData = new TaskCreationData()
            {
                CMUserIds = CM_UserIds,
                CMUserNames = cm_userNames,
                PosmNames = posmNames,
                Headers = headers.Concat(posmNames).ToList(),
                Dates = dates
            };
            return taskCreationData;
        }

        private static void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }

        private FileData GetFileData(ExcelPackage excel)
        {
            string fileName = Guid.NewGuid() + ".xlsx";
            string path = Path.Combine(_tempPath, fileName);
            if (File.Exists(path)) File.Delete(path);

            FileStream objFileStrm = File.Create(path);
            objFileStrm.Close();

            File.WriteAllBytes(path, excel.GetAsByteArray());
            excel.Dispose();
            var fileData = _fileService.GetFileContentFromPath(path);
            _fileService.Delete(path);
            return fileData;
        }

        private void AutoFitColumns(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Column(i + 1).AutoFit();
            }
        }

        private void InsertRows(List<DateTime> dates, List<int> CM_UserIds, List<string> cm_userNames, ExcelWorksheet workSheet)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            foreach (var day in dates)
            {
                for (int i = 0; i < CM_UserIds.Count; i++)
                {
                    workSheet.Cells[currentRowNumberForData, 1].Value = day.ToString("dd-MMM-yy");
                    workSheet.Cells[currentRowNumberForData, 2].Value = cm_userNames[i];
                    workSheet.Cells[currentRowNumberForData, 3].Value = CM_UserIds[i];
                    currentRowNumberForData++;
                }
            }
        }

        public void SetTableStyle(ExcelWorksheet workSheet,int columnCount)
        {
            //for(var i = 0; i < columnCount; i++)
            //{
            //    workSheet.Column(i + 1).AutoFit();
            //}
           
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
        }

        public void SetHeaderStyle(ExcelWorksheet workSheet)
        {
            var header = workSheet.Row(1);
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(159, 208, 142));
            header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

    }
}
