﻿using RadioApp.ViewModels;

namespace RadioApp;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        //builder.Services.AddTransient<SearchPageViewModel>();
        //builder.Services.AddTransient<SearchResultPageViewModel>();
        //builder.Services.AddTransient<PlaylistPageViewModel>();
        //builder.Services.AddTransient<MyFavoritePageViewModel>();
        //builder.Services.AddTransient<MyFavoriteDetailPageViewModel>();
        builder.Services.AddTransient<PlayingPageViewModel>();

        builder.Services.AddSingleton<ShellViewModel>();
        //builder.Services.AddSingleton<SettingPageViewModel>();
        //builder.Services.AddSingleton<LoginPageViewModel>();
        //builder.Services.AddSingleton<RegisterPageViewModel>();
        //builder.Services.AddSingleton<CacheCleanViewModel>();
        //builder.Services.AddSingleton<LogPageViewModel>();
        builder.Services.AddSingleton<SongMenuPageViewModel>();
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<CateDetailViewModel>();
        //builder.Services.AddSingleton<AutoClosePageViewModel>();
        return builder;
    }
}
