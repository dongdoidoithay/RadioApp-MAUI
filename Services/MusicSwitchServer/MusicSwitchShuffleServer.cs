using RadioApp.Models;
using RadioApp.Services.Interface;
using RadioApp.Utils;
namespace RadioApp.Services.MusicSwitchServer;

public class MusicSwitchShuffleServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchShuffleServer(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }
    public async Task<Episode?> GetPreviousAsync(string currentMusicId)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return default;
        }
        if (playlist.Count == 1)
        {
            return playlist[0];
        }

        Episode randomPlaylist;
        do
        {
            randomPlaylist = RandomUtils.GetOneFromList<Episode>(playlist);
        } while (randomPlaylist.episodeId == currentMusicId);
        return randomPlaylist;
    }
    public async Task<Episode?> GetNextAsync(string currentMusicId)
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            return default;
        }
        if (playlist.Count == 1)
        {
            return playlist[0];
        }

        Episode randomPlaylist;
        do
        {
            randomPlaylist = RandomUtils.GetOneFromList<Episode>(playlist);
        } while (randomPlaylist.episodeId == currentMusicId);
        return randomPlaylist;
    }
}