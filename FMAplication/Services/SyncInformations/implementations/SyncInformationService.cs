using FMAplication.Domain.SyncInformations;
using FMAplication.Models.SyncInformations;
using FMAplication.Repositories;
using FMAplication.Services.SyncInformations.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FMAplication.Extensions;
using System;

namespace FMAplication.Services.SyncInformations.implementations
{
    public class SyncInformationService : ISyncInformationService
    {
        private readonly IRepository<SyncInformation> _syncInformation;
        public SyncInformationService(IRepository<SyncInformation> syncInformation)
        {
            _syncInformation = syncInformation;
        }

        public async Task<SyncInformationModel> Create(SyncInformationModel payload)
        {
            SyncInformation newRecord = new SyncInformation()
            {
                LastSyncTime = payload.LastSyncTime
            };
            await _syncInformation.CreateAsync(newRecord);
            return newRecord.ToMap<SyncInformation, SyncInformationModel>();
        }

        public async Task<List<SyncInformationModel>> GetAll()
        {
            var list =  _syncInformation.GetAll().OrderByDescending(x=>x.CreatedTime).ToList();
            var models = list.ToMap<SyncInformation,SyncInformationModel>();
            return await Task.FromResult(models);
        }

        public async Task<SyncInformationModel> Update(SyncInformationModel payload)
        {
            var record = await _syncInformation.FindAsync(x => x.Id == payload.Id);
            if (record is null) throw new Exception("SyncInformation not found");
            record.LastSyncTime = payload.LastSyncTime;
            await _syncInformation.UpdateAsync(record);
            return record.ToMap<SyncInformation, SyncInformationModel>();
        }
    }
}
