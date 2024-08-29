using RadioApp.Models;
using RadioApp.Services.Interface;
namespace RadioApp.Services.MusicSwitchServer;

public class MusicSwitchRepeatListServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchRepeatListServer(IPlaylistService playlistService)
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

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].episodeId == currentMusicId)
            {
                nextId = i - 1;
                break;
            }
        }
        //The first song of the list
        if (nextId < 0)
        {
            nextId = playlist.Count - 1;
        }
        return playlist[nextId];
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

        int nextId = 0;
        for (int i = 0; i < playlist.Count; i++)
        {
            if (playlist[i].episodeId == currentMusicId)
            {
                nextId = i + 1;
                break;
            }
        }
        //Last song
        if (playlist.Count == nextId)
        {
            nextId = 0;
        }
        return playlist[nextId];
    }
}