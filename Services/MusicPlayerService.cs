using Microsoft.Extensions.Logging;
using RadioApp.Models;
using RadioApp.Storages;
using RadioApp.Services.Interface;
using RadioApp.Utils;
using RadioApp.Enums;
using RadioApp.Services.MusicSwitchServer;

namespace RadioApp.Services;

public class MusicPlayerService
{
    private readonly HttpClient _httpClient;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly PlayerService _playerService;
    private readonly IMusicCacheStorage _musicCacheStorage;
    private readonly IPlaylistService _playlistService;
    private readonly IMusicSwitchServerFactory _musicSwitchServerFactory;
    private readonly MusicNetPlatform _musicNetPlatform;
    public bool IsPlaying => _playerService.IsPlaying;
    public Episode? Metadata => _playerService.CurrentMetadata;
    public MusicPosition CurrentPosition => _playerService.CurrentPosition;

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;
    private readonly ILogger<MusicPlayerService> _logger;

    public MusicPlayerService(ILogger<MusicPlayerService> logger, 
        PlayerService playerService, 
        WifiOptionsService wifiOptionsService, 
        HttpClient httpClient, 
        IMusicCacheStorage musicCacheStorage,
        IPlaylistService playlistService,
        MusicNetPlatform musicNetPlatform,
        IMusicSwitchServerFactory musicSwitchServerFactory
        )
    {
        _logger = logger;
        _musicSwitchServerFactory = musicSwitchServerFactory;
        _playerService = playerService;
        _httpClient = httpClient;
        _wifiOptionsService = wifiOptionsService;
        _musicCacheStorage = musicCacheStorage;
        _playlistService = playlistService;
        _musicNetPlatform = musicNetPlatform;

        _playerService.NewMusicAdded += (_, _) => NewMusicAdded?.Invoke(this, EventArgs.Empty);
        _playerService.IsPlayingChanged += (_, _) => IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PositionChanged += (_, _) => PositionChanged?.Invoke(this, EventArgs.Empty);
        _playerService.PlayFinished += async (_, _) => await Next();
        _playerService.PlayFailed += async (_, _) => await MediaFailed();
        _playerService.PlayNext += async (_, _) => await Next();
        _playerService.PlayPrevious += async (_, _) => await Previous();


    }

    /// <summary>
    /// Play
    /// </summary>
    public async Task PlayAsync(string musicId)
    {
        var playlist = await _playlistService.GetOneAsync(musicId);
        await PlayByPlaylistAsync(playlist);
    }
    public async Task PlayEpisodeAsync(Episode music)
    {
        await PlayByPlaylistAsync(music);
    }
    private async Task PlayByPlaylistAsync(Episode playlist)
    {
        if (playlist == null)
        {
            await ToastService.Show("Play list loading failed");
            return;
        }

        var cachePath = await _musicCacheStorage.GetOrAddAsync(playlist, async (x) =>
        {
            if (!await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync())
            {
                return default;
            }

            string key = GuidUtils.GetFormatN();
            LoadingService.Loading(key, "Song loadding....");

            ///Re -obtain the playback link        
            var playUrl = x.songUrl;
            if (playUrl.IsEmpty())
            {
                LoadingService.LoadComplete(key);
                _logger.LogInformation($"Play address obtaining failure。{x.albumId}-{x.subtitle}");
                return default;
            }

            var fileExtension = GetPlayUrlFileExtension(playUrl);
            var data = await _httpClient.GetByteArrayAsync(playUrl);
            LoadingService.LoadComplete(key);
            return new MusicCacheMetadata(fileExtension, data);
        });

        if (cachePath.IsEmpty())
        {
            await Next();
        }

        var image = await _httpClient.GetByteArrayAsync(playlist.ImageUrl);
        playlist.ByteImage = image;
        await _playerService.PlayAsync(playlist);
    }
    private string GetPlayUrlFileExtension(string playUrl)
    {
        string pattern = """
            .+(?<Extension>\.\S+)\??\S*
            """;
        var (success, result) = RegexUtils.GetOneGroupInFirstMatch(playUrl, pattern);
        if (!success)
        {
            _logger.LogInformation($"Failure to parse the suffix,{playUrl}");
            return "";
        }
        return result;
    }

    /// <summary>
    /// bài trước
    /// </summary>
    public async Task Previous()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetPreviousAsync(_playerService.CurrentMetadata.episodeId);
        await PlayByPlaylistAsync(previousMusic);
    }

    /// <summary>
    /// bài tiếp theo
    /// </summary>
    public async Task Next()
    {
        var previousMusic = await _musicSwitchServerFactory.Create(GlobalConfig.MyUserSetting.Player.PlayMode).GetNextAsync(_playerService.CurrentMetadata.episodeId);
        if(previousMusic!=null)
            await PlayByPlaylistAsync(previousMusic);
    }

    private async Task MediaFailed()
    {
        await Next();
    }

    public async Task SetPlayPosition(double positionMillisecond)
    {
        await _playerService.SetPlayPosition(positionMillisecond);
    }

    public async Task SetMuted(bool value)
    {
        await _playerService.SetMuted(value);
    }

    public async Task SetVolume(int value)
    {
        await _playerService.SetVolume(value);
    }
}
