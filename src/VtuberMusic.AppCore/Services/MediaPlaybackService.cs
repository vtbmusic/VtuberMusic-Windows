using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace VtuberMusic.AppCore.Services;
public partial class MediaPlaybackService : IMediaPlayBackService {
    private readonly MediaPlayer _mediaPlayer = new();
    private readonly SystemMediaTransportControls SMTC;

    private Music nowPlaying { get; set; }

    public ObservableCollection<Music> Playlist { get; } = new ObservableCollection<Music>();
    private readonly List<Music> originPlaylist = new();
    private readonly IVtuberMusicService _vtuberMusicService;
    public Music NowPlaying {
        get => this.nowPlaying;
        set {
            this.nowPlaying = value;
            PlayingChanged?.Invoke(this, this.nowPlaying);
            _ = WeakReferenceMessenger.Default.Send(new PlaybackMusicChangedMessage(value));
        }
    }

    public double Volume {
        get => _mediaPlayer.Volume;
        set => _mediaPlayer.Volume = value;
    }

    public MediaPlaybackState Statue => _mediaPlayer.PlaybackSession.PlaybackState;
    private PlaylistPlayMode _PlaylistPlayMode { get; set; }
    public PlaylistPlayMode PlaylistPlayMode {
        get => this._PlaylistPlayMode;
        set {
            this._PlaylistPlayMode = value;
            PlaylistPlayModeChanged?.Invoke(this, value);
            _ = WeakReferenceMessenger.Default.Send(new PlaybackPlaylistModeChangedMessage(value));
        }
    }

    public TimeSpan Position { get => _mediaPlayer.PlaybackSession.Position; set => _mediaPlayer.PlaybackSession.Position = value; }
    public TimeSpan Duration => _mediaPlayer.PlaybackSession.NaturalDuration;

    public event EventHandler<PlaylistPlayMode> PlaylistPlayModeChanged;
    public event EventHandler<Music> PlayingChanged;
    public event EventHandler<MediaPlaybackState> StatueChanged;
    public event EventHandler<MediaPlayerFailedEventArgs> Error;
    public event EventHandler<double> VolumeChanged;
    public event EventHandler<TimeSpan> PositionChanged;
    public event EventHandler<TimeSpan> DurationChanged;

    public MediaPlaybackService(IVtuberMusicService vtuberMusicService) {
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
        _vtuberMusicService = vtuberMusicService;
    }

    private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args) {
        _ = WeakReferenceMessenger.Default.Send(new PlaybackPositionChangedMessage(new PlaybackPositionChangedMessageData(this.Position, this.Duration)));
        PositionChanged?.Invoke(this, this.Position);
    }

    private void PlaybackSession_NaturalDurationChanged(MediaPlaybackSession sender, object args) {
        _ = WeakReferenceMessenger.Default.Send(new PlaybackPositionChangedMessage(new PlaybackPositionChangedMessageData(this.Position, this.Duration)));
        DurationChanged?.Invoke(this, this.Duration);
    }

    private void MediaPlaybackService_PlaylistPlayModeChanged(object sender, PlaylistPlayMode e) {
        if (e == PlaylistPlayMode.Shuffle) {
            shuffle(this.Playlist);
        } else {
            this.Playlist.Clear();
            foreach (var item in originPlaylist) {
                this.Playlist.Add(item);
            }
        }
    }

    private void CommandManager_PreviousReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args) => Previous();

    private void CommandManager_NextReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args) => Next();

    private void _mediaPlayer_VolumeChanged(MediaPlayer sender, object args) {
        VolumeChanged?.Invoke(this, sender.Volume);
        _ = WeakReferenceMessenger.Default.Send(new PlaybackVolumeChangedMessage(sender.Volume));
    }

    private void _mediaPlayer_CurrentStateChanged(MediaPlayer sender, object args) {
        StatueChanged?.Invoke(this, this.Statue);
        _ = WeakReferenceMessenger.Default.Send(new PlaybackStatueChangedMessage(this.Statue));
    }

    private void _mediaPlayer_MediaEnded(MediaPlayer sender, object args) {
        switch (this.PlaylistPlayMode) {
            case PlaylistPlayMode.SingleRepeat:
                SetMusic(this.NowPlaying);
                break;
            default: Next(); break;
        }
    }
    private void _mediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args) => Error?.Invoke(this, args);

    public void Pause() => _mediaPlayer.Pause();

    public void Play() => _mediaPlayer.Play();

    public void Next() {
        if (this.Playlist.Contains(this.NowPlaying)) {
            if (this.Playlist.IndexOf(this.NowPlaying) + 1 == this.Playlist.Count) {
                SetMusic(this.Playlist.First());
            } else {
                SetMusic(this.Playlist[this.Playlist.IndexOf(this.NowPlaying) + 1]);
            }
        } else {
            if (this.Playlist.Count != 0) {
                SetMusic(this.Playlist.First());
            }
        }
    }

    public void Previous() {
        if (this.Playlist.Contains(this.NowPlaying)) {
            if (this.Playlist.IndexOf(this.NowPlaying) - 1 < 0) {
                SetMusic(this.Playlist.Last());
            } else {
                SetMusic(this.Playlist[this.Playlist.IndexOf(this.NowPlaying) - 1]);
            }
        } else {
            if (this.Playlist.Count != 0) {
                SetMusic(this.Playlist.First());
            }
        }
    }

    public void Stop() {
        _mediaPlayer.Source = null;
        this.NowPlaying = null;
    }

    public void SetCollection(IEnumerable<Music> musicCollection, int startIndex = 0) {
        this.Playlist.Clear();
        originPlaylist.Clear();
        foreach (var music in musicCollection) {
            this.Playlist.Add(music);
            originPlaylist.Add(music);
        }

        if (this.PlaylistPlayMode == PlaylistPlayMode.Shuffle) {
            shuffle(this.Playlist);
        }

        if (this.Playlist.Count > startIndex) {
            SetMusic(this.Playlist[startIndex]);
        }
    }

    public async void SetMusic(Music music) {
        if (!this.Playlist.Contains(music)) {
            this.Playlist.Add(music);
            originPlaylist.Add(music);

            if (this.PlaylistPlayMode == PlaylistPlayMode.Shuffle) {
                shuffle(this.Playlist);
            }
        }

        this.NowPlaying = music;

        var songUrl = await _vtuberMusicService.GetSongUrl(music.id);

        var mediaFileStream = await LoadMediaAsync(songUrl.Data);

        //var mediaSource = MediaSource.CreateFromStorageFile(mediaFile);
        //MediaPlaybackItem mediaPlaybackItem = new MediaPlaybackItem(mediaSource);
        //mediaPlaybackItem.ApplyDisplayProperties(updateSMTC(mediaPlaybackItem.GetDisplayProperties()));
        //_mediaPlayer.Source = mediaPlaybackItem;
        _mediaPlayer.SetStreamSource(mediaFileStream);
        Play();
    }

    public async Task<IRandomAccessStream> LoadMediaAsync(string url) {
        var uri = new Uri(url);
        
        Regex regex = new Regex("[^\\/]+$");
        var cacheFileName = regex.Match(uri.LocalPath).Value;

        try {
            var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(cacheFileName);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return fileStream;
        } catch {}

        try {
                var httpClient = new HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri(url));
                if (buffer != null && buffer.Length > 0u) {
                    var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(cacheFileName, CreationCollisionOption.ReplaceExisting);

                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite)) {
                        await stream.WriteAsync(buffer);
                        await stream.FlushAsync();
                    }
                    var fileStream = await file.OpenAsync(FileAccessMode.Read);
                    return fileStream;
                }

        } catch { }
        
        return null;
    }
    private MediaItemDisplayProperties updateSMTC(MediaItemDisplayProperties properties) {
        properties.Type = MediaPlaybackType.Music;

        properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(this.NowPlaying.picUrl));
        properties.MusicProperties.Title = this.NowPlaying.name;
        properties.MusicProperties.Artist = MusicHelepr.GetArtistString(this.NowPlaying.artists);

        return properties;
    }

    private void shuffle<T>(IList<T> collection) {
        Random _random = new(DateTimeOffset.Now.Minute + DateTimeOffset.Now.Second);
        var n = collection.Count;
        for (var i = 0; i < (n - 1); i++) {
            var r = i + _random.Next(n - i);
            (collection[i], collection[r]) = (collection[r], collection[i]);
        }
    }

    public void SetNextMusic(Music music) {
        var index = this.Playlist.IndexOf(this.NowPlaying) + 1;
        if (index == this.Playlist.Count) {
            index = 0;
        }

        if (this.Playlist.Any(m => m.id == music.id)) {
            _ = this.Playlist.Remove(this.Playlist.Where(m => m.id == music.id).First());
        }

        this.Playlist.Insert(index, music);

        if (this.NowPlaying == null) {
            Next();
        }
    }
}
