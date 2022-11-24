using FMAplication.Models.SyncInformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.SyncInformations.Interfaces
{
    public interface ISyncInformationService
    {
        Task<List<SyncInformationModel>> GetAll();
        Task<SyncInformationModel> Update(SyncInformationModel payload);
        Task<SyncInformationModel> Create(SyncInformationModel payload);
    }
}
