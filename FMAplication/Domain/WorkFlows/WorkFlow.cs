using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMAplication.Attributes;
using FMAplication.Core;
using FMAplication.Enumerations;

namespace FMAplication.Domain.WorkFlows
{
    public class WorkFlow : AuditableEntity<int>
    {
        //[Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

       

        public int WorkflowType { get; set; }

        public WorkflowConfigType WorkflowConfigType { get; set; }

        public int WorkflowStep { get; set; }//Workflow Step (numbrt)

        [UniqueKey]
        //[Required]
        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        public List<WorkflowLog> Logs { get; set; }

        public List<WorkFlowConfiguration> Configurations { get; set; }

    }
}