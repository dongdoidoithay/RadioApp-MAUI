using Microsoft.Extensions.Logging;
using RadioApp.Models;
using RadioApp.Utils;

namespace RadioApp;
public class UpdateCheck
{
    private readonly HttpClientHelper MyHttpClient = new HttpClientHelper();

    private readonly ILogger<UpdateCheck> _logger;
    public UpdateCheck(ILogger<UpdateCheck> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Automatic update address
    /// </summary>
    private string CheckUpdateUrl
    {
        get
        {
            if (GlobalConfig.UpdateDomain.IsEmpty())
            {
                throw new Exception("Update the server is not configured");
            }

            string osTag;
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                osTag = "windows";
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                osTag = "android";
            }
            else
            {
                throw new ArgumentException("Unwilling system type");
            }
            return $"{GlobalConfig.UpdateDomain}/{osTag}";
        }
    }

    public async Task DoAsync(bool isBackgroundCheck)
    {
        await Task.Run(async () =>
        {
            try
            {
                string json = await MyHttpClient.GetReadString(CheckUpdateUrl);
                var obj = json.ToObject<AppUpgradeInfo>();
                if (obj == null)
                {
                    if (!isBackgroundCheck)
                    {
                        await ToastService.Show("The inspection failed, the connection server failed");
                    }
                    else
                    {
                        _logger.LogError(new Exception("Failed to connect to server"), "Automatic update check failed");
                    }
                    return;
                }

                string version;
                string minVersion;
#if ANDROID
                version = obj.Version[..obj.Version.LastIndexOf(".")];
                minVersion = obj.MinVersion[..obj.MinVersion.LastIndexOf(".")];
#else
                version = obj.Version;
                minVersion = obj.MinVersion;
#endif

                var (isNeedUpdate, isAllowRun) = VersionUtils.CheckNeedUpdate(GlobalConfig.CurrentVersionString, version, minVersion);

                async void CheckUpdateInner()
                {
                    if (isNeedUpdate == false)
                    {
                        if (!isBackgroundCheck)
                        {
                            await ToastService.Show("The current version is the latest version");
                        }
                        return;
                    }

                    string message = $"Home a new version {obj.Version}";
                    if (isAllowRun)
                    {
                        bool isDoUpdate = await App.Current.MainPage.DisplayAlert("hint", message, "download", "neglect");
                        if (isDoUpdate == true)
                        {
                            try
                            {
                                await Browser.Default.OpenAsync(obj.DownloadUrl.ToUri(), BrowserLaunchMode.SystemPreferred);
                            }
                            catch (Exception ex)
                            {
                                await ToastService.Show("Start the browser failure, please try it out");
                                _logger.LogError(ex, "Failure to open the link。");
                            }
                        }
                    }
                    else
                    {
                        bool isDoUpdate = await App.Current.MainPage.DisplayAlert("The current version has been stopped", message, "Download and exit", "Exit");
                        if (isDoUpdate == true)
                        {
                            try
                            {
                                await Browser.Default.OpenAsync(obj.DownloadUrl.ToUri(), BrowserLaunchMode.External);
                            }
                            catch (Exception ex)
                            {
                                await ToastService.Show("Start the browser failure, please try it out");
                                _logger.LogError(ex, "Failure to open the link。");
                            }
                        }
                        Application.Current.Quit();
                    }
                }

                MainThread.BeginInvokeOnMainThread(CheckUpdateInner);
            }
            catch (Exception ex)
            {
                if (!isBackgroundCheck)
                {
                    await ToastService.Show($"Inspection failed：{ex.Message}");
                }
                else
                {
                    _logger.LogError(ex, "Automatic update check failure");
                }
            }
        });
    }

}