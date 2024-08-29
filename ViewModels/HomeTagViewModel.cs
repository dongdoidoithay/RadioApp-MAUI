using CommunityToolkit.Mvvm.ComponentModel;

namespace RadioApp.ViewModels;

public partial class HomeTagViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private bool _isSelected;
}
