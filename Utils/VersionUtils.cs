using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioApp.Utils;

/// <summary>
/// Program version help class
/// </summary>
public static class VersionUtils
{
    /// <summary> 
    /// Check if the current version needs to be updated 
    /// </summary> 
    /// <param name="current Version">Current version</param> 
    /// <param name="new Version">Latest version</param> 
    /// <returns>Returns whether an update is needed</returns>
    public static bool CheckNeedUpdate(string currentVersion, string newVersion)
    {
        Version current = ToVersionWithBuild(currentVersion);
        Version version = ToVersionWithBuild(newVersion);
        return CheckNeedUpdate(current, version);
    }

    private static Version ToVersionWithBuild(string versionString)
    {
        if (Version.TryParse(versionString, out var version))
        {
            var build = version.Build;
            if (build == -1)
            {
                build = 0;
            }
            var revision = version.Revision;
            if (revision == -1)
            {
                revision = 0;
            }
            version = new Version(version.Major, version.Minor, build, revision);
        }
        return version;
    }

    /// <summary> 
    /// Check if the current version needs to be updated 
    /// </summary> 
    /// <param name="current Version">Current version</param> 
    /// <param name="new Version">Latest version</param> 
    /// <returns>Returns whether an update is needed</returns>
    public static bool CheckNeedUpdate(Version currentVersion, Version newVersion)
    {
        return currentVersion.CompareTo(newVersion) < 0;
    }

    /// <summary> 
    /// Check if the current version needs to be updated 
    /// </summary> 
    /// <param name="current Version">Current version</param> 
    /// <param name="new Version">Latest version</param> 
    /// <param name="min Version">Minimum running version</param> 
    /// <returns>Return (whether automatic update is required, whether the current version is allowed to be used)</returns>
    public static (bool IsNeedUpdate, bool IsAllowUse) CheckNeedUpdate(string currentVersion, string newVersion, string minVersion)
    {
        Version current = ToVersionWithBuild(currentVersion);
        Version version = ToVersionWithBuild(newVersion);
        Version min = ToVersionWithBuild(minVersion);
        return CheckNeedUpdate(current, version, min);
    }

    /// <summary> 
    /// Check if the current version needs to be updated 
    /// </summary> 
    /// <param name="current Version">Current version</param> 
    /// <param name="new Version">Latest version</param> 
    /// <param name="min Version">Minimum running version</param> 
    /// <returns>Return (whether automatic update is required, whether the current version is allowed to be used)</returns>
    public static (bool IsNeedUpdate, bool IsAllowUse) CheckNeedUpdate(Version currentVersion, Version newVersion, Version minVersion)
    {
        bool isNeedUpdate = CheckNeedUpdate(currentVersion, newVersion);
        int result = currentVersion.CompareTo(minVersion);
        if (result < 0)
        {
            return (isNeedUpdate, false);
        }
        else if (result == 0)
        {
            return (isNeedUpdate, true);
        }
        else
        {
            return (isNeedUpdate, true);
        }
    }
}
