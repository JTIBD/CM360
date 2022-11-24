using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FMAplication.Repositories;
using FMAplication.Services.AvCommunication.Interfaces;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.DailyTasks;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.Brand;
using FMAplication.Services.AzureStorageService.Interfaces;
using FMAplication.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Hosting;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.Domain.Users;
using FMAplication.Exceptions;
using FMAplication.Helpers;

namespace FMAplication.Services.AvCommunication.Implementation
{
    public class AvCommunicationService : IAvCommunicationService
    {
        private readonly IRepository<Domain.AVCommunications.AvCommunication> _avCommunication;
        private readonly IRepository<Domain.Brand.Brand> _brand;
        private readonly IRepository<CommunicationSetup> _communicationSetup;
        private readonly IRepository<AvSetup> _avSetup;
        private readonly IWebHostEnvironment _host;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IRepository<CmsUserSalesPointMapping> _cmsUserSalesPointMapping;
        private readonly IRepository<DailyAVTask> _dailyAvTask;
        private readonly IRepository<DailyCommunicationTask> _dailyCommunicationTask;


        public AvCommunicationService(IRepository<Domain.AVCommunications.AvCommunication> avCommunication, 
         IRepository<Domain.Brand.Brand> brand, IWebHostEnvironment host, IRepository<AvSetup> avSetup,
         IBlobStorageService blobStorageService, IRepository<CommunicationSetup> communicationSetup,
         IRepository<CMUser> cmUser, IRepository<CmsUserSalesPointMapping> cmsUserSalesPointMapping, IRepository<DailyAVTask> dailyAvTask, IRepository<DailyCommunicationTask> dailyCommunicationTask)
        {
            _avCommunication = avCommunication;
            _brand = brand;
            _host = host;
            _blobStorageService = blobStorageService;
            _communicationSetup = communicationSetup;
            _avSetup = avSetup;
            _cmUser = cmUser;
            _cmsUserSalesPointMapping = cmsUserSalesPointMapping;
            _dailyAvTask = dailyAvTask;
            _dailyCommunicationTask = dailyCommunicationTask;
        }
        public async Task<List<AvCommunicationViewModel>> GetAvsCommunications()
        {
            var data = _avCommunication.GetAll().Where(x=>x.Status == Status.Active).OrderByDescending(x=>x.CreatedTime).ToList()
                .ToMap<Domain.AVCommunications.AvCommunication, AvCommunicationViewModel>();

            var brandIds = await _avCommunication.GetAll().Select(x => x.BrandId).Distinct().ToListAsync();
            var brands = _brand.GetAll().Where(x => brandIds.Contains(x.Id)).ToList()
                .ToMap<Domain.Brand.Brand, BrandModel>();

            foreach (var aVModel in data)
            {
                aVModel.BrandModel = brands.FirstOrDefault(x => x.Id == aVModel.BrandId);
            }

            await SetEditAndDeletability(data);

            return data;
        }

        public async Task SaveAvCommunication(AvCommunicationViewModel model)
        {
           var filePath = await GetFilePath(model);

           var avModel = new Domain.AVCommunications.AvCommunication
           {
               BrandId = model.BrandId, CampaignType = model.CampaignType, 
               Description = model.Description,
               FilePath = filePath, 
               CampaignName =  model.CampaignName
           };
          await _avCommunication.CreateAsync(avModel);
        }

        private async Task SetEditAndDeletability(List<AvCommunicationViewModel> avModels)
        {
            if (!avModels.Any()) return;
            var ids = avModels.Select(x => x.Id);
            var communicationSetups = await _communicationSetup.FindAllAsync(x => ids.Contains(x.AvCommunicationId));
            var avSetups = _avSetup.FindAll(x => ids.Contains(x.AvId)).ToList();
            var datetimeNow = DateTime.UtcNow;

            avModels.ForEach(x =>
            {
                x.IsDeletable = communicationSetups.All(c => c.AvCommunicationId != x.Id) && avSetups.All(setup => setup.AvId != x.Id);
            });

            avModels.ForEach(x =>
            {
                x.IsEditable = communicationSetups.All(c => c.AvCommunicationId != x.Id || c.FromDate > datetimeNow || datetimeNow > c.ToDate) &&
                               avSetups.All(setup=>setup.AvId != x.Id || setup.FromDate > datetimeNow || datetimeNow > setup.ToDate);
            });

            var theRests = avModels.FindAll(x => x.IsEditable);

            var communicationSetupIds = communicationSetups.Select(x => x.Id).ToList();
            var avSetupIds = avSetups.Select(x => x.AvId).ToList();

            var dailyAvTasks = _dailyAvTask.GetAll().Where(x => avSetupIds.Contains(x.AvSetupId)).ToList();
            var avSetupsHavingReport = avSetups.Where(x => dailyAvTasks.Any(task => task.AvSetupId == x.Id)).ToList();

            var dailyCommunicationTasks = _dailyCommunicationTask.GetAll()
                .Where(x => communicationSetupIds.Contains(x.CommunicationSetupId)).ToList();
            var communicationSetupHavingReport = communicationSetups
                .Where(x => dailyCommunicationTasks.Any(task => task.CommunicationSetupId == x.Id)).ToList();

            foreach (var avCommunication in theRests)
            {
                avCommunication.IsEditable = avSetupsHavingReport.All(x => x.AvId != avCommunication.Id) &&
                                             communicationSetupHavingReport.All(x => x.AvCommunicationId != avCommunication.Id);
            }
        }

        private async Task<string> GetFilePath(AvCommunicationViewModel model)
        {
            if (model.File == null) throw new AppException("Uploaded file is required.");
            if (!IsValidFile(model.File, model.CampaignType)) return null;
            if (!IsValidFileSize(model.File, model.CampaignType)) return null;

            var fileName = Path.GetFileName(model.File.FileName);
            string mimeType = "application/octet-stream";//model.File.ContentType;


            byte[] fileData;
            await using (var target = new MemoryStream())
            {
                await model.File.CopyToAsync(target);
                fileData = target.ToArray();
            }

            string folder = model.CampaignType == AvCommunicationCampaignType.Image ? "Communication" : "Av";

            var filePath = await _blobStorageService.UploadFileToBlobAsync(fileName,
                fileData, mimeType, folder);
            return filePath;
        }

        public async Task RemoveAvCommunication(int id)
        {
            var existingAv = await _avCommunication.FindAsync(x => x.Id == id); 
            if (existingAv == null) throw new AppException("AV communication not found");

            existingAv.Status = Status.InActive;
            await _avCommunication.UpdateAsync(existingAv);
        }

        public async Task<AvCommunicationViewModel> GetAvsCommunication(int id)
        {
            var data = (await _avCommunication.FindAsync(x => x.Status == Status.Active && x.Id == id))
                .ToMap<Domain.AVCommunications.AvCommunication, AvCommunicationViewModel>();

            if (data == null) throw new AppException("AV communication not found");
            
            data.BrandModel =  (await _brand.GetAll().FirstOrDefaultAsync(x => x.Id == data.BrandId)).ToMap<Domain.Brand.Brand, BrandModel>();
            return data;
        }

        public async Task UpdateAvCommunication(AvCommunicationViewModel model)
        {
            var existingAv = await _avCommunication.FindAsync(x => x.Id == model.Id);
            if (existingAv == null) throw new AppException("AV communication not found");

            existingAv.BrandId = model.BrandId;
            existingAv.Description = model.Description;
            existingAv.CampaignType = model.CampaignType;
            existingAv.CampaignName = model.CampaignName;

            if (model.FilePath != existingAv.FilePath)
            {
                existingAv.FilePath = await GetFilePath(model);   
            }

            await _avCommunication.UpdateAsync(existingAv);
        }

     


        #region private Methods

        private int GetFileSizeInMb(IFormFile file)
        {
            return (int) (file.Length / (1024 * 1024));
        }

        private bool IsValidFile(IFormFile file, AvCommunicationCampaignType campaignType)
        {
            string ext = Path.GetExtension(file.FileName);
            var validFormat = campaignType == AvCommunicationCampaignType.Video
                ? ApplicationConst.ValidVideoFormat
                : ApplicationConst.ValidImageFormat;

            if (!(validFormat.IndexOf(ext) > -1))
                throw  new AppException($"Invalid file type");

            return true;
        }

        private bool IsValidFileSize(IFormFile file, AvCommunicationCampaignType campaignType)
        {
            int standardFileSize = campaignType == AvCommunicationCampaignType.Video  ? 30 : 1;

            int fileSize = GetFileSizeInMb(file);

             if (fileSize > standardFileSize)
                 throw new AppException("File size limit exceeded");
             return true;
        }     

        #endregion
    }
}
