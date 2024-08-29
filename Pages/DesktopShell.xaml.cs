using RadioApp.ViewModels;

namespace RadioApp.Pages;

public partial class DesktopShell : Shell
{
    public DesktopShell()
    {
        InitializeComponent();
       // BindingContext = new ShellViewModel();
    }

    private void Shell_Loaded(object sender, EventArgs e)
    {
        SetWindowSize();
    }
    private void SetWindowSize()
    {
        //16:9 scale factor = 0.5625
        //Screen width = device pixel width / scaling ratio 
        //Window width = screen width * 0.5625 
        //Screen height = device pixel height / scaling ratio 
        //Window height = window height * 0.5625
        var disp = DeviceDisplay.Current.MainDisplayInfo;

        var screenWidth = disp.Width / disp.Density;
        var screenHeight = disp.Height / disp.Density;

        var width = screenWidth * 0.6;
        // Set the minimum width of the window
        if (width < 1000)
        {
            width = 1000;
        }
        var height = width * 0.5625;

        //Center
        Window.X = (screenWidth - width) / 2;
        Window.Y = (screenHeight - height) / 2;

        //Set the window size
        Window.Width = width;
        Window.Height = height;
        Window.MinimumWidth = width;
        Window.MinimumHeight = height;
    }
}