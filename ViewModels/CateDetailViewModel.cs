using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RadioApp.Models;
using RadioApp.Models.Responses;
using RadioApp.Pages;
using RadioApp.Services;
using RadioApp.Services.Interface;
using RadioApp.Utils;
using System.Collections.ObjectModel;

namespace RadioApp.ViewModels;

[QueryProperty(nameof(IdCate), nameof(IdCate))]
[QueryProperty(nameof(TypeCate), nameof(TypeCate))]
public partial class CateDetailViewModel: ViewModelBase
{
    private readonly ILogger<CateDetailViewModel> _logger;
    private readonly IPlaylistService _playlistService;
    private readonly MusicResultService _musicResultService;
    private readonly GetDataService _dataService;
    public string IdCate { get; set; }
    public string TypeCate { get; set; }
    private int CurrentPage = 0;


    [ObservableProperty]
    Category cate;

    [ObservableProperty]
    Episode episodeForPlaying;

    [ObservableProperty]
    ObservableCollection<Episode> episodes;

    public CateDetailViewModel(ILogger<CateDetailViewModel> logger, IPlaylistService playlistService,
        MusicResultService musicResultService, GetDataService dataService)
    {
        _logger = logger;
        _playlistService = playlistService;
        _musicResultService = musicResultService;
        _dataService = dataService;

        episodes = new ObservableCollection<Episode>();
    }

    internal async Task InitializeAsync()
    {
        await FetchListEpisodeByCate();
    }
    private async Task FetchListEpisodeByCate()
    {
        string key = GuidUtils.GetFormatN();
        try
        {
           
            LoadingService.Loading(key, "Data loadding....");
            CateEpisodes data = await _dataService.GetEpisodeByCate(IdCate,CurrentPage);

            if (data == null)
            {
                await ToastService.Show("No Data");
              
                LoadingService.LoadComplete(key);
                return;
            }
            if (data.episodes?.Count() > 0)
            {
                Episodes.Clear();
                Episodes = new ObservableCollection<Episode>(data.episodes);
                if (data.cate != null)
                {
                    data.cate.image = Config.APIUrl + data.cate.image;
                    Cate = new Category(data.cate);
                }
                LoadingService.LoadComplete(key);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Play page initialization failed。");
            await ToastService.Show("Data failed to load");
            LoadingService.LoadComplete(key);
        }

    }

    [RelayCommand]
    Task LoadNextPageAsyncCommand()
    {
        //if (_currentTagId.IsEmpty())
        //{
        //    return;
        //}

        try
        {
            // Loading("loading....");
            var page = CurrentPage + 1;
            //var songMenus = await _musicNetPlatform.GetSongMenusFromTagAsync((NetMusicLib.Enums.PlatformEnum)Platform, _currentTagId, page);
            //_currentPage = page;
            //foreach (var songMenu in songMenus)
            //{
            //    SongMenus.Add(new SongMenuViewModel()
            //    {
            //        SongMenuType = SongMenuEnum.Tag,
            //        PlatformName = "xxx",
            //        Id = songMenu.Id,
            //        Name = songMenu.Name,
            //        ImageUrl = songMenu.ImageUrl,
            //        LinkUrl = songMenu.LinkUrl
            //    });
            //}
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, $"The song list rolling loading failed,id={_currentTagId}");
        }
        finally
        {
            LoadComplete();
        }
        return Task.FromResult(0);
    }



    [RelayCommand]
    void SearchEpisode()
    {
        //var episodesList = cate.Episodes
        //    .Where(ep => ep.subtitle.Contains(TextToSearch, StringComparison.InvariantCultureIgnoreCase))
        //    .ToList();
        //Episodes.ReplaceRange(episodesList);
    }

    [RelayCommand]
    //Task TapEpisode(Episode episode) => Shell.Current.GoToAsync($"{nameof(SongMenuPage)}?Id={episode.episodeId}&ShowId={IdCate}");
    Task TapEpisode(Episode episode) => _musicResultService.PlayMusicAsync(episode);
    [RelayCommand]
    async Task Subscribe()
    {
        //if (Show is null || subscriptionsService is null)
        //    return;

        //if (Show.IsSubscribed)
        //{
        //    var isUnsubcribe = await subscriptionsService.UnSubscribeFromShowAsync(Show.Show);
        //    Show.IsSubscribed = !isUnsubcribe;
        //}
        //else
        //{
        //    subscriptionsService.SubscribeToShow(Show.Show);
        //    Show.IsSubscribed = true;
        //}
    }

    [RelayCommand]
    Task PlayEpisode(Episode episode) => _musicResultService.PlayMusicAsync(episode);

    [RelayCommand]
    Task AddToListenLater(Episode episode)
    {
        //var itemHasInListenLaterList = listenLaterService.IsInListenLater(episode);
        //if (itemHasInListenLaterList)
        //{
        //    listenLaterService.Remove(episode);
        //}
        //else
        //{
        //    listenLaterService.Add(episode, Show.Show);
        //}

        // episode.IsInListenLater = !itemHasInListenLaterList;

        return Task.CompletedTask;
    }
}
