using FMAplication.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Services.Common.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardModel> GetAllDashboardDataAsync();
    }
}
