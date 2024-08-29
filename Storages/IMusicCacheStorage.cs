using RadioApp.Models;

namespace RadioApp.Storages;

public interface IMusicCacheStorage
{
    Task<string> GetOrAddAsync(Episode playlist, Func<Episode, Task<MusicCacheMetadata>> delegateFunc);
    Task CalcCacheSizeAsync(Action<double> delegage);
    Task ClearCacheAsync();
}