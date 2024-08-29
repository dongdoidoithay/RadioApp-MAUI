using CommunityToolkit.Mvvm.ComponentModel;
using RadioApp.Enums;
using System.ComponentModel;

namespace RadioApp.ViewModels;
public partial class SongMenuViewModel : ObservableObject
{

    public SongMenuEnum SongMenuType { get; set; }
    [ObservableProperty]
    private string _platformName = null!;

    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private string _linkUrl = null!;

    [ObservableProperty]
    private string _imageUrl = null!;
}