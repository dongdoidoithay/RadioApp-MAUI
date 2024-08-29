using CommunityToolkit.Mvvm.ComponentModel;

namespace RadioApp.ViewModels;

public partial class MusicViewModel : ObservableObject
{
    /// <summary>
    /// Song ID
    /// </summary>
    [ObservableProperty]
    private string _id = null!;

    /// <summary>
    /// name of the song
    /// </summary>
    [ObservableProperty]
    private string _name = null!;

    /// <summary>
    /// Singer name
    /// </summary>
    [ObservableProperty]
    private string _artist = null!;

    /// <summary>
    /// The album name
    /// </summary>
    [ObservableProperty]
    private string _album = null!;

    /// <summary>
    /// picture
    /// </summary>
    [ObservableProperty]
    private string _imageUrl = null!;

}

