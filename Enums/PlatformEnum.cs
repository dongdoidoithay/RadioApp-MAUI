using System.ComponentModel;

namespace RadioApp.Enums;

[Flags]
public enum PlatformEnum
{
    [Description("Sách nói")]
    AudioBook,
    [Description("Nhạc")]
    Music,
    [Description("Truyện tiểu thuyết")]
    Novels,
    [Description("Truyện Tranh")]
    Mangas,
    [Description("Comics Marvel")]
    Comics
}

