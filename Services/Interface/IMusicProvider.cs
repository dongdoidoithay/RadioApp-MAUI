
using RadioApp.Enums;
using RadioApp.Models;
using RadioApp.Setting;

namespace RadioApp.Services.Interface;
public interface IMusicProvider
{
    public PlatformEnum Platform { get; }

    /// <summary>
    /// Get music label classification
    /// </summary>
    /// <returns></returns>
    Task<PlatformMusicTag?> GetMusicTagsAsync();

    /// <summary>
    /// Get the song list corresponding to the music label
    /// </summary>
    /// <returns></returns>
    Task<List<SongMenu>> GetSongMenusFromTagAsync(string id, int page);

    /// <summary>
    /// Get label song details details
    /// </summary>
    /// <returns></returns>
    Task<List<Music>> GetTagMusicsAsync(string tagId);

    /// <summary>
    /// Get the rankings
    /// </summary>
    /// <returns></returns>
    Task<List<SongMenu>> GetSongMenusFromTop();

    /// <summary>
    /// Get the ranking list details
    /// </summary>
    /// <returns></returns>
    Task<List<Music>> GetTopMusicsAsync(string topId);

    Task<List<string>> GetHotWordAsync();
    Task<List<string>> GetSearchSuggestAsync(string keyword);
    Task<List<Music>> SearchAsync(string keyword);
    Task<string> GetPlayUrlAsync(string id, string extendDataJson = "");
    Task<string> GetImageUrlAsync(string id, string extendDataJson = "");
    Task<string> GetLyricAsync(string id, string extendDataJson = "");
    Task<string> GetShareUrlAsync(string id, string extendDataJson = "");
}