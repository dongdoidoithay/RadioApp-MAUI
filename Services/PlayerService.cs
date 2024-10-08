﻿using NativeMediaMauiLib;
using RadioApp.Models;

namespace RadioApp.Services;

public class PlayerService
{
    private readonly INativeAudioService _audioService;
    private System.Timers.Timer _timerPlayProgress;
    private bool _isBuffering = false;

    /// <summary>
    /// Whether to play
    /// </summary>
    public bool IsPlaying { get; set; }

    public Episode? CurrentMetadata { get; set; } = null!;
    public MusicPosition CurrentPosition { get; set; } = new MusicPosition();

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;
    public event EventHandler? PlayFinished;
    public event EventHandler? PlayFailed;
    public event EventHandler? PlayNext;
    public event EventHandler? PlayPrevious;
    public PlayerService(INativeAudioService audioService)
    {
        _audioService = audioService;
        _audioService.PlayFinished += (_, _) => PlayFinished?.Invoke(this, EventArgs.Empty);
        _audioService.PlayFailed += (_, _) => PlayFailed?.Invoke(this, EventArgs.Empty);

        _audioService.Played += async (_, _) => await PlayAsync(CurrentMetadata);
        _audioService.Paused += async (_, _) => await PlayAsync(CurrentMetadata);
        _audioService.SkipToNext += (_, _) => PlayNext?.Invoke(this, EventArgs.Empty);
        _audioService.SkipToPrevious += (_, _) => PlayPrevious?.Invoke(this, EventArgs.Empty);

        _timerPlayProgress = new System.Timers.Timer();
        _timerPlayProgress.Interval = 1000;
        _timerPlayProgress.Elapsed += _timerPlayProgress_Elapsed;
        _timerPlayProgress.Start();
    }

    private void _timerPlayProgress_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (!IsPlaying)
        {
            return;
        }

        CurrentPosition.position = TimeSpan.FromMilliseconds(_audioService.CurrentPositionMillisecond);
        CurrentPosition.Duration = TimeSpan.FromMilliseconds(_audioService.CurrentDurationMillisecond);
        CurrentPosition.PlayProgress = _audioService.CurrentPositionMillisecond / _audioService.CurrentDurationMillisecond;

        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Task PlayAsync(Episode metadata)
    {
        if (CurrentMetadata == null)
        {
            return PlayAsync(metadata, true, 0);
        }

        var isOtherMusic = CurrentMetadata?.episodeId != metadata.episodeId;
        var isPlaying = isOtherMusic || !_audioService.IsPlaying;
        var position = isOtherMusic ? 0 : CurrentPosition.position.TotalMilliseconds;

        return PlayAsync(metadata, isPlaying, position);
    }

    public async Task PlayAsync(Episode metadata, bool isPlaying, double position = 0)
    {
        if (_isBuffering)
        {
            return;
        }
        _isBuffering = true;

        //var isOtherMusic = CurrentMetadata?.episodeId != metadata.episodeId;
        //if (isOtherMusic)
        if(metadata != null)
        {
            CurrentMetadata = metadata;

            if (_audioService.IsPlaying)
            {
                await InternalPauseAsync();
            }

            await _audioService.InitializeAsync(metadata.songUrl, new AudioMetadata(metadata.ByteImage, metadata.subtitle, metadata.desc, metadata.albumId));

            await InternalPlayPauseAsync(isPlaying, position);

            NewMusicAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await InternalPlayPauseAsync(isPlaying, position);
        }
        _isBuffering = false;
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task InternalPlayPauseAsync(bool isPlaying, double position)
    {
        if (isPlaying)
        {
            await InternalPlayAsync(position);
        }
        else
        {
            await InternalPauseAsync();
        }
    }

    private async Task InternalPauseAsync()
    {
        await _audioService.PauseAsync();
        IsPlaying = false;
    }

    private async Task InternalPlayAsync(double positionMillisecond = 0)
    {
        await _audioService.PlayAsync(positionMillisecond);
        IsPlaying = true;
    }
    public async Task SetPlayPosition(double positionMillisecond)
    {
        await InternalPlayAsync(positionMillisecond);
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task SetMuted(bool value)
    {
        await _audioService.SetMuted(value);
    }

    public async Task SetVolume(int value)
    {
        await _audioService.SetVolume(value);
    }
}
