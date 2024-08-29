using CommunityToolkit.Mvvm.ComponentModel;

namespace RadioApp.ViewModels;
/// <summary>
/// Lyrics details
/// </summary>
public partial class LyricViewModel : ObservableObject
{
    /// <summary>
    /// Lyrics
    /// </summary>
    [ObservableProperty]
    private int _positionMillisecond;

    /// <summary>
    /// lyrics
    /// </summary>
    [ObservableProperty]
    private string _info = null!;

    /// <summary>
    /// Whether to show high shine
    /// </summary>
    [ObservableProperty]
    private bool _isHighlight = false;

}