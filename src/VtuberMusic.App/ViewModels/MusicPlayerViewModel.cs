using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;
using Windows.Media.Playback;

namespace VtuberMusic.App.ViewModels;
public partial class MusicPlayerViewModel : ObservableRecipient {
    private readonly IMediaPlayBackService _mediaPlayBackService = Ioc.Default.GetRequiredService<IMediaPlayBackService>();

    public Music PlayingMusic => _mediaPlayBackService.NowPlaying;
    public MediaPlaybackState PlayerState => _mediaPlayBackService.Statue;
    public TimeSpan PlayerPosition { get => _mediaPlayBackService.Position; set => _mediaPlayBackService.Position = value; }
    public TimeSpan PlayerDuration => _mediaPlayBackService.Duration;
    public PlaylistPlayMode PlayerPlaylistPlayMode { get => _mediaPlayBackService.PlaylistPlayMode; set => _mediaPlayBackService.PlaylistPlayMode = value; }
    public ObservableCollection<Music> PlayerPlaylist => _mediaPlayBackService.Playlist;
    public double Volume {
        get => _mediaPlayBackService.Volume;
        set => _mediaPlayBackService.Volume = value;
    }

    [RelayCommand]
    public void NextMusic() => _mediaPlayBackService.Next();
    [RelayCommand]
    public void PreviousMusic() => _mediaPlayBackService.Previous();

    [RelayCommand]
    public void TogglePlaying() {
        switch (_mediaPlayBackService.Statue) {
            case MediaPlaybackState.Paused:
                _mediaPlayBackService.Play();
                break;
            default:
                _mediaPlayBackService.Pause();
                break;
        }
    }

    [RelayCommand]
    public void TogglePlaylistPlayMode() {
        switch (_mediaPlayBackService.PlaylistPlayMode) {
            case PlaylistPlayMode.PlaylistRepeat:
                _mediaPlayBackService.PlaylistPlayMode = PlaylistPlayMode.SingleRepeat;
                break;
            case PlaylistPlayMode.SingleRepeat:
                _mediaPlayBackService.PlaylistPlayMode = PlaylistPlayMode.Shuffle;
                break;
            case PlaylistPlayMode.Shuffle:
                _mediaPlayBackService.PlaylistPlayMode = PlaylistPlayMode.PlaylistRepeat;
                break;
        }
    }

    public MusicPlayerViewModel(IMediaPlayBackService mediaPlayBackService) {
        _mediaPlayBackService = mediaPlayBackService;

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackMusicChangedMessage message) {
            DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(this.PlayingMusic)); });
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackStatueChangedMessage message) {
            DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(this.PlayerState)); });
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPlaylistModeChangedMessage message) {
            DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(this.PlayerPlaylistPlayMode)); });
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackVolumeChangedMessage message) {
            DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(this.Volume)); });
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPositionChangedMessage message) {
            DispatcherHelper.TryRun(delegate {
                OnPropertyChanged(nameof(this.PlayerDuration));
                OnPropertyChanged(nameof(this.PlayerPosition));
            });
        });
    }
}
