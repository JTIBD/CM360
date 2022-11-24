using System.ComponentModel;

namespace FMAplication.Enumerations
{
    public enum DailyTaskType
    {
        [Description("CustomerSurvey")]
        CustomerSurvey = 100,

        [Description("ConsumerSurvey")]
        ConsumerSurvey = 150,

        [Description("PosmDistribution")]
        PosmDistribution =200,

        [Description("Av")]
        Av = 250,

        [Description("Communication")]
        Communication = 300,

        [Description("Information")]
        Information = 350,

        [Description("Audit")]
        Audit = 400
    }
}