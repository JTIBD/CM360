using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain;
using FMAplication.Domain.DailyTasks;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Reasons;
using FMAplication.Repositories;
using FMAplication.Services.Reasons.Interfaces;
using FMAplication.MobileModels;
using FMAplication.MobileModels.Reasons;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services.Reasons.Implementations
{
    public class ReasonService : IReasonService
    {
        private readonly IRepository<Reason> _reasonRepository;
        private readonly IRepository<ReasonReasonTypeMapping> _reasonReasonTypeMapping;
        private readonly IRepository<ReasonType> _reasonType;
        private readonly IRepository<DailyAuditTask> _dailyAuditTask;
        private readonly IRepository<DailyAVTask> _dailyAvTask;
        private readonly IRepository<DailyCommunicationTask> _dailyCommunicationTask;
        private readonly IRepository<DailyConsumerSurveyTask> _dailyConsumerSurveyTask;
        private readonly IRepository<DailyInformationTask> _dailyInformationTask;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<DailySurveyTask> _dailySurveyTask;

        public ReasonService(IRepository<Reason> repository, IRepository<ReasonReasonTypeMapping> reasonReasonTypeMapping, IRepository<ReasonType> reasonReasonType, IRepository<DailyAuditTask> dailyAuditTask, IRepository<DailyAVTask> dailyAvTask, IRepository<DailyCommunicationTask> dailyCommunicationTask, IRepository<DailyConsumerSurveyTask> dailyConsumerSurveyTask, IRepository<DailyInformationTask> dailyInformationTask, IRepository<DailyPosmTask> dailyPosmTask, IRepository<DailySurveyTask> dailySurveyTask)
        {
            _reasonRepository = repository;
            _reasonReasonTypeMapping = reasonReasonTypeMapping;
            _reasonType = reasonReasonType;
            _dailyAuditTask = dailyAuditTask;
            _dailyAvTask = dailyAvTask;
            _dailyCommunicationTask = dailyCommunicationTask;
            _dailyConsumerSurveyTask = dailyConsumerSurveyTask;
            _dailyInformationTask = dailyInformationTask;
            _dailyPosmTask = dailyPosmTask;
            _dailySurveyTask = dailySurveyTask;
        }


        public async Task<List<ReasonModel>> GetAllReasonsAsync()
        {
            var res = await _reasonRepository.GetAll().Include(x=>x.ReasonReasonTypeMappings)
                    .ThenInclude(x=>x.ReasonType).ToListAsync();
            res.ForEach(x =>
            {
                x.ReasonReasonTypeMappings = x.ReasonReasonTypeMappings.Where(r => r.ReasonType?.Text != "Av").ToList();
            });
            return res.MapToModel();
        }

        public async Task<ReasonModel> GetReasonAsync(int id)
        {
            var result = await _reasonRepository.GetAllActive().Where(s => s.Id == id)
                .Include(x => x.ReasonReasonTypeMappings).FirstOrDefaultAsync();
            return result.MapToModel();
        }

        public async Task<List<ReasonsMBModel>> GetAllReasonForMobileAsync()
        {
            var res = await _reasonRepository.GetAllActive().ToListAsync();
            return res.ToMap<Reason, ReasonsMBModel>();
        }

        public async Task<List<ReasonsWithType>> GetAllReasonsWithTypes()
        {
            var listAsync = await _reasonType.GetAllActive().Where(x=>x.Text != "Av").Include(x => x.ReasonReasonTypeMappings)
                    .ThenInclude(x => x.Reason).ToListAsync();
            var result = new List<ReasonsWithType>();
            
            foreach (var item in listAsync)
            {
                var reasonWithType = new ReasonsWithType();
                reasonWithType.Type = item.Code;
                reasonWithType.ReasonInType = item.ReasonReasonTypeMappings.Select(x=>x.Reason).ToMap<Reason,ReasonsMBModel>();
                result.Add(reasonWithType);
            }
            return result;
        }

        public async Task<List<ReasonTypeModel>> GetAllReasonTypes()
        {
            var list = await _reasonType.GetAllActive().Where(x=>x.Text != "Av").ToListAsync();
            return list.ToMap<ReasonType, ReasonTypeModel>();
        }

        public async Task<ReasonModel> CreateAsync(ReasonModel model)
        {
            var example = model.ToMap<ReasonModel, Reason>();

            if (model.ReasonReasonTypeMappings?.Any() == true)
            { 
                example.ReasonReasonTypeMappings = model.ReasonReasonTypeMappings.ToMap<ReasonReasonTypeMappingModel, ReasonReasonTypeMapping>();
            }
            var result = await _reasonRepository.CreateAsync(example);
            return result.ToMap<Reason, ReasonModel>();
        }

        public async Task<ReasonModel> UpdateAsync(ReasonModel model)
        {
            var existing = await _reasonRepository.GetAllActive().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            existing.ReasonReasonTypeMappings =
                _reasonReasonTypeMapping.GetAllActive().Where(x => x.ReasonId == model.Id).ToList();
            if (existing is null) throw new AppException("Reason not found");
            var example = model.ToMap<ReasonModel, Reason>();
            if (model.ReasonReasonTypeMappings?.Any() == true)
            {
                var addedMaps = model.ReasonReasonTypeMappings
                    .Where(x => !existing.ReasonReasonTypeMappings.Any(ex => ex.ReasonTypeId == x.ReasonTypeId)).ToList();
                var removedTypes = existing.ReasonReasonTypeMappings
                    .Where(x => !model.ReasonReasonTypeMappings.Any(ex => ex.ReasonTypeId == x.ReasonTypeId)).ToList();
                if (removedTypes.Any()) await _reasonReasonTypeMapping.DeleteListAsync(removedTypes);
                
                example.ReasonReasonTypeMappings = addedMaps.ToMap<ReasonReasonTypeMappingModel, ReasonReasonTypeMapping>();
            }
            var result = await _reasonRepository.UpdateAsync(example);
            return result.ToMap<Reason, ReasonModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existing = _reasonRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (existing is null) throw new AppException("Reason not found.");
            await CheckReferences(id);
            var result = await _reasonRepository.DeleteAsync(s => s.Id == id);
            return result;
        }

        private async Task CheckReferences(int reasonId)
        {
            var dailyAuditTasks = await _dailyAuditTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailyAvTasks = await _dailyAvTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailyCommunicationTasks = await _dailyCommunicationTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailyConsumerSurveyTask = await _dailyConsumerSurveyTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailyInformationTask = await  _dailyInformationTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailyPosmTasks = await _dailyPosmTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var dailySurveyTasks = await _dailySurveyTask.GetAll().CountAsync(x => x.ReasonId == reasonId);
            var references = new List<string>();
            if(dailyAuditTasks > 0) references.Add("Audit task");
            if(dailyAvTasks > 0) references.Add("Av task");
            if(dailyCommunicationTasks > 0) references.Add("Communication task");
            if(dailyConsumerSurveyTask > 0) references.Add("Consumer survey");
            if(dailyInformationTask > 0) references.Add("Information task");
            if(dailyPosmTasks > 0) references.Add("Posm task");
            if(dailySurveyTasks > 0) references.Add("Customer Survey");
            if (references.Any()) throw new AppException($"The reason has been used in {string.Join(", ", references)}");
        }

        public async Task<bool> IsCodeExistAsync(string name, int id)
        {
            var result = id <= 0
                ? await _reasonRepository.IsExistAsync(s => s.Name == name)
                : await _reasonRepository.IsExistAsync(s => s.Name == name && s.Id != id);

            return result;
        }

    }
}
