using System;
using FMAplication.Core;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyCommunicationTask : DailyBaseTask
    {
        
        public int CommunicationSetupId { get; set; }

    }
}