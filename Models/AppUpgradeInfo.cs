
namespace RadioApp.Models;

/// <summary> 
/// A general model required for automatic app updates 
/// </summary>
public class AppUpgradeInfo
{
    /// <summary>
    /// Program name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// version number
    /// </summary>
    public int? VersionCode { get; set; }
    /// <summary>
    /// current version
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// The smallest version of the program running
    /// </summary>
    public string MinVersion { get; set; }
    /// <summary>
    /// download link
    /// </summary>
    public string DownloadUrl { get; set; }
    /// <summary>
    /// Update log
    /// </summary>
    public string Log { get; set; }
    /// <summary>
    ///File signature type (e.g. MD5, SHA1, etc.)
    /// </summary>
    public string SignType { get; set; }
    /// <summary>
    /// Signature
    /// </summary>
    public string SignValue { get; set; }
    /// <summary>
    /// time
    /// </summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>
    /// File size
    /// </summary>
    public int? FileLength { get; set; }
}