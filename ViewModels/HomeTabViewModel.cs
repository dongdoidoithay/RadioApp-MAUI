using CommunityToolkit.Mvvm.ComponentModel;

namespace RadioApp.ViewModels;

public partial class HomeTabViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private bool _isSelected;
}
