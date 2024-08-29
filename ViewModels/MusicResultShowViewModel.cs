
using CommunityToolkit.Mvvm.ComponentModel;

namespace RadioApp.ViewModels;

public class MusicResultGroupViewModel : List<MusicResultShowViewModel>
{
    public string Name { get; private set; }

    public MusicResultGroupViewModel(string name, List<MusicResultShowViewModel> musics) : base(musics)
    {
        Name = name;
    }
}
public partial class MusicResultShowViewModel : MusicViewModel
{
    /// <summary>
    /// Serial number
    /// </summary>
    [ObservableProperty]
    private int _seq;

    /// <summary>
    /// duration
    /// </summary>
    [ObservableProperty]
    private string _duration = null!;

    /// <summary>
    ///Cost (free, VIP, etc.)
    /// </summary>
    [ObservableProperty]
    private string _fee = null!;
    /// <summary>
    /// Extended data
    /// </summary>
    public string ExtendDataJson { get; set; }
}