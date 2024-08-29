using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using NativeMediaMauiLib.Platforms.Android;
using NativeMediaMauiLib.Platforms.Android.CurrentActivity;
using static Android.Icu.Text.CaseMap;

namespace RadioApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity, IAudioActivity
    {
        MediaPlayerServiceConnection mediaPlayerServiceConnection;
        public MediaPlayerServiceBinder Binder { get; set; }

        public event StatusChangedEventHandler StatusChanged;
        public event CoverReloadedEventHandler CoverReloaded;
        public event PlayingEventHandler Playing;
        public event BufferingEventHandler Buffering;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                CrossCurrentActivity.Current.Init(this, savedInstanceState);
                NotificationHelper.CreateNotificationChannel(ApplicationContext);
                if (mediaPlayerServiceConnection == null)
                    InitializeMedia();

                //var intent = new Intent(ApplicationContext, typeof(MediaPlayerService));
                //ApplicationContext.StartForegroundService(intent);

                //HandleIntent(intent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        private void InitializeMedia()
        {
            mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
            var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            BindService(mediaPlayerServiceIntent, mediaPlayerServiceConnection, Bind.AutoCreate);
        }


        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent); // Handle intent when activity is brought to front
        }

        private void HandleIntent(Intent intent)
        {
            if (intent.HasExtra("SeekPosition"))
            {
                int position = intent.GetIntExtra("SeekPosition", -1);
                if (position >= 0 && Binder != null)
                {
                    // Call method in MediaPlayerService to seek to position
                    Binder.GetMediaPlayerService().Seek(position);
                }
            }
        }
    }
}
