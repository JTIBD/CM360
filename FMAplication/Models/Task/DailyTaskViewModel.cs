using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.MobileModels.Surveys;
using FMAplication.Models.AvCommunications;
using FMAplication.Models.PosmAssign;
using FMAplication.Models.Survey;

namespace FMAplication.Models.Task
{
    public class DailyTaskViewModel
    {
        public DailyTaskViewModel()
        {
            PosmTasks = new List<PosmAssignTaskModel>();
        }

        public  int SalesPointId { get; set; }
        public DateTime TaskDate { get; set; }


        public List<PosmAssignTaskModel> PosmTasks { get; set; }
        public List<SurveyMBModel> Surveys { get; set; }
        public List<AvSetupModel> AvSetups { get; set; }
    }
}
