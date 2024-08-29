using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
namespace RadioApp.ViewModels;

public partial class MusicTypeTagViewModel : ObservableObject
{
    [ObservableProperty]
    private string _typeName = null!;

    [ObservableProperty]
    private ObservableCollection<HomeTagViewModel> _tags;
}