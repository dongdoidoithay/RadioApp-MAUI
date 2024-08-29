using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using RadioApp.Utils;
using NativeMediaMauiLib;

namespace RadioApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("NetConfig.json").Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var netConfig = json.ToObject<NetConfig>();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseNativeMedia()
                .UseBusiness()
                .ConfigureServices()
                .ConfigurePages()
                .ConfigureViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<UpdateCheck>();
            builder.Services.AddHttpClient();
            //builder.Services.AddMusicNetPlatform();
            return builder.Build();
        }
    }
}
