using RadioApp.Pages;
using RadioApp.Services.Interface;
using RadioApp.Storages;
using RadioApp.Utils;

namespace RadioApp
{
    public partial class App : Application
    {
        public App(IEnvironmentConfigService configService)
        {
            InitializeComponent();

            if (!Directory.Exists(GlobalConfig.AppDataDirectory))
            {
                Directory.CreateDirectory(GlobalConfig.AppDataDirectory);
            }
            if (!Directory.Exists(GlobalConfig.MusicCacheDirectory))
            {
                Directory.CreateDirectory(GlobalConfig.MusicCacheDirectory);
            }



            string deviceId = Preferences.Get("DeviceId", "");
            if (deviceId.IsEmpty())
            {
                deviceId = GuidUtils.GetFormatDefault();
                Preferences.Set("DeviceId", deviceId);
            }

            using var stream = FileSystem.OpenAppPackageFileAsync("NetConfig.json").Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var netConfig = json.ToObject<NetConfig>();

            GlobalConfig.UpdateDomain = netConfig?.UpdateDomain ?? "";
            GlobalConfig.ApiDomain = netConfig?.ApiDomain ?? "";
            GlobalConfig.CurrentVersion = AppInfo.Current.Version;
            DatabaseProvide.Initialize();

            var task = Task.Run(configService.ReadAllSettingsAsync);
            GlobalConfig.MyUserSetting = task.Result;

            App.Current.UserAppTheme = (AppTheme)GlobalConfig.MyUserSetting.General.AppThemeInt;

            if (Config.Desktop)
            {
                MainPage = new DesktopShell();
            }
            else
            {
                MainPage = new MobileShell();
            }

            //Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            //Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
            Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
            Routing.RegisterRoute(nameof(CateDetailPage), typeof(CateDetailPage));

            //Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            //Routing.RegisterRoute(nameof(CacheCleanPage), typeof(CacheCleanPage));
            //Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));
            //Routing.RegisterRoute(nameof(ChooseTagPage), typeof(ChooseTagPage));
            Routing.RegisterRoute(nameof(SongMenuPage), typeof(SongMenuPage));
            //Routing.RegisterRoute(nameof(AutoClosePage), typeof(AutoClosePage));

            //AutoCloseJob.Initialize();
        }
    }
}
