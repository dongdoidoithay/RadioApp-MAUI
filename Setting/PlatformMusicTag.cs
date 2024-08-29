namespace RadioApp.Setting;

public class PlatformMusicTag
{
    public List<MusicTag> HotTags { get; set; }

    public List<MusicTypeTag> AllTypes { get; set; }

    public PlatformMusicTag(List<MusicTag> hotTags, List<MusicTypeTag> allTypes)
    {
        HotTags = hotTags;
        AllTypes = allTypes;
    }
}

/// <summary>
/// Music type label
/// </summary>
public class MusicTypeTag
{
    public string TypeName { get; set; } = null!;
    public List<MusicTag> Tags { get; set; } = null!;
}

/// <summary>
/// Music label
/// </summary>
public class MusicTag
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}