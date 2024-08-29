

using RadioApp.Services;
using RadioApp.Services.Interface;

namespace RadioApp;

public static class BusinessAppBuilderExtensions
{
    public static MauiAppBuilder UseBusiness(this MauiAppBuilder builder)
    {
        //Network data platform
        builder.Services.AddSingleton<MusicNetPlatform>();
        //Local service
        builder.Services.AddSingleton<IEnvironmentConfigService, EnvironmentConfigService>();
        builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
        //builder.Services.AddSingleton<IUserService, UserService>();
        //builder.Services.AddSingleton<IMyFavoriteService, MyFavoriteService>();


        return builder;
    }
}
