using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RadioApp.Enums;
using RadioApp.Models;
using RadioApp.Models.Responses;
using RadioApp.Pages;
using RadioApp.Services;
using RadioApp.Setting;
using RadioApp.Utils;
using System.Collections.ObjectModel;

namespace RadioApp.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private int _currentPage = 0;
    private string _currentRootCateId = "";
    private string _currentChildCateId = "";

    [ObservableProperty]
    private ObservableCollection<Category> _homeCates;

    [ObservableProperty]
    private ObservableCollection<Category> _homeCateChild;

    [ObservableProperty]
    private ObservableCollection<Episode> _homeEpisodes;


    private static readonly object LockPlatformMusicTags = new object();
    private static readonly Dictionary<PlatformEnum, PlatformMusicTag> PlatformMusicTags = new Dictionary<PlatformEnum, PlatformMusicTag>();

    private readonly MusicNetPlatform _musicNetPlatform;
    private readonly GetDataService _getDataService;

    private readonly SearchPage _searchPage;
    private readonly ILogger<HomePageViewModel> _logger;


    public HomePageViewModel(GetDataService getDataService,MusicNetPlatform musicNetPlatform, SearchPage searchPage, ILogger<HomePageViewModel> logger)
    {
        _logger = logger;
        _searchPage = searchPage;
        _musicNetPlatform = musicNetPlatform;
        _getDataService = getDataService;

        HomeCates = new ObservableCollection<Category>();
        HomeCateChild = new ObservableCollection<Category>();
        HomeEpisodes = new ObservableCollection<Episode>();
       

    }



    internal async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await FetchRootCateAsync();
    }
    private async Task FetchRootCateAsync()
    {
        var rootCate = await _getDataService.GetRootCate();

        //if (rootCate == null)
        //{
        //    await Shell.Current.DisplayAlert(
        //        AppResource.Error_Title,
        //        AppResource.Error_Message,
        //        AppResource.Close);
        //    return;
        //}

        if (_currentRootCateId == "")
        {
            HomeCates.Clear();
            int _index = 0;
            foreach (var category in rootCate)
            {
                if (_index == 0)
                {
                    _currentRootCateId = category.CateId;
                    category.IsSelected = true;
                }
                else
                {
                    category.IsSelected = false;
                }
                HomeCates.Add(category);
            }
            //lay phan tu dau tien
            await SelectTab(_currentRootCateId);
        }

    }
    private async Task FetchCateByParentIdAsync(string cateParentId)
    {
        var cates = await _getDataService.GetCateByParent(cateParentId);
        HomeCateChild.Clear();
        if (cates != null && cates.Count() > 0)
        {
            //add to list
            foreach (var child in cates)
            {
                child.Image = Config.APIUrl + child.Image;
                //episode.image = await imageProcessingService.ProcessRemoteImage(new Uri(episode.ImageUrl));
                HomeCateChild.Add(child);
            }
        }
        //load Episode
        // await FetchListEpisodeByCate(cateParentId);
    }

    private async Task FetchListEpisodeByCate(string cateId)
    {
        try
        {
            //Loading("loading....");
            CateEpisodes data = await _getDataService.GetEpisodeByCate(cateId, _currentPage);
            //if (data != null && data.episodes.Count() > 0)
            //{
            //    //add to list
            //    foreach (var episode in data.episodes)
            //    {
            //        //episode.image = await imageProcessingService.ProcessRemoteImage(new Uri(episode.ImageUrl));
            //        HomeEpisodes.Add(episode);
            //    }
            //}
            if (data.episodes?.Count() > 0)
            {
                HomeEpisodes.Clear();
                HomeEpisodes = new ObservableCollection<Episode>(data.episodes);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Home Epi list failed to load,Cateid={cateId}");
        }

    }



    [RelayCommand]
    private async void TabChangedAsync(string id)
    {

        await SelectTab(id);
    }
    private async Task SelectTab(string id)
    {
        foreach (var tab in HomeCates)
        {
            if (tab.CateId == id)
            {
                tab.IsSelected = true;
            }
            else
            {
                tab.IsSelected = false;
            }
        }
        await FetchCateByParentIdAsync(id);
    }


    [RelayCommand]
    private async void LoadLastPageTagSongMenusAsync()
    {
        if (_currentRootCateId.IsEmpty())
        {
            return;
        }

        try
        {
            // Loading("loading....");
            _currentPage = _currentPage + 1;
            CateEpisodes data = await _getDataService.GetEpisodeByCate(_currentRootCateId, _currentPage);
            if (data != null && data.episodes.Count() > 0)
            {
                //add to list
                foreach (var episode in data.episodes)
                {
                    //episode.image = await imageProcessingService.ProcessRemoteImage(new Uri(episode.ImageUrl));
                    HomeEpisodes.Add(episode);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"The song list rolling loading failed");
        }

    }



    [RelayCommand]
    private async void GoToDetailCateAsync(Category cate)
    {
       
        await Shell.Current.GoToAsync($"{nameof(CateDetailPage)}?IdCate={cate.CateId}&TypeCate={cate.Type}");
    }

    //[RelayCommand]
    //private async void GoToSearchPageAsync()
    //{
    //    await App.Current.MainPage.Navigation.PushAsync(_searchPage, true);
    //}
}