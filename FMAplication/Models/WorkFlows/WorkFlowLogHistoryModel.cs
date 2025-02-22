﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.WorkFlows;
using FMAplication.Enumerations;

namespace FMAplication.Models.WorkFlows
{
    public class WorkFlowLogHistoryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int WorkflowLogId { get; set; }
        //public WorkFlowLogModel WorkflowLog { get; set; }
        public WorkflowStatus WorkflowStatus { get; set; }
        public string WorkflowTitle { get; set; }
        public string Comments { get; set; }
        public bool IsSeen { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
