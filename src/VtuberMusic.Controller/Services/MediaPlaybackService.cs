using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.Core.Models;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace VtuberMusic.AppCore.Services {
    public partial class MediaPlaybackService : IMediaPlayBackService {
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private SystemMediaTransportControls SMTC;

        private Music nowPlaying { get; set; }
        private string _baseUrl = "https://api.aqua.chat/v2/song/url/media/";

        public ObservableCollection<Music> Playlist { get; } = new ObservableCollection<Music>();
        private List<Music> originPlaylist = new List<Music>();

        public Music NowPlaying {
            get => nowPlaying;
            set {
                nowPlaying = value;
                PlayingChanged?.Invoke(this, nowPlaying);
                WeakReferenceMessenger.Default.Send(new PlaybackMusicChangedMessage(value));
            }
        }

        public double Volume {
            get => _mediaPlayer.Volume;
            set {
                _mediaPlayer.Volume = value;
            }
        }

        public MediaPlaybackState Statue => _mediaPlayer.PlaybackSession.PlaybackState;
        private PlaylistPlayMode _PlaylistPlayMode { get; set; }
        public PlaylistPlayMode PlaylistPlayMode {
            get { return _PlaylistPlayMode; }
            set {
                _PlaylistPlayMode = value;
                PlaylistPlayModeChanged?.Invoke(this, value);
                WeakReferenceMessenger.Default.Send(new PlaybackPlaylistModeChangedMessage(value));
            }
        }

        public TimeSpan Position { get => _mediaPlayer.PlaybackSession.Position; set => _mediaPlayer.PlaybackSession.Position = value; }
        public TimeSpan Duration { get => _mediaPlayer.PlaybackSession.NaturalDuration; }

        public event EventHandler<PlaylistPlayMode> PlaylistPlayModeChanged;
        public event EventHandler<Music> PlayingChanged;
        public event EventHandler<MediaPlaybackState> StatueChanged;
        public event EventHandler<MediaPlayerFailedEventArgs> Error;
        public event EventHandler<double> VolumeChanged;
        public event EventHandler<TimeSpan> PositionChanged;
        public event EventHandler<TimeSpan> DurationChanged;

        public MediaPlaybackService() {
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;

            _mediaPlayer.MediaFailed += _mediaPlayer_MediaFailed;
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
            _mediaPlayer.CurrentStateChanged += _mediaPlayer_CurrentStateChanged;
            _mediaPlayer.VolumeChanged += _mediaPlayer_VolumeChanged;
            _mediaPlayer.PlaybackSession.NaturalDurationChanged += PlaybackSession_NaturalDurationChanged;
            _mediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;

            _mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.NextReceived += CommandManager_NextReceived;

            _mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PreviousReceived += CommandManager_PreviousReceived;

            _mediaPlayer.CommandManager.AutoRepeatModeBehavior.EnablingRule = MediaCommandEnablingRule.Never;
            _mediaPlayer.CommandManager.ShuffleBehavior.EnablingRule = MediaCommandEnablingRule.Never;

            PlaylistPlayModeChanged += MediaPlaybackService_PlaylistPlayModeChanged;

            SMTC = _mediaPlayer.SystemMediaTransportControls;
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args) {
            WeakReferenceMessenger.Default.Send(new PlaybackPositionChangedMessage(new PlaybackPositionChangedMessageData(Position, Duration)));
            PositionChanged?.Invoke(this, Position);
        }

        private void PlaybackSession_NaturalDurationChanged(MediaPlaybackSession sender, object args) {
            WeakReferenceMessenger.Default.Send(new PlaybackPositionChangedMessage(new PlaybackPositionChangedMessageData(Position, Duration)));
            DurationChanged?.Invoke(this, Duration);
        }

        private void MediaPlaybackService_PlaylistPlayModeChanged(object sender, PlaylistPlayMode e) {
            if (e == PlaylistPlayMode.Shuffle) {
                shuffle(Playlist);
            } else {
                Playlist.Clear();
                foreach (var item in originPlaylist) {
                    Playlist.Add(item);
                }
            }
        }

        private void CommandManager_PreviousReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args) => Previous();
        private void CommandManager_NextReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args) => Next();

        private void _mediaPlayer_VolumeChanged(MediaPlayer sender, object args) {
            VolumeChanged?.Invoke(this, sender.Volume);
            WeakReferenceMessenger.Default.Send(new PlaybackVolumeChangedMessage(sender.Volume));
        }

        private void _mediaPlayer_CurrentStateChanged(MediaPlayer sender, object args) {
            StatueChanged?.Invoke(this, Statue);
            WeakReferenceMessenger.Default.Send(new PlaybackStatueChangedMessage(Statue));
        }

        private void _mediaPlayer_MediaEnded(MediaPlayer sender, object args) {
            switch (PlaylistPlayMode) {
                case PlaylistPlayMode.SingleRepeat:
                    SetMusic(NowPlaying);
                    break;
                default: Next(); break;
            }
        }
        private void _mediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args) => Error?.Invoke(this, args);

        public void Pause() => _mediaPlayer.Pause();
        public void Play() => _mediaPlayer.Play();

        public void Next() {
            if (Playlist.Contains(NowPlaying)) {
                if (Playlist.IndexOf(NowPlaying) + 1 == Playlist.Count) {
                    SetMusic(Playlist.First());
                } else {
                    SetMusic(Playlist[Playlist.IndexOf(NowPlaying) + 1]);
                }
            } else {
                if (Playlist.Count != 0) {
                    SetMusic(Playlist.First());
                }
            }
        }

        public void Previous() {
            if (Playlist.Contains(NowPlaying)) {
                if (Playlist.IndexOf(NowPlaying) - 1 < 0) {
                    SetMusic(Playlist.Last());
                } else {
                    SetMusic(Playlist[Playlist.IndexOf(NowPlaying) - 1]);
                }
            } else {
                if (Playlist.Count != 0) {
                    SetMusic(Playlist.First());
                }
            }
        }

        public void Stop() {
            _mediaPlayer.Source = null;
            NowPlaying = null;
        }

        public void SetCollection(IEnumerable<Music> musicCollection, int startIndex = 0) {
            Playlist.Clear();
            originPlaylist.Clear();
            foreach (var music in musicCollection) {
                Playlist.Add(music);
                originPlaylist.Add(music);
            }

            if (PlaylistPlayMode == PlaylistPlayMode.Shuffle) shuffle(Playlist);
            if (Playlist.Count > startIndex) SetMusic(Playlist[startIndex]);
        }

        public void SetMusic(Music music) {
            if (!Playlist.Contains(music)) {
                Playlist.Add(music);
                originPlaylist.Add(music);

                if (PlaylistPlayMode == PlaylistPlayMode.Shuffle) shuffle(Playlist);
            }

            NowPlaying = music;

            var mediaSource = MediaSource.CreateFromUri(new Uri(_baseUrl + music.id));
            var mediaPlaybackItem = new MediaPlaybackItem(mediaSource);
            mediaPlaybackItem.ApplyDisplayProperties(updateSMTC(mediaPlaybackItem.GetDisplayProperties()));

            _mediaPlayer.Source = mediaPlaybackItem;
            Play();
        }

        private MediaItemDisplayProperties updateSMTC(MediaItemDisplayProperties properties) {
            properties.Type = MediaPlaybackType.Music;

            properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(NowPlaying.picUrl));
            properties.MusicProperties.Title = NowPlaying.name;
            properties.MusicProperties.Artist = MusicHelepr.GetArtistString(NowPlaying.artists);

            return properties;
        }

        private void shuffle<T>(IList<T> collection) {
            var _random = new Random(DateTimeOffset.Now.Minute + DateTimeOffset.Now.Second);
            int n = collection.Count;
            for (int i = 0; i < (n - 1); i++) {
                int r = i + _random.Next(n - i);
                T t = collection[r];
                collection[r] = collection[i];
                collection[i] = t;
            }
        }

        public void SetNextMusic(Music music) {
            var index = Playlist.IndexOf(NowPlaying) + 1;
            if (index == Playlist.Count) index = 0;

            if (Playlist.Any(m => m.id == music.id)) {
                Playlist.Remove(Playlist.Where(m => m.id == music.id).First());
            }

            Playlist.Insert(index, music);

            if (NowPlaying == null) Next();
        }
    }
}
