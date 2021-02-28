using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace VtuberMusic_UWP.Service
{
    public class Player
    {
        // 核心
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private MediaTimelineController _timelineController = new MediaTimelineController();
        private SystemMediaTransportControls _systemMediaTransportControls;
        private MediaSource _mediaSource;
        private MediaSource mediaSource
        {
            get
            {
                return _mediaSource;
            }
            set
            {
                _mediaSource = value;
                _mediaPlayer.Source = _mediaSource;
            }
        }
        // 播放
        public EventHandler<PlayState> PlayStateChanged;
        public EventHandler<TimeSpan> PositionChanged;
        public EventHandler<TimeSpan> DurationChanged;
        public EventHandler<double> VolumeChanged;

        public double Volume
        {
            get
            {
                return _mediaPlayer.Volume;
            }
            set
            {
                _mediaPlayer.Volume = value;
                if (VolumeChanged != null)
                {
                    VolumeChanged(this, _mediaPlayer.Volume);
                }
            }
        }
        private TimeSpan? _duration;
        public TimeSpan? Duration
        {
            get
            {
                return _duration;
            }
            private set
            {
                _duration = value;
                if (DurationChanged != null)
                {
                    DurationChanged(this, _duration.GetValueOrDefault());
                }
            }
        }
        public TimeSpan Position
        {
            get
            {
                return _timelineController.Position;
            }
            set
            {
                _timelineController.Position = value;
            }
        }

        private PlayState _playState;
        public PlayState PlayState
        {
            get
            {
                return _playState;
            }
            private set
            {
                _playState = value;
                if (PlayStateChanged != null)
                {
                    PlayStateChanged(this, _playState);
                }
            }
        }
        // 播放列表
        public ObservableCollection<MusicData> PlayList { get; private set; } = new ObservableCollection<MusicData>();
        public EventHandler PlayListChanged;
        public EventHandler<MusicData> NowPlayingMusicChanged;
        public EventHandler<PlayMode> PlayModeChanged;

        private PlayMode _playMode;
        public PlayMode PlayMode
        {
            get
            {
                return _playMode;
            }
            set
            {
                _playMode = value;
                if (PlayModeChanged != null)
                {
                    PlayModeChanged(this, _playMode);
                }
            }
        }

        private MusicData _nowPlayingMusic;
        public MusicData NowPlayingMusic
        {
            get
            {
                return _nowPlayingMusic;
            }
            private set
            {
                _nowPlayingMusic = value;
                if (NowPlayingMusicChanged != null)
                {
                    NowPlayingMusicChanged(this, value);
                }
            }
        }

        public Player()
        {
            // 设置系统媒体信息控件
            _mediaPlayer.CommandManager.IsEnabled = false;
            _systemMediaTransportControls = _mediaPlayer.SystemMediaTransportControls;
            _systemMediaTransportControls.IsEnabled = true;
            // 绑定系统媒体信息控件事件
            _systemMediaTransportControls.ButtonPressed += _systemMediaTransportControls_ButtonPressed;
            // 设置核心
            _mediaPlayer.TimelineController = _timelineController;
            _timelineController.PositionChanged += positionChanged;
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
            _mediaPlayer.VolumeChanged += _mediaPlayer_VolumeChanged;
            // 绑定事件
            PlayListChanged += playListChanged;
            PlayStateChanged += playStateChanged;
            NowPlayingMusicChanged += nowPlayingMusicChanged;
        }

        private void _mediaPlayer_VolumeChanged(MediaPlayer sender, object args)
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(this, _mediaPlayer.Volume);
            }
        }

        private void nowPlayingMusicChanged(object sender, MusicData e)
        {
            updateMediaTransportControls(e);
            _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Changing;
        }

        private void _mediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            switch (PlayMode)
            {
                case PlayMode.ListRepeat:
                    Next();
                    break;
                case PlayMode.SingleRepeat:
                    Position = TimeSpan.Zero;
                    break;
                case PlayMode.Shuffle:
                    break;
                default:
                    Next();
                    break;
            }
        }

        private void positionChanged(MediaTimelineController sender, object args)
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, _timelineController.Position);
            }
        }

        private void playStateChanged(object sender, PlayState e)
        {
            switch (e)
            {
                case PlayState.Playing:
                    _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    _systemMediaTransportControls.IsStopEnabled = true;
                    break;
                case PlayState.Pause:
                    _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    _systemMediaTransportControls.IsStopEnabled = true;
                    break;
                case PlayState.Stop:
                    _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    _systemMediaTransportControls.IsStopEnabled = false;
                    break;
            }
        }

        public void SetMusic(MusicData music)
        {
            if (music != null)
            {
                if (!PlayList.Any(m => m.Id == music.Id))
                {
                    PlayListAddMusic(music);
                }

                NowPlayingMusic = music;
                Stop();
                mediaSource = MediaSource.CreateFromUri(new Uri(music.ResourcesUrl.Music));
                mediaSource.OpenOperationCompleted += _mediaSource_OpenOperationCompleted;
            }
            else
            {
                NowPlayingMusic = music;
                mediaSource = null;
            }
        }

        private void _mediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            Duration = sender.Duration;
            Start();
        }

        private void playListChanged(object sender, object args)
        {
            if (PlayList.Count != 0)
            {
                _systemMediaTransportControls.IsPreviousEnabled = true;
                _systemMediaTransportControls.IsNextEnabled = true;
                _systemMediaTransportControls.IsPlayEnabled = true;
                _systemMediaTransportControls.IsPauseEnabled = true;
                _systemMediaTransportControls.IsStopEnabled = true;
            }
            else
            {
                _systemMediaTransportControls.IsPreviousEnabled = false;
                _systemMediaTransportControls.IsNextEnabled = false;
                _systemMediaTransportControls.IsPlayEnabled = false;
                _systemMediaTransportControls.IsPauseEnabled = false;
                _systemMediaTransportControls.IsStopEnabled = false;

                SetMusic(null);
                Stop();
            }
        }

        public void Start()
        {
            Stop();
            Play();
        }

        public void Play()
        {
            _timelineController.Resume();
            PlayState = PlayState.Playing;
        }

        public void Pause()
        {
            _timelineController.Pause();
            PlayState = PlayState.Pause;
        }

        public void Stop()
        {
            _timelineController.Position = TimeSpan.Zero;
            _timelineController.Pause();
            PlayState = PlayState.Stop;
        }

        #region 系统媒体传输控件
        private void updateMediaTransportControls(MusicData music)
        {
            if (music != null)
            {
                var updater = _systemMediaTransportControls.DisplayUpdater;
                updater.Type = MediaPlaybackType.Music;
                if (music.SourceName != null)
                {
                    updater.MusicProperties.AlbumTitle = (string)music.SourceName;
                }

                updater.MusicProperties.AlbumArtist = music.VocalName;
                updater.MusicProperties.Artist = music.VocalName;
                updater.MusicProperties.Title = music.OriginName;
                updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(music.ResourcesUrl.CoverImg));

                updater.Update();
            }
            else
            {
                _systemMediaTransportControls.DisplayUpdater.ClearAll();
            }
        }

        private void _systemMediaTransportControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Play();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Pause();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    Stop();
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Next();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Previous();
                    break;
            }
        }
        #endregion

        #region 播放列表
        public void Next()
        {
            if (PlayList.Count != 0)
            {
                switch (PlayMode)
                {
                    case PlayMode.Shuffle:
                        break;
                    default:
                        if (PlayList.IndexOf(NowPlayingMusic) == PlayList.Count - 1)
                        {
                            SetMusic(PlayList.First());
                        }
                        else
                        {
                            SetMusic(PlayList[PlayList.IndexOf(NowPlayingMusic) + 1]);
                        }
                        break;
                }
            }
        }

        public void Previous()
        {
            if (PlayList.Count != 0)
            {
                switch (PlayMode)
                {
                    case PlayMode.Shuffle:
                        break;
                    default:
                        if (PlayList.IndexOf(NowPlayingMusic) == 0)
                        {
                            SetMusic(PlayList.Last());
                        }
                        else
                        {
                            SetMusic(PlayList[PlayList.IndexOf(NowPlayingMusic) - 1]);
                        }
                        break;
                }
            }
        }

        public void PlayListAddMusic(MusicData music)
        {
            if (!PlayList.Any(m => m.Id == music.Id))
            {
                PlayList.Add(music);
                if (PlayListChanged != null)
                {
                    PlayListChanged(this, null);
                }
            }
        }

        public void PlayListDeleteMusic(MusicData music)
        {
            if (PlayList.Any(m => m.Id == music.Id))
            {
                PlayList.Remove(music);
                if (PlayListChanged != null)
                {
                    PlayListChanged(this, null);
                }
            }
        }
        #endregion
    }

    public enum PlayMode {
        ListRepeat,
        SingleRepeat,
        Shuffle
    }

    public enum PlayState
    {
        Playing,
        Pause,
        Stop
    }
}
