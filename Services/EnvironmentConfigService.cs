using RadioApp.Enums;
using RadioApp.Models.Entities;
using RadioApp.Services.Interface;
using RadioApp.Setting;
using RadioApp.Storages;
using RadioApp.Utils;

namespace RadioApp.Services;

public class EnvironmentConfigService : IEnvironmentConfigService
{
    public async Task<EnvironmentSetting> ReadAllSettingsAsync()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            environmentConfig = await InitializationEnvironmentSettingAsync();
        }

        var result = new EnvironmentSetting();
        var playerSetting = environmentConfig.PlayerSettingJson.ToObject<PlayerSetting>() ?? throw new Exception("The configuration information does not exist：PlayerSettingJson");
        result.Player = new PlayerSetting()
        {
            Volume = playerSetting.Volume,
            IsSoundOff = playerSetting.IsSoundOff,
            PlayMode = playerSetting.PlayMode
        };

        //通用设置
        var generalConfig = environmentConfig.GeneralSettingJson.ToObject<GeneralSetting>() ?? throw new Exception("The configuration information does not exist：GeneralSetting");
        result.General = new GeneralSetting()
        {
            IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
            AppThemeInt = generalConfig.AppThemeInt,
            IsHideWindowWhenClosed = generalConfig.IsHideWindowWhenClosed,
        };

        //播放设置
        var playConfig = environmentConfig.PlaySettingJson.ToObject<PlaySetting>() ?? throw new Exception("The configuration information does not exist：PlaySetting");
        result.Play = new PlaySetting()
        {
            IsPlayingPageKeepScreenOn = playConfig.IsPlayingPageKeepScreenOn,
            IsCleanPlaylistWhenPlaySongMenu = playConfig.IsCleanPlaylistWhenPlaySongMenu,
            IsWifiPlayOnly = playConfig.IsWifiPlayOnly
        };

        //搜索设置
        var searchConfig = environmentConfig.SearchSettingJson.ToObject<SearchSetting>() ?? throw new Exception("配置信息不存在：SearchSetting");
        result.Search = new SearchSetting()
        {
            IsHideShortMusic = searchConfig.IsHideShortMusic,
            IsMatchSearchKey = searchConfig.IsMatchSearchKey
        };

        return result;
    }

    private async Task<EnvironmentConfigEntity> InitializationEnvironmentSettingAsync()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig != null)
        {
            return environmentConfig;
        }

        environmentConfig = new EnvironmentConfigEntity()
        {
            PlayerSettingJson = (new PlayerSetting()
            {
                Volume = 50,
                IsSoundOff = false,
                PlayMode = PlayModeEnum.RepeatList
            }).ToJson(),
            GeneralSettingJson = (new GeneralSetting()
            {
                AppThemeInt = 0,
                IsAutoCheckUpdate = true,
                IsHideWindowWhenClosed = false
            }).ToJson(),
            SearchSettingJson = (new SearchSetting()
            {
                IsHideShortMusic = true,
                IsMatchSearchKey = false
            }).ToJson(),
            PlaySettingJson = (new PlaySetting()
            {
                IsPlayingPageKeepScreenOn = true,
                IsCleanPlaylistWhenPlaySongMenu = true,
                IsWifiPlayOnly = true
            }).ToJson()
        };
        try
        {
            var count = await DatabaseProvide.DatabaseAsync.InsertAsync(environmentConfig);
            if (count == 0)
            {
                throw new Exception("Initialization environment configuration failed");
            }
        }
        catch (Exception ex) { 
        Console.WriteLine(ex.ToString());
        }
        return environmentConfig;
    }

    public async Task WritePlayerSettingAsync(PlayerSetting playerSetting)
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            throw new Exception("The environmental configuration information does not exist, the playback settings fail");
        }

        environmentConfig.PlayerSettingJson = playerSetting.ToJson();
        await DatabaseProvide.DatabaseAsync.UpdateAsync(environmentConfig);
    }

    public async Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.GeneralSettingJson = generalSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.SearchSettingJson = searchSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.PlaySettingJson = playSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}
