using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.WorkFlows
{
    public class WorkflowTypeModel
    {
        public int Id { get; set; }
        [StringLength(256, MinimumLength = 1)]
        public string WorkflowTypeName { get; set; }
        public string WorkflowMessage { get; set; }

        [StringLength(256, MinimumLength = 1)]
        public string DbTableName { get; set; }
        public bool IsActive { get; set; }
        public bool IsWorkflowDefAvailable { get; set; }
        public bool IsWorkflowConfigAvailable { get; set; }
    }
}
