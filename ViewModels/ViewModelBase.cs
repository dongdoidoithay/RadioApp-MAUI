using CommunityToolkit.Mvvm.ComponentModel;
using RadioApp.Services;
using RadioApp.Utils;

namespace RadioApp.ViewModels;

public partial class ViewModelBase : ObservableValidator
{
    private string _loadingKey = "";

    [ObservableProperty]
    private bool _isLogin;
    public ViewModelBase()
    {

    }

    internal void Loading(string message)
    {
        //The same page is not allowed to load at the same time
        if (_loadingKey.IsNotEmpty())
        {
            return;
        }
        _loadingKey = GuidUtils.GetFormatN();
        LoadingService.Loading(_loadingKey, message);
    }
    internal void LoadComplete()
    {
        if (_loadingKey.IsEmpty())
        {
            return;
        }
        LoadingService.LoadComplete(_loadingKey);
        _loadingKey = "";
    }
}