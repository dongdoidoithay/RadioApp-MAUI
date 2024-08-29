using RadioApp.HandCursorControls;
using RadioApp.ViewModels;

namespace RadioApp.Pages;

public partial class HomePage : ContentPage
{
    private bool _isFirstAppearing = true;
    //Control each time when scrolling, only one page of data is loaded
    private DateTime _lastScrollToTime = DateTime.Now;
    HomePageViewModel viewModel => BindingContext as HomePageViewModel;
    private readonly UpdateCheck _updateCheck;
    public HomePage(HomePageViewModel vm, UpdateCheck updateCheck)
    {
        InitializeComponent();
        BindingContext = vm;
        _updateCheck = updateCheck;
    }
  

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
        if (_isFirstAppearing)
        {
            _isFirstAppearing = false;
            HandCursor.Binding();
            //if (GlobalConfig.MyUserSetting.General.IsAutoCheckUpdate)
            //{
            //    //Make sure the main thread is loaded and complete
            //    await Task.Delay(5000);

            //    //Automatic update
            //    await _updateCheck.DoAsync(true);
            //}
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        //TODO In Win UI, the Remaining Items Threshold Reached Command cannot be triggered at present, so implement it yourself first
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        if (_lastScrollToTime.Subtract(DateTime.Now).TotalMilliseconds > 0)
        {
            return;
        }
        _lastScrollToTime = DateTime.Now.AddSeconds(1);

        if (sender is CollectionView cv && cv is IElementController element)
        {
            var count = element.LogicalChildren.Count;
            if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
            {
                if (cv.RemainingItemsThresholdReachedCommand.CanExecute(null))
                {
                    cv.RemainingItemsThresholdReachedCommand.Execute(null);
                }
            }
        }
    }
}