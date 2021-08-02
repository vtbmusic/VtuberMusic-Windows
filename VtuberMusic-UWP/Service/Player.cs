using System;
using System.Collections.ObjectModel;
using System.Linq;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace VtuberMusic_UWP.Service
{
    public class Player
    {
        #region 播放器核心
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private MediaTimelineController _timelineController = new MediaTimelineController();
        private MediaPlaybackItem _mediaPlaybackItem;
        private MediaPlaybackItem mediaPlaybackItem
        {
            get
            {
                return _mediaPlaybackItem;
            }
            set
            {
                _mediaPlaybackItem = value;
                _mediaPlayer.Source = value;
            }
        }

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
        public TimeSpan Duration
        {
            get
            {
                return _mediaPlayer.PlaybackSession.NaturalDuration;
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

        public MediaPlaybackState PlayState
        {
            get
            {
                return _mediaPlayer.PlaybackSession.PlaybackState;
            }
        }
        #endregion

        #region 事件
        public EventHandler<MediaTimelineControllerState> PlayStateChanged;
        public EventHandler<TimeSpan> PositionChanged;
        public EventHandler<double> VolumeChanged;
        #endregion

        #region 播放列表
        public ObservableCollection<Music> PlayList { get; private set; } = new ObservableCollection<Music>();
        public EventHandler PlayListChanged;
        public EventHandler<Music> NowPlayingMusicChanged;
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

        private Music _nowPlayingMusic;
        public Music NowPlayingMusic
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

                if (NowPlayingMusic != null) App.ViewModel.SetAppBackgroundImage(new Uri(NowPlayingMusic.picUrl));
            }
        }
        #endregion

        public Player()
        {
            InitSMTC();
            // 设置核心
            _mediaPlayer.TimelineController = _timelineController;
            _timelineController.PositionChanged += positionChanged;
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
            _mediaPlayer.VolumeChanged += _mediaPlayer_VolumeChanged;
            _timelineController.StateChanged += _timelineController_StateChanged;
            // 绑定事件
            PlayListChanged += playListChanged;
            PlayList.CollectionChanged += delegate
            {
                if (PlayListChanged != null) playListChanged(this, null);
            };
        }

        #region 事件处理
        private void _timelineController_StateChanged(MediaTimelineController sender, object args)
        {
            if (PlayStateChanged != null) PlayStateChanged(this, sender.State);
        }
        private void _mediaPlayer_VolumeChanged(MediaPlayer sender, object args)
        {
            if (VolumeChanged != null) VolumeChanged(this, _mediaPlayer.Volume);
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
            }
        }

        private void positionChanged(MediaTimelineController sender, object args)
        {
            updateSMTCTimeline();
            if (PositionChanged != null)
            {
                PositionChanged(this, _timelineController.Position);
            }
        }

        private void playListChanged(object sender, object args)
        {
            if (PlayList.Count != 0)
            {
                _mediaPlayer.CommandManager.IsEnabled = true;
            }
            else
            {
                _mediaPlayer.CommandManager.IsEnabled = false;

                SetMusic(null);
                Stop();
            }
        }
        #endregion

        #region 载入媒体
        public async void SetMusic(Music music)
        {
            if (music != null)
            {
                if (!PlayList.Any(m => m.id == music.id))
                {
                    PlayList.Add(music);
                }

                mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri((await App.Client.GetSongMeduaUri(music.id)).Data)));
                updateSMTC(music, mediaPlaybackItem);
                mediaPlaybackItem.Source.OpenOperationCompleted += Source_OpenOperationCompleted;
                _mediaPlaybackItem = mediaPlaybackItem;

                Stop();
                NowPlayingMusic = music;
            }
            else
            {
                NowPlayingMusic = music;
                _mediaPlaybackItem = null;
            }
        }

        private void Source_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            Start();
        }
        #endregion

        #region 播放器控制方法
        public void Start()
        {
            Stop();
            _timelineController.Start();
        }

        public void Play()
        {
            _timelineController.Resume();
        }

        public void Pause()
        {
            _timelineController.Pause();
        }

        public void Stop()
        {
            _timelineController.Position = TimeSpan.Zero;
            _timelineController.Pause();
        }
        #endregion

        #region 系统媒体控件
        private void InitSMTC()
        {
            _mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PlayBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PauseBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            _mediaPlayer.CommandManager.NextReceived += CommandManager_NextReceived;
            _mediaPlayer.CommandManager.PreviousReceived += CommandManager_PreviousReceived;
            _mediaPlayer.CommandManager.ShuffleReceived += CommandManager_ShuffleReceived;
            _mediaPlayer.CommandManager.PlayReceived += CommandManager_PlayReceived;
            _mediaPlayer.CommandManager.PauseReceived += CommandManager_PauseReceived;
        }

        #region 处理按钮点击
        private void CommandManager_PauseReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPauseReceivedEventArgs args) => Pause();
        private void CommandManager_PlayReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPlayReceivedEventArgs args) => Play();
        private void CommandManager_ShuffleReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerShuffleReceivedEventArgs args)
        {
            if (args.IsShuffleRequested)
            {
                PlayMode = PlayMode.Shuffle;
            }
            else
            {
                PlayMode = PlayMode.ListRepeat;
            }
        }

        private void CommandManager_PreviousReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args) => Previous();
        private void CommandManager_NextReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args) => Next();
        #endregion

        private void updateSMTCTimeline()
        {
            var pros = new SystemMediaTransportControlsTimelineProperties();
            pros.StartTime = TimeSpan.Zero;
            pros.MinSeekTime = TimeSpan.Zero;
            pros.MaxSeekTime = Duration;
            pros.Position = Position;
            pros.EndTime = Duration;

            _mediaPlayer.SystemMediaTransportControls.UpdateTimelineProperties(pros);
        }

        private void updateSMTC(Music music, MediaPlaybackItem mediaPlaybackItem)
        {
            var pros = mediaPlaybackItem.GetDisplayProperties();

            if (music != null)
            {
                pros.Type = MediaPlaybackType.Music;
                if (music.alias != null)
                {
                    pros.MusicProperties.AlbumTitle = (string)music.alias;
                }

                var artists = "";
                foreach (var temp in music.artists)
                {
                    artists += temp.name.origin + "";
                }

                pros.MusicProperties.AlbumArtist = artists;
                pros.MusicProperties.Artist = artists;
                pros.MusicProperties.Title = music.name;
                pros.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(music.picUrl));
            }
            else
            {
                pros.ClearAll();
            }

            mediaPlaybackItem.ApplyDisplayProperties(pros);
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
        #endregion
    }

    public enum PlayMode
    {
        ListRepeat,
        SingleRepeat,
        Shuffle
    }
}
