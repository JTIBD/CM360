using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.DailyTasks
{
    public class DailyAVTask : DailyBaseTask
    {

        public int AvSetupId { get; set; }
    }
}