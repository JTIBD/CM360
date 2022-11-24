using FMAplication.Attributes;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.WorkFlows
{
    public class WorkFlowModel
    {
        
        public WorkFlowModel()
        {
            this.ConfigList = new List<WorkFlowConfigurationModel>();
        }
        public int Id { get; set; }
        [Required]
       
        public string Name { get; set; }

        public int WorkflowType { get; set; }

        public int WorkflowStep { get; set; }
        public WorkflowConfigType WorkflowConfigType { get; set; }

        public string Code { get; set; }
        public Status Status { get; set; }
        

        public List<WorkFlowConfigurationModel> ConfigList;


    }
}