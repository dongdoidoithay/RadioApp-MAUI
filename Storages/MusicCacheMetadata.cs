using RadioApp.Models;
using Microsoft.Extensions.Logging;

namespace RadioApp.Storages;

public class MusicCacheStorage : IMusicCacheStorage
{
    private readonly ILogger<MusicCacheStorage> _logger;
    public MusicCacheStorage(ILogger<MusicCacheStorage> logger)
    {
        _logger = logger;
    }
    public Task CalcCacheSizeAsync(Action<double> delegage)
    {
        var files = Directory.GetFiles(GlobalConfig.MusicCacheDirectory);
        foreach (var file in files)
        {
            var fi = new FileInfo(file);
            delegage(fi.Length);
        }
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync()
    {
        var files = Directory.GetFiles(GlobalConfig.MusicCacheDirectory);
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Captive file deletion failed。");
            }
        }
        return Task.CompletedTask;
    }

    public async Task<string> GetOrAddAsync(Episode playlist, Func<Episode, Task<MusicCacheMetadata>> delegateFunc)
    {
        var fileName = Preferences.Get($"music-{playlist.episodeId}", "");
        if (fileName.IsNotEmpty() && File.Exists(fileName))
        {
            return fileName;
        }
        var musicCacheMetadata = await delegateFunc(playlist);
        if (musicCacheMetadata == null)
        {
            return default;
        }

        var cacheFileNameOnly = $"{playlist.episodeId}{musicCacheMetadata.FileExtension}";
        var cachePath = Path.Combine(GlobalConfig.MusicCacheDirectory, cacheFileNameOnly);
        await File.WriteAllBytesAsync(cachePath, musicCacheMetadata.Buffer);

        Preferences.Set($"music-{playlist.episodeId}", cachePath);
        return cachePath;
    }
}