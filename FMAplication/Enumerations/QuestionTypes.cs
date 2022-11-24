using System.ComponentModel;

namespace FMAplication.Enumerations
{
    public enum QuestionTypes
    {
        [Description("Single Choice")]
        SingleChoice = 1,

        [Description("Multiple Choice")]
        MultipleChoice = 50,

        [Description("Emo")]
        Emo = 100,

        [Description("Rating")]
        Rating = 150,

        [Description("Date Picker")]
        DatePicker = 200,

        [Description("Dropdown")]
        Dropdown = 250,

        [Description("Email")]
        Email = 300,

        [Description("Slider")]
        Slider = 350,

        [Description("Slider")]
        Text = 400,

        [Description("Slider")]
        YesNo = 450,

        [Description("Signature")]
        Signature = 500,

        [Description("Signature")]
        Numeric = 550,
    }
}