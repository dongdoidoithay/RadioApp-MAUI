using RadioApp.Enums;

namespace RadioApp.Setting;

public class EnvironmentSetting
{
    /// <summary>
    /// Player settings
    /// </summary>
    public PlayerSetting Player { get; set; } = null!;
    /// <summary>
    /// Conventional settings
    /// </summary>
    public GeneralSetting General { get; set; } = null!;
    /// <summary>
    /// Platform settings
    /// </summary>
    public SearchSetting Search { get; set; } = null!;
    /// <summary>
    /// Play settings
    /// </summary>
    public PlaySetting Play { get; set; } = null!;
}

public class PlayerSetting
{
    /// <summary>
    /// volume
    /// </summary>
    public double Volume { get; set; }

    /// <summary>
    /// Whether it is mute
    /// </summary>
    public bool IsSoundOff { get; set; }

    /// <summary>
    /// Play mode
    /// </summary>
    public PlayModeEnum PlayMode { get; set; }
}

public class GeneralSetting
{
    /// <summary>
    /// Whether to automatically check the update
    /// </summary>
    public bool IsAutoCheckUpdate { get; set; }

    /// <summary>
    /// Exterior
    /// </summary>
    public int AppThemeInt { get; set; }

    /// <summary>
    /// To minimize when closed
    /// </summary>
    public bool IsHideWindowWhenClosed { get; set; } = false;
}

public class SearchSetting
{
    /// <summary>
    /// Hidden songs with less than 1 minute
    /// </summary>
    public bool IsHideShortMusic { get; set; }

    /// <summary>
    /// The song name or singer name must include search words
    /// </summary>
    public bool IsMatchSearchKey { get; set; }
}

public class PlaySetting
{
    /// <summary>
    /// You can play only under wifi
    /// </summary>
    public bool IsWifiPlayOnly { get; set; }

    /// <summary>
    /// Play page prohibiting screen closure
    /// </summary>
    public bool IsPlayingPageKeepScreenOn { get; set; }

    /// <summary>
    /// Clear the playlist before playing the song list
    /// </summary>
    public bool IsCleanPlaylistWhenPlaySongMenu { get; set; }
}
