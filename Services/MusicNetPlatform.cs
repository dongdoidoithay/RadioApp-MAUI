
using RadioApp.Enums;
using RadioApp.Models;
using RadioApp.Services.Interface;
using RadioApp.Setting;

namespace RadioApp.Services;

public class MusicNetPlatform
{

    private readonly IEnumerable<IMusicProvider> _musicProviderList;
    public MusicNetPlatform(IEnumerable<IMusicProvider> musicProviderList)
    {
        _musicProviderList = musicProviderList;
    }

    private IMusicProvider GetMusicProvider(PlatformEnum platform)
    {
        return _musicProviderList.First(x => x.Platform == platform);
    }

    public async Task<List<string>> GetHotWordAsync()
    {
        return await GetMusicProvider(PlatformEnum.Music).GetHotWordAsync();
    }

    public async Task<List<string>> GetSearchSuggestAsync(string keyword)
    {
        return await GetMusicProvider(PlatformEnum.Music).GetSearchSuggestAsync(keyword);
    }

    public async Task<List<Music>> SearchAsync(PlatformEnum platform, string keyword)
    {
        return await GetMusicProvider(platform).SearchAsync(keyword);
    }

    public async Task<string> GetPlayUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await GetMusicProvider(platform).GetPlayUrlAsync(id, extendDataJson);
    }
    public async Task<string> GetImageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await GetMusicProvider(platform).GetImageUrlAsync(id, extendDataJson);
    }

    public async Task<string> GetLyricAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return await GetMusicProvider(platform).GetLyricAsync(id, extendDataJson);
    }

    public Task<string> GetPlayPageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "")
    {
        return GetMusicProvider(platform).GetShareUrlAsync(id, extendDataJson);
    }

    public async Task<PlatformMusicTag?> GetMusicTagsAsync(PlatformEnum platform)
    {
        return await GetMusicProvider(platform).GetMusicTagsAsync();
    }

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id, int page)
    {
        return await GetMusicProvider(platform).GetSongMenusFromTagAsync(id, page);
    }
    public Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform)
    {
        return GetMusicProvider(platform).GetSongMenusFromTop();
    }

    public async Task<List<Music>> GetTopMusicsAsync(PlatformEnum platform, string topId)
    {
        return await GetMusicProvider(platform).GetTopMusicsAsync(topId);
    }

    public async Task<List<Music>> GetTagMusicsAsync(PlatformEnum platform, string tagId)
    {
        return await GetMusicProvider(platform).GetTagMusicsAsync(tagId);
    }
}
