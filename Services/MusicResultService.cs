

using RadioApp.Models;
using RadioApp.Services.Interface;

namespace RadioApp.Services;

public class MusicResultService
{
    private readonly MusicPlayerService _musicPlayerService;
    private readonly IPlaylistService _playlistService;
    //private readonly MusicNetPlatform _musicNetPlatform;
    public MusicResultService(IPlaylistService playlistService, MusicPlayerService musicPlayerService
        //, MusicNetPlatform musicNetPlatform
        )
    {
        _playlistService = playlistService;
        _musicPlayerService = musicPlayerService;
       // _musicNetPlatform = musicNetPlatform;
    }

    public async Task PlayAllAsync(List<Episode> musics)
    {
        await AddToPlaylistAsync(musics);
        await PlayMusicAsync(musics.First());
    }

    public async Task PlayAsync(Episode music)
    {
        //await AddToPlaylistAsync(music);
        await PlayMusicAsync(music);
    }

    private async Task AddToPlaylistAsync(Episode music)
    {
        await _playlistService.AddOrUpdateAsync(music);
    }

    private async Task AddToPlaylistAsync(List<Episode> musics)
    {
        await _playlistService.AddOrUpdateAsync(musics);
    }

    public async Task PlayMusicAsync(Episode music)
    {
        //await _musicPlayerService.PlayAsync(music.episodeId);
        await _musicPlayerService.PlayEpisodeAsync(music);
    }

}