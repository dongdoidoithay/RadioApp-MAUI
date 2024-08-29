using RadioApp.Models;
using RadioApp.Services.Interface;

namespace RadioApp.Services.MusicSwitchServer;

public class MusicSwitchRepeatOneServer : IMusicSwitchServer
{
    private readonly IPlaylistService _playlistService;
    public MusicSwitchRepeatOneServer(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public async Task<Episode> GetPreviousAsync(string currentMusicId)
    {
        return await _playlistService.GetOneAsync(currentMusicId);
    }

    public async Task<Episode> GetNextAsync(string currentMusicId)
    {
        return await _playlistService.GetOneAsync(currentMusicId);
    }
}