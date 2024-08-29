using RadioApp.Setting;

namespace RadioApp;

internal class GlobalConfig
{
    /// <summary>
    /// APP name
    /// </summary>
    private const string AppName = "RadioApp";
    /// <summary>
    /// Local database name
    /// </summary>
    public const string LocalDatabaseName = "RadioZing.db";
    /// <summary>
    /// App Data folder path
    /// </summary>
    public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

    /// <summary>
    /// Cache path
    /// </summary>
    public static readonly string CacheDirectory = Path.Combine(AppDataDirectory, "cache");
    /// <summary>
    /// Song cache path
    /// </summary>
    public static readonly string MusicCacheDirectory = Path.Combine(CacheDirectory, "musics");

    public static Version CurrentVersion { get; set; }
    public static string CurrentVersionString => CurrentVersion.ToString();

    public static string ApiDomain { get; set; }

    public static string UpdateDomain { get; set; }

    public static EnvironmentSetting MyUserSetting { get; set; }
}
