using System.ComponentModel;


namespace RadioApp.Enums;
public enum PlayModeEnum
{
    [Description("Repeat One")]
    RepeatOne,
    [Description("Repeat List")]
    RepeatList,
    [Description("Shuffle")]
    Shuffle
}