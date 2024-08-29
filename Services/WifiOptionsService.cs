using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioApp.Services;

public class WifiOptionsService
{
    public async Task<bool> HasWifiOrCanPlayWithOutWifiAsync()
    {
        if (Config.Desktop)
        {
            return true;
        }

        var canPlayMusic = false;
        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("mistake", "Fail to play：Network Unavailable", "closure");
        }
        else
        {
            var profiles = Connectivity.ConnectionProfiles;
            var hasWifi = profiles.Contains(ConnectionProfile.WiFi);

            if (!GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly || hasWifi)
            {
                canPlayMusic = true;
            }
            else
            {
                canPlayMusic = await Shell.Current.DisplayAlert("hint", "Currently a non -WiFi environment，确定用流量播放吗？", "Allow this time", "Cancel");
            }
        }
        return canPlayMusic;
    }
}
