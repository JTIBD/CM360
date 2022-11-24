using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Questions;
using FMAplication.Enumerations;
using FMAplication.MobileModels.AvCommunications;
using FMAplication.MobileModels.Reasons;
using TaskStatus = FMAplication.Enumerations.TaskStatus;

namespace FMAplication.MobileModels.DailyTasks
{

    public class DailySurveyExecutionModel : ConsumerSurveyExecutionModel
    {

    }
    public class DailySurveyTaskMBModel : DailyTaskBaseMbModel
    {
        public DailySurveyTaskMBModel()
        {
            QuestionAnswers = new List<DailySurveyTaskAnswerMBModel>();
        }
        public DailySurveyExecutionModel SurveyExecution { get; set; }
        public ICollection<DailySurveyTaskAnswerMBModel> QuestionAnswers { get; set; }
    }


    public static class DailySurveyTaskMBModelExtensions
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DailySurveyTask, DailySurveyTaskMBModel>();
            cfg.CreateMap<DailyTask, DailyTaskMBModel>();
        }).CreateMapper();

        public static DailySurveyTaskMBModel MapToMBModel(this DailySurveyTask source)
        {
            var result = Mapper.Map<DailySurveyTaskMBModel>(source);
            return result;
        }

        public static List<DailySurveyTaskMBModel> MapToMBModel(this IEnumerable<DailySurveyTask> source)
        {
            var result = Mapper.Map<List<DailySurveyTaskMBModel>>(source);
            return result;
        }

    }

}
