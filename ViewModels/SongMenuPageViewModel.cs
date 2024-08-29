using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RadioApp.Enums;
using RadioApp.Extensions;
using RadioApp.Models;
using RadioApp.Services;
using RadioApp.Services.Interface;
using RadioApp.Utils;
using System.Collections.ObjectModel;

namespace RadioApp.ViewModels;

[QueryProperty(nameof(Json), nameof(Json))]
[QueryProperty(nameof(PlatformString), nameof(PlatformString))]
public partial class SongMenuPageViewModel : ViewModelBase
{
    private readonly ILogger<SongMenuPageViewModel> _logger;
    public string Json { get; set; }
    public string PlatformString { get; set; }
    private PlatformEnum Platform => (PlatformEnum)Enum.Parse(typeof(PlatformEnum), PlatformString);

    private readonly IPlaylistService _playlistService;
    private readonly MusicNetPlatform _musicNetPlatform;
    private readonly MusicResultService _musicResultService;

    [ObservableProperty]
    private SongMenuViewModel _songMenu;

    [ObservableProperty]
    private ObservableCollection<MusicResultGroupViewModel> _musicResultCollection = null!;

    public SongMenuPageViewModel(MusicNetPlatform musicNetPlatform, MusicResultService musicResultService, IPlaylistService playlistService, ILogger<SongMenuPageViewModel> logger)
    {
        MusicResultCollection = new ObservableCollection<MusicResultGroupViewModel>();
        _musicNetPlatform = musicNetPlatform;
        _musicResultService = musicResultService;
        _playlistService = playlistService;
        _logger = logger;
    }
    public async Task InitializeAsync()
    {
        try
        {
            //Loading("Song load....");
            MusicResultCollection.Clear();
            SongMenu = Json.ToObject<SongMenuViewModel>() ?? throw new ArgumentNullException("Song list information does not exist");

            List<Music> musics=new List<Music>();
            for (int i = 0; i < 20; i++) {
                musics.Add( new Music()
                {
                    Id = "id" + i,
                    Album = "album" + i,
                    Artist = "Art" + i,
                    Duration = TimeSpan.Zero,
                    ExtendDataJson = "https://archive.org/download/xich-tam-tuan-thien_202302/xich-tam-tuan-thien-tap-3.mp3",
                    Fee = FeeEnum.Free,
                    ImageUrl = "https://images.ctfassets.net/hrltx12pl8hq/3Z1N8LpxtXNQhBD5EnIg8X/975e2497dc598bb64fde390592ae1133/spring-images-min.jpg",
                    Name = "name" + i,
                    Platform = PlatformEnum.Comics
                });

            }

            switch (SongMenu.SongMenuType)
            {
                case SongMenuEnum.Tag:
                   // musics = await _musicNetPlatform.GetTagMusicsAsync((NetMusicLib.Enums.PlatformEnum)Platform, SongMenu.Id);
                    break;
                case SongMenuEnum.Top:
                   // musics = await _musicNetPlatform.GetTopMusicsAsync((NetMusicLib.Enums.PlatformEnum)Platform, SongMenu.Id);
                    break;
                default:
                    throw new ArgumentNullException("Non -supported song list");
            }

            //IMusicSearchFilter vipMusicFilter = new VipMusicFilter();
            //musics = vipMusicFilter.Filter(musics);

            int seq = 0;
            var platformMusics = musics.Select(
                  x => new MusicResultShowViewModel()
                  {
                      Seq = ++seq,
                      Id = x.Id,
                      Name = x.Name,
                      Artist = x.Artist,
                      Album = x.Album,
                      Duration = x.DurationText,
                      ImageUrl = x.ImageUrl,
                      ExtendDataJson=x.ExtendDataJson,
                      Fee = "Fee"
                  }).ToList();
            MusicResultCollection.Add(new MusicResultGroupViewModel("Comic", platformMusics));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"The song is loaded and failed：{SongMenu.PlatformName},type={SongMenu.SongMenuType},id={SongMenu.Id}");
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    public async void PlayAllAsync()
    {
        if (MusicResultCollection.Count == 0)
        {
            return;
        }
        if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlaySongMenu)
        {
            if (!await _playlistService.RemoveAllAsync())
            {
                await ToastService.Show("The playlist has failed to clear");
                return;
            }
        }
        await _musicResultService.PlayAllAsync(MusicResultCollection[0].ToLocalMusics());
    }

    [RelayCommand]
    public async void PlayAsync(MusicResultShowViewModel musicResult)
    {
        await _musicResultService.PlayAsync(musicResult.ToLocalMusic());
    }
}