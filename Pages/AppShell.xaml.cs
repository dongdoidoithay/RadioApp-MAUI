using RadioApp.ViewModels;

namespace RadioApp.Pages;

public partial class MobileShell : Shell
{
    public MobileShell()
    {
        InitializeComponent();
        BindingContext = new ShellViewModel();
    }
}
