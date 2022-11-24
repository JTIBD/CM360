using FMAplication.MobileModels.AvCommunications;
using FMAplication.Models.AvCommunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.AvCommunication.Interfaces
{
    public interface IAvCommunicationService
    {
        Task<List<AvCommunicationViewModel>> GetAvsCommunications();
        Task SaveAvCommunication(AvCommunicationViewModel model);
        Task RemoveAvCommunication(int id);
        Task<AvCommunicationViewModel> GetAvsCommunication(int id);
        Task UpdateAvCommunication(AvCommunicationViewModel model);
    }
}
