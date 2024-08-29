using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RadioApp.Services;
using RadioApp.Services.Interface;
using RadioApp.Utils;
using System.Collections.ObjectModel;

namespace RadioApp.ViewModels;

public partial class PlayingPageViewModel : ViewModelBase
{
    private readonly ILogger<PlayingPageViewModel> _logger;
    private static readonly HttpClient MyHttpClient = new HttpClient();
    //When controlling manual scrolling lyrics, the system pauses lyrics rolling
    private DateTime _lastScrollToTime = DateTime.Now;
    private readonly MusicPlayerService _playerService;
    private readonly IDispatcherTimer _timerLyricsUpdate;
    private readonly IPlaylistService _playlistService;
    public EventHandler<LyricViewModel> ScrollToLyric { get; set; } = null!;

    /// <summary>
    /// The currently played song
    /// </summary>
    [ObservableProperty]
    private MusicResultShowViewModel? _currentMusic;

    /// <summary>
    /// The currently played song pictures
    /// </summary>
    [ObservableProperty]
    private byte[]? _currentMusicImageByteArray;

    /// <summary>
    /// Each line of lyrics
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<LyricViewModel> _lyrics = null!;

    /// <summary>
    /// Share button text
    /// </summary>
    [ObservableProperty]
    private string _shareLabelText = Config.Desktop ? "Copy the song link" : "Share song link";

    public PlayingPageViewModel(MusicPlayerService playerService, IPlaylistService playlistService, ILogger<PlayingPageViewModel> logger)
    {
        _playerService = playerService;
        Lyrics = new ObservableCollection<LyricViewModel>();

        _timerLyricsUpdate = App.Current.Dispatcher.CreateTimer();
        _timerLyricsUpdate.Interval = TimeSpan.FromMilliseconds(300);
        _playlistService = playlistService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (!Config.Desktop && GlobalConfig.MyUserSetting.Play.IsPlayingPageKeepScreenOn)
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            }

            _playerService.NewMusicAdded += _playerService_NewMusicAdded;
            _timerLyricsUpdate.Tick += _timerLyricsUpdate_Tick;
            _timerLyricsUpdate.Start();
            await NewMusicAddedDoAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("The song information that is being played failed to load");
            _logger.LogError(ex, "Play page initialization failed。");
        }
    }

    private async void _playerService_NewMusicAdded(object sender, EventArgs e)
    {
        await NewMusicAddedDoAsync();
    }
    private async Task NewMusicAddedDoAsync()
    {
        if (_playerService.Metadata == null)
        {
            return;
        }
        var playlist = await _playlistService.GetOneAsync(_playerService.Metadata.episodeId);
        if (playlist == null)
        {
            await ToastService.Show("Song information does not exist");
            return;
        }

        CurrentMusic = new MusicResultShowViewModel()
        {
            Id = playlist.episodeId,
            Name = playlist.subtitle,
            Artist = playlist.cateId,
            Album = playlist.albumId,
            ImageUrl = playlist.ImageUrl,
            ExtendDataJson = playlist.songUrl,
        };
        CurrentMusicImageByteArray = await MyHttpClient.GetByteArrayAsync(CurrentMusic.ImageUrl);
        await GetLyricDetailAsync();
    }

    /// <summary>
    /// Analysis lyrics
    /// </summary>
    private async Task GetLyricDetailAsync()
    {
        if (Lyrics.Count > 0)
        {
            Lyrics.Clear();
        }
        if (CurrentMusic == null)
        {
            return;
        }

        string lyric = await GetLyric();
        if (lyric.IsEmpty())
        {
            return;
        }

        string pattern = ".*";
        var lyricRowList = RegexUtils.GetAll(lyric, pattern);
        foreach (var lyricRow in lyricRowList)
        {
            if (lyricRow.IsEmpty())
            {
                continue;
            }
            pattern = @"\[(?<mm>\d*):(?<ss>\d*).(?<fff>\d*)\](?<lyric>.*)";
            var (success, result) = RegexUtils.GetMultiGroupInFirstMatch(lyricRow, pattern);
            if (success == false)
            {
                continue;
            }

            int totalMillisecond = Convert.ToInt32(result["mm"]) * 60 * 1000 + Convert.ToInt32(result["ss"]) * 1000 + Convert.ToInt32(result["fff"]);
            var info = result["lyric"];
            Lyrics.Add(new LyricViewModel() { PositionMillisecond = totalMillisecond, Info = info });
        }
    }

    private async Task<string> GetLyric()
    {
        if (CurrentMusic == null)
        {
            return default;
        }
        return"";// await _musicNetPlatform.GetLyricAsync((NetMusicLib.Enums.PlatformEnum)CurrentMusic.Platform, CurrentMusic.IdOnPlatform, CurrentMusic.ExtendDataJson);
    }

    [RelayCommand]
    private async void ShareMusicLinkAsync()
    {
        if (CurrentMusic == null)
        {
            return;
        }

        try
        {
            //string musicUrl = await _musicNetPlatform.GetPlayPageUrlAsync((NetMusicLib.Enums.PlatformEnum)CurrentMusic.Platform, CurrentMusic.IdOnPlatform);
            //if (Config.Desktop)
            //{
            //    await Clipboard.Default.SetTextAsync(musicUrl);
            //    await ToastService.Show($"{CurrentMusic.Name} - {CurrentMusic.Artist}{Environment.NewLine}The song link has been copied");
            //}
            //else
            //{
            //    await Share.RequestAsync(new ShareTextRequest
            //    {
            //        Uri = musicUrl,
            //        Title = $"Share song link{Environment.NewLine}{CurrentMusic.Name} - {CurrentMusic.Artist}"
            //    });
            //}
        }
        catch (Exception ex)
        {
            await ToastService.Show("Song link analysis failed");
            _logger.LogError(ex, "Copy the song link failed。");
        }
    }

    [RelayCommand]
    private void LyricsScrolledDo()
    {
        _lastScrollToTime = DateTime.Now.AddSeconds(1);
    }

    private void _timerLyricsUpdate_Tick(object sender, EventArgs e)
    {
        if (_lastScrollToTime.Subtract(DateTime.Now).TotalMilliseconds > 0)
        {
            return;
        }

        if (CurrentMusic == null)
        {
            return;
        }
        if (_playerService.IsPlaying == false)
        {
            return;
        }
        if (Lyrics.Count == 0)
        {
            return;
        }

        try
        {
            var positionMillisecond = _playerService.CurrentPosition.position.TotalMilliseconds;
            //Take the first line of indexes greater than the current progress. On this basis
            int highlightIndex = 0;
            foreach (var lyric in Lyrics)
            {
                lyric.IsHighlight = false;
                if (lyric.PositionMillisecond > positionMillisecond)
                {
                    break;
                }
                highlightIndex++;
            }
            if (highlightIndex > 0)
            {
                highlightIndex = highlightIndex - 1;
            }

            Lyrics[highlightIndex].IsHighlight = true;
            ScrollToLyric?.Invoke(this, Lyrics[highlightIndex]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Play page progress update failed。");
        }
    }

    public void OnDisappearing()
    {
        if (DeviceDisplay.Current.KeepScreenOn == true)
        {
            DeviceDisplay.Current.KeepScreenOn = false;
        }

        _playerService.NewMusicAdded -= _playerService_NewMusicAdded;
        _timerLyricsUpdate.Tick -= _timerLyricsUpdate_Tick;
        _timerLyricsUpdate.Stop();
    }
}
