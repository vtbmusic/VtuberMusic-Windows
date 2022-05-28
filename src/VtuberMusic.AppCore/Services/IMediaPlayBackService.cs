using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;
using Windows.Media.Playback;

namespace VtuberMusic.AppCore.Services {
    public interface IMediaPlayBackService {
        ObservableCollection<Music> Playlist { get; }

        Music NowPlaying { get; }
        double Volume { get; set; }
        MediaPlaybackState Statue { get; }
        PlaylistPlayMode PlaylistPlayMode { get; set; }
        TimeSpan Position { get; set; }
        TimeSpan Duration { get; }

        void Play();
        void Pause();
        void Stop();

        void SetCollection(IEnumerable<Music> musicCollection, int startIndex = 0);
        void SetMusic(Music music);
        void SetNextMusic(Music music);
        void Next();
        void Previous();

        event EventHandler<Music> PlayingChanged;
        event EventHandler<MediaPlaybackState> StatueChanged;
        event EventHandler<MediaPlayerFailedEventArgs> Error;
        event EventHandler<double> VolumeChanged;
        event EventHandler<TimeSpan> PositionChanged;
        event EventHandler<TimeSpan> DurationChanged;
        event EventHandler<PlaylistPlayMode> PlaylistPlayModeChanged;
    }
}