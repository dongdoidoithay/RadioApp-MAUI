using CommunityToolkit.Mvvm.ComponentModel;
using RadioApp.Pages;
namespace RadioApp.ViewModels;

internal partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = null!;

    [ObservableProperty]
    private string _icon = null!;

    public AppSection SearchResult { get; set; }
    public AppSection Playlist { get; set; }
    public AppSection MyFavorite { get; set; }
    public AppSection Settings { get; set; }

    public ShellViewModel()
    {
        SearchResult = new AppSection() { Title = "search", Icon = "search.png", IconDark = "search_dark.png", TargetType = typeof(SearchResultPage) };
        Playlist = new AppSection() { Title = "playlist", Icon = "playlist.png", IconDark = "playlist_dark.png", TargetType = typeof(PlaylistPage) };
        MyFavorite = new AppSection() { Title = "My song list", Icon = "heart.png", IconDark = "heart_dark.png", TargetType = typeof(MyFavoritePage) };
        Settings = new AppSection() { Title = "My", Icon = "my.png", IconDark = "my_dark.png", TargetType = typeof(SettingsPage) };
    }
}
