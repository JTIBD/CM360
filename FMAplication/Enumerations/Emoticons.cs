using System.ComponentModel;

namespace FMAplication.Enumerations
{
    public enum Emoticons
    {
        [Description("Very Bad")]
        VeryBad = 50,

        [Description("Bad")]
        Bad = 100,

        [Description("Okay")]
        Okay = 150,

        [Description("Good")]
        Good = 200,

        [Description("Very Good")]
        VeryGood = 250
    }
}