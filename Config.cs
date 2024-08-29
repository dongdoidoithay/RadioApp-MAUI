namespace RadioApp;

public static class Config
{
    public static string APIUrl = $"https://api.novelfull.us";
    public static bool Desktop
    {
        get
        {
#if WINDOWS || MACCATALYST
                return true;
#else
            return false;
#endif
        }
    }

    public static bool IsDarkTheme
    {
        get
        {
            if (App.Current?.UserAppTheme == AppTheme.Unspecified)
            {
                var appTheme = Application.Current.RequestedTheme;
                return appTheme == AppTheme.Dark;
            }
            else
            {
                return App.Current?.UserAppTheme == AppTheme.Dark;
            }
        }
    }
}
