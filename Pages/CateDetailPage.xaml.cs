using RadioApp.ViewModels;

namespace RadioApp.Pages;

public partial class CateDetailPage : ContentPage
{
    private CateDetailViewModel viewModel => BindingContext as CateDetailViewModel;
    public CateDetailPage(CateDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}