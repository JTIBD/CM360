﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Core;
using FMAplication.Enumerations;

namespace FMAplication.Domain.WorkFlows
{

    public class WorkflowLog : AuditableEntity<int>
    {
        public int RowId { get; set; }
        public int WorkFlowFor { get; set; }
        public int MasterWorkFlowId { get; set; }
        public string TableName { get; set; }
        public int OrgRoleId { get; set; }
        public int WorkflowStatus { get; set; }
        [ForeignKey("MasterWorkFlowId")]
        public WorkFlow WorkFlow { get; set; }
        public List<WorkflowLogHistory> Histories { get; set; }


    }
}
