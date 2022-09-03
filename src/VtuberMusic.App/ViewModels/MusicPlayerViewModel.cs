using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.Core.Models;
using Windows.Media.Playback;

namespace VtuberMusic.App.ViewModels {
    public class MusicPlayerViewModel : AppViewModel {
        public Music PlayingMusic { get => MediaPlaybackService.NowPlaying; }
        public MediaPlaybackState PlayerState { get => MediaPlaybackService.Statue; }
        public TimeSpan PlayerPosition { get => MediaPlaybackService.Position; set => MediaPlaybackService.Position = value; }
        public TimeSpan PlayerDuration { get => MediaPlaybackService.Duration; }
        public PlaylistPlayMode PlayerPlaylistPlayMode { get => MediaPlaybackService.PlaylistPlayMode; set => MediaPlaybackService.PlaylistPlayMode = value; }
        public ObservableCollection<Music> PlayerPlaylist => MediaPlaybackService.Playlist;
        public double Volume {
            get => MediaPlaybackService.Volume;
            set {
                MediaPlaybackService.Volume = value;
            }
        }

        public IRelayCommand NextMusicCommand { get; }
        public IRelayCommand PreviousMusicCommand { get; }
        public IRelayCommand TogglePlayingCommand { get; }
        public IRelayCommand TogglePlaylistPlayModeCommand { get; }

        public MusicPlayerViewModel() {
            NextMusicCommand = new RelayCommand(() => MediaPlaybackService.Next());
            PreviousMusicCommand = new RelayCommand(() => MediaPlaybackService.Previous());
            TogglePlaylistPlayModeCommand = new RelayCommand(() => {
                switch (MediaPlaybackService.PlaylistPlayMode) {
                    case PlaylistPlayMode.PlaylistRepeat:
                        MediaPlaybackService.PlaylistPlayMode = PlaylistPlayMode.SingleRepeat;
                        break;
                    case PlaylistPlayMode.SingleRepeat:
                        MediaPlaybackService.PlaylistPlayMode = PlaylistPlayMode.Shuffle;
                        break;
                    case PlaylistPlayMode.Shuffle:
                        MediaPlaybackService.PlaylistPlayMode = PlaylistPlayMode.PlaylistRepeat;
                        break;
                }
            });

            TogglePlayingCommand = new RelayCommand(() => {
                switch (MediaPlaybackService.Statue) {
                    case MediaPlaybackState.Paused:
                        MediaPlaybackService.Play();
                        break;
                    default:
                        MediaPlaybackService.Pause();
                        break;
                }
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackMusicChangedMessage message) {
                DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(PlayingMusic)); });
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackStatueChangedMessage message) {
                DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(PlayerState)); });
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPlaylistModeChangedMessage message) {
                DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(PlayerPlaylistPlayMode)); });
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackVolumeChangedMessage message) {
                DispatcherHelper.TryRun(delegate { OnPropertyChanged(nameof(Volume)); });
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPositionChangedMessage message) {
                DispatcherHelper.TryRun(delegate {
                    OnPropertyChanged(nameof(PlayerDuration));
                    OnPropertyChanged(nameof(PlayerPosition));
                });
            });
        }
    }
}
