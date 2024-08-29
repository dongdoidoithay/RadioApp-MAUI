using RadioApp.HandCursorControls;
using RadioApp.Enums;
using RadioApp.Services;
using RadioApp.Services.Interface;
using RadioApp.Models;
using RadioApp.ViewModels;
using RadioApp.Pages;

namespace RadioApp.Controls;

public partial class Player : ContentView
{
    public static readonly BindableProperty IsPlayingPageProperty =
        BindableProperty.Create(
            nameof(IsPlayingPage),
            typeof(bool),
            typeof(Player),
            false);
    public bool IsPlayingPage
    {
        get { return (bool)GetValue(IsPlayingPageProperty); }
        set { SetValue(IsPlayingPageProperty, value); }
    }

    private MusicPlayerService _playerService = null!;
    private MusicResultService _musicResultService = null!;
    private IPlaylistService _playlistService = null!;
    private IEnvironmentConfigService _configService = null!;
    public Player()
    {
        InitializeComponent();
        this.IsVisible = false;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        HandCursor.Binding();

        if (_musicResultService == null)
        {
            _musicResultService = this.Handler.MauiContext.Services.GetRequiredService<MusicResultService>();
        }
        if (_playlistService == null)
        {
            _playlistService = this.Handler.MauiContext.Services.GetRequiredService<IPlaylistService>();
        }
        if (_configService == null)
        {
            _configService = this.Handler.MauiContext.Services.GetRequiredService<IEnvironmentConfigService>();
        }
        if (_playerService == null)
        {
            _playerService = this.Handler.MauiContext.Services.GetRequiredService<MusicPlayerService>();
            InitPlayer();
        }

        if (Config.Desktop)
        {
            MainBlock.HeightRequest = 90;
        }
        else
        {
            MainBlock.HeightRequest = IsPlayingPage ? 110 : 90;
            var miniPlayer = this.FindByName<StackLayout>("PhoneMiniPlayer");
            var fullPlayer = this.FindByName<StackLayout>("PhoneFullPlayer");

            if (miniPlayer != null && fullPlayer != null)
            {
                miniPlayer.IsVisible = !IsPlayingPage;
                fullPlayer.IsVisible = IsPlayingPage;
            }
            //PhoneMiniPlayer.IsVisible = !IsPlayingPage;
            //PhoneFullPlayer.IsVisible = IsPlayingPage;

        }
    }

    internal void OnAppearing()
    {
        InitPlayer();
    }

    void InitPlayer()
    {
        if (_playerService == null)
        {
            return;
        }

        _playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded += playerService_NewMusicAdded;
        _playerService.PositionChanged += _playerService_PositionChanged;

        UpdateCurrentMusic();
        UpdateRepeatModel();
        if (Config.Desktop)
        {
            UpdateSoundOnOff().Wait();
            UpdateVolume().Wait();
        }
        if (!IsPlayingPage && !Config.IsDarkTheme)
        {
        
            BindingContext = new
            {
                ImgBack = "back.png",
                ImgNext = "next.png",
                LblMusicInfo = Color.FromArgb("#262626"),
                LblPosition = Color.FromArgb("#262626"),
                LblProgressSplit = Color.FromArgb("#262626"),
                MinimumTrackColor = Color.FromArgb("#262626"),
                MaximumTrackColor = Color.FromArgb("#717171"),
                ThumbColor = Color.FromArgb("#C98FFF")
            };

        }
        else
        {
            BindingContext = new
            {
                ImgBack = "back_dark.png",
                ImgNext = "next_dark.png",
                LblMusicInfo = Color.FromArgb("#FCF2F7"),
                LblPosition = Color.FromArgb("#FCF2F7"),
                LblProgressSplit = Color.FromArgb("#FCF2F7"),
                MinimumTrackColor = Color.FromArgb("#FFFFFF"),
                MaximumTrackColor = Color.FromArgb("#FCF2F7"),
                ThumbColor = Color.FromArgb("#C98FFF")
            };
        }
    }

    private void PlayerService_IsPlayingChanged(object? sender, EventArgs e)
    {
        IsPlayingChangedDo(_playerService.IsPlaying);
    }
    private void IsPlayingChangedDo(bool isPlaying)
    {
        string playImagePath;
        if (!IsPlayingPage && !Config.IsDarkTheme)
        {
            playImagePath = isPlaying ? "pause.png" : "play.png";
        }
        else
        {
            playImagePath = isPlaying ? "pause_dark.png" : "play_dark.png";
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.IsVisible = true;
            var _imagePlay = this.FindByName<Image>("ImgPlay");
            _imagePlay.Source = playImagePath;
            ToolTipProperties.SetText(_imagePlay, isPlaying ? "pause" : "Play");
        });
    }

    private void playerService_NewMusicAdded(object? sender, EventArgs e)
    {
        NewMusicAddedDo(_playerService.Metadata);
    }

    private void NewMusicAddedDo(Episode metadata)
    {
        var task = Task.Run(async () => await _playlistService.GetOneAsync(metadata.episodeId));
        var playlist = task.Result;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (playlist != null)
            {
                //FavoriteView.Music = new MusicResultShowViewModel()
                //{
                //    Id = playlist.Id,
                //    Name = playlist.Name,
                //    Album = playlist.Album,
                //    Artist = playlist.Artist,
                //    ExtendDataJson = playlist.ExtendDataJson,
                //    ImageUrl = playlist.ImageUrl
                //};
            }
            var _imageCurrent = this.FindByName<Image>("ImgCurrentMusic");
            _imageCurrent.Source = ImageSource.FromStream(
                () => new MemoryStream(metadata.ByteImage)
            );
            var _LblMusicInfo = this.FindByName<Label>("LblMusicInfo");

            _LblMusicInfo.Text = $"{metadata.subtitle} - {metadata.desc}";
        });
    }

    private void _playerService_PositionChanged(object? sender, EventArgs e)
    {
        PositionChangedDo(_playerService.CurrentPosition);
    }

    private void PositionChangedDo(MusicPosition position)
    {
        var _LblPosition = this.FindByName<Label>("LblPosition");
        var _LblDuration = this.FindByName<Label>("LblDuration");
        var _SliderPlayProgress = this.FindByName<Slider>("SliderPlayProgress");
        MainThread.BeginInvokeOnMainThread(() =>
        {


            _LblPosition.Text = $"{position.position.Minutes:D2}:{position.position.Seconds:D2}";
            _LblDuration.Text = $"{position.Duration.Minutes:D2}:{position.Duration.Seconds:D2}";

            if (!_isPlayProgressDragging)
            {
                _SliderPlayProgress.Value = position.PlayProgress;
            }
        });
    }

    private async void ImgPlay_Tapped(object sender, EventArgs e)
    {
        //await _playerService.PlayAsync(_playerService.Metadata.episodeId);
    }

    private async void ImgSoundOff_Tapped(object sender, EventArgs e)
    {
        GlobalConfig.MyUserSetting.Player.IsSoundOff = !GlobalConfig.MyUserSetting.Player.IsSoundOff;
        await WritePlayerSettingAsync();
        await UpdateSoundOnOff();
    }

    private void UpdateCurrentMusic()
    {
        if (_playerService.Metadata == null)
        {
            return;
        }

        NewMusicAddedDo(_playerService.Metadata);
        PositionChangedDo(_playerService.CurrentPosition);
        IsPlayingChangedDo(_playerService.IsPlaying);
    }

    private async Task UpdateSoundOnOff()
    {
        string imagePath;
        if (!IsPlayingPage && !Config.IsDarkTheme)
        {
            imagePath = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off.png" : "sound_on.png";
        }
        else
        {
            imagePath = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off_dark.png" : "sound_on_dark.png";
        }
        var _ImgSoundOff = this.FindByName<Image>("ImgSoundOff");
        _ImgSoundOff.Source = imagePath;
        await _playerService.SetMuted(GlobalConfig.MyUserSetting.Player.IsSoundOff);
    }
    private async Task UpdateVolume()
    {
        var _SliderVolume = this.FindByName<Slider>("SliderVolume");
        _SliderVolume.Value = GlobalConfig.MyUserSetting.Player.Volume;
        await _playerService.SetVolume((int)GlobalConfig.MyUserSetting.Player.Volume);
    }

    private async void ImgRepeat_Tapped(object sender, EventArgs e)
    {
        SetNextRepeatMode();
        UpdateRepeatModel();
        await WritePlayerSettingAsync();
    }

    private void SetNextRepeatMode()
    {
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatList;
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.Shuffle;
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatOne; 
            return;
        }
    }
    private void UpdateRepeatModel()
    {
        var _ImgRepeat = this.FindByName<Image>("ImgRepeat");
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            
            ToolTipProperties.SetText(_ImgRepeat, "Single cycle");
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                _ImgRepeat.Source = "repeat_one.png";
            }
            else
            {
                _ImgRepeat.Source = "repeat_one_dark.png";
            }
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            ToolTipProperties.SetText(_ImgRepeat, "List cycle");
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                _ImgRepeat.Source = "repeat_list.png";
            }
            else
            {
                _ImgRepeat.Source = "repeat_list_dark.png";
            }
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            ToolTipProperties.SetText(_ImgRepeat, "Shuffle Playback");
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                _ImgRepeat.Source = "shuffle.png";
            }
            else
            {
                _ImgRepeat.Source = "shuffle_dark.png";
            }
            return;
        }
    }

    private async Task WritePlayerSettingAsync()
    {
        if (_configService == null)
        {
            return;
        }
        await _configService.WritePlayerSettingAsync(GlobalConfig.MyUserSetting.Player);
    }

    private async void Previous_Tapped(object sender, EventArgs e)
    {
        await _playerService.Previous();
    }

    private async void Next_Tapped(object sender, EventArgs e)
    {
        await _playerService.Next();
    }

    private async void SliderVolume_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int volume = (int)e.NewValue;
        await _playerService.SetVolume(volume);
        GlobalConfig.MyUserSetting.Player.Volume = volume;
        await WritePlayerSettingAsync();
    }

    private bool _isPlayProgressDragging;
    private void SliderPlayProgress_DragStarted(object sender, EventArgs e)
    {
        _isPlayProgressDragging = true;
    }
    private async void SliderPlayProgress_DragCompleted(object sender, EventArgs e)
    {
        if (_playerService.Metadata != null)
        {
            var sliderPlayProgress = sender as Slider;
            if (sliderPlayProgress != null)
            {
                var positionMillisecond = _playerService.CurrentPosition.Duration.TotalMilliseconds * sliderPlayProgress.Value;
                await _playerService.SetPlayPosition(positionMillisecond);
            }
        }
        _isPlayProgressDragging = false;
    }

    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
        _playerService.PositionChanged -= _playerService_PositionChanged;
        Navigation.PopAsync(true);
    }

    private async void GoToPlayingPage_Tapped(object sender, EventArgs e)
    {
        var vm = this.Handler.MauiContext.Services.GetRequiredService<PlayingPageViewModel>();
        await Navigation.PushAsync(new PlayingPage(vm), true);
    }
}