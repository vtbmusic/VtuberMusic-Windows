using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VtuberMusic_UWP.Components;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace VtuberMusic_UWP.Service {
    public class Player : INotifyPropertyChanged {
        #region 播放器核心
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private MediaPlaybackItem _mediaPlaybackItem;
        private MediaPlaybackItem mediaPlaybackItem {
            get { return this._mediaPlaybackItem; }
            set {
                this._mediaPlaybackItem = value;
                this._mediaPlayer.Source = value;
            }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public double Volume {
            get { return this._mediaPlayer.Volume; }
            set {
                this._mediaPlayer.Volume = value;
                VolumeChanged?.Invoke(this, this._mediaPlayer.Volume);
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 音频总时间
        /// </summary>
        public TimeSpan Duration { get { return this._mediaPlayer.PlaybackSession.NaturalDuration; } }
        /// <summary>
        /// 当前播放位置
        /// </summary>
        public TimeSpan Position {
            get { return this._mediaPlayer.PlaybackSession.Position; }
            set {
                this.NotifyPropertyChanged();
                this._mediaPlayer.PlaybackSession.Position = value;
            }
        }

        /// <summary>
        /// 播放状态
        /// </summary>
        public MediaPlaybackState PlayState { get { return this._mediaPlayer.PlaybackSession.PlaybackState; } }
        #endregion

        #region 事件
        /// <summary>
        /// 播放状态改变
        /// </summary>
        public event EventHandler<MediaPlaybackState> PlayStateChanged;
        /// <summary>
        /// 播放进度改变
        /// </summary>
        public event EventHandler<TimeSpan> PositionChanged;
        /// <summary>
        /// 音量改变
        /// </summary>
        public event EventHandler<double> VolumeChanged;
        #endregion

        #region 播放列表
        /// <summary>
        /// 播放列表
        /// </summary>
        public ObservableCollection<Music> PlayList { get; private set; } = new ObservableCollection<Music>();
        /// <summary>
        /// 播放列表改变
        /// </summary>
        public event EventHandler PlayListChanged;
        /// <summary>
        /// 正在播放音乐改变
        /// </summary>
        public event EventHandler<Music> NowPlayingMusicChanged;
        /// <summary>
        /// 播放列表模式改变
        /// </summary>
        public event EventHandler<PlayMode> PlayModeChanged;

        private PlayMode _playMode;
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayMode PlayMode {
            get { return this._playMode; }
            set {
                this._playMode = value;
                PlayModeChanged?.Invoke(this, this._playMode);
            }
        }

        private Music _nowPlayingMusic;
        /// <summary>
        /// 正在播放音乐
        /// </summary>
        public Music NowPlayingMusic {
            get { return this._nowPlayingMusic; }
            private set {
                this._nowPlayingMusic = value;
                NowPlayingMusicChanged?.Invoke(this, value);
                this.NotifyPropertyChanged();

                if (this.NowPlayingMusic != null) App.ViewModel.BackgroundImageUri = this.NowPlayingMusic.picUrl;
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public Player() {
            this.InitSMTC();
            // 设置核心
            this._mediaPlayer.PlaybackSession.PositionChanged += this.positionChanged;
            this._mediaPlayer.MediaEnded += this._mediaPlayer_MediaEnded;
            this._mediaPlayer.VolumeChanged += this._mediaPlayer_VolumeChanged;
            this._mediaPlayer.PlaybackSession.PlaybackStateChanged += this._mediaPlayer_StateChanged;
            this._mediaPlayer.MediaOpened += this._mediaPlayer_MediaOpened;
            this._mediaPlayer.MediaFailed += this._mediaPlayer_MediaFailed;
            // 绑定事件
            PlayListChanged += this.playListChanged;
            this.PlayList.CollectionChanged += delegate {
                this.playListChanged(this, null);
            };
        }

        private async void _mediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args) {
            Crashes.TrackError(args.ExtendedErrorCode,
                new Dictionary<string, string>() {
                    { "error_type", args.Error.ToString() },
                    { "error_message", args.ErrorMessage },
                    { "music_url", this._mediaPlaybackItem.Source.Uri.ToString() },
                    { "music_id", this.NowPlayingMusic.id }
                });

            await App.RootFrame.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                InfoBarPopup.Show($"播放失败: { args.Error.ToString() }", args.ErrorMessage + "\n" + args.ExtendedErrorCode.ToString(), Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
            }));
        }

        #region 事件处理
        private void _mediaPlayer_StateChanged(MediaPlaybackSession sender, object args) {
            PlayStateChanged?.Invoke(this, sender.PlaybackState);
            this.NotifyPropertyChanged("PlayState");
        }

        private void _mediaPlayer_MediaOpened(MediaPlayer sender, object args) =>
            this.NotifyPropertyChanged("Duration");

        private void _mediaPlayer_VolumeChanged(MediaPlayer sender, object args) {
            VolumeChanged?.Invoke(this, this._mediaPlayer.Volume);
            this.NotifyPropertyChanged("Volume");
        }

        private void _mediaPlayer_MediaEnded(MediaPlayer sender, object args) {
            switch (this.PlayMode) {
                case PlayMode.ListRepeat:
                    this.Next();
                    break;
                case PlayMode.SingleRepeat:
                    this.Position = TimeSpan.Zero;
                    break;
                case PlayMode.Shuffle:
                    break;
            }
        }

        private void positionChanged(MediaPlaybackSession sender, object args) {
            this.updateSMTCTimeline();

            PositionChanged?.Invoke(this, this.Position);
            this.NotifyPropertyChanged("Position");
        }

        private void playListChanged(object sender, object args) {
            if (this.PlayList.Count != 0) {
                this._mediaPlayer.CommandManager.IsEnabled = true;
            } else {
                this._mediaPlayer.CommandManager.IsEnabled = false;

                this.SetMusic(null);
                this.Stop();
            }
        }
        #endregion

        #region 载入媒体
        /// <summary>
        /// 载入音乐
        /// </summary>
        /// <param name="music">Music Object</param>
        public async void SetMusic(Music music) {
            try {
                if (music != null) {
                    if (!this.PlayList.Any(m => m.id == music.id)) {
                        this.PlayList.Add(music);
                    }

                    this.mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(( await App.Client.GetSongMeduaUri(music.id) ).Data)));
                    this.updateSMTC(music, this.mediaPlaybackItem);
                    this.mediaPlaybackItem.Source.OpenOperationCompleted += this.Source_OpenOperationCompleted;
                    this._mediaPlaybackItem = this.mediaPlaybackItem;

                    this.Stop();
                    this.NowPlayingMusic = music;
                } else {
                    this.NowPlayingMusic = music;
                    this._mediaPlaybackItem = null;
                }
            } catch (Exception ex) {
                var data = new Dictionary<string, string>()
                {
                    { "music_id", music.id }
                };

                Crashes.TrackError(ex, data);
            }
        }

        private void Source_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args) {
            this.NotifyPropertyChanged(nameof(this.Duration));
            this.Start();
        }
        #endregion

        #region 播放器控制方法
        /// <summary>
        /// 启动播放
        /// </summary>
        public void Start() {
            this.Stop();
            this._mediaPlayer.Play();
        }

        /// <summary>
        /// 恢复播放
        /// </summary>
        public void Play() => this._mediaPlayer.Play();
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause() => this._mediaPlayer.Pause();
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop() {
            this._mediaPlayer.PlaybackSession.Position = TimeSpan.Zero;
            this.Pause();
        }
        #endregion

        #region 系统媒体控件
        private void InitSMTC() {
            this._mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            this._mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            this._mediaPlayer.CommandManager.PlayBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            this._mediaPlayer.CommandManager.PauseBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            this._mediaPlayer.CommandManager.NextReceived += this.CommandManager_NextReceived;
            this._mediaPlayer.CommandManager.PreviousReceived += this.CommandManager_PreviousReceived;
            this._mediaPlayer.CommandManager.ShuffleReceived += this.CommandManager_ShuffleReceived;
            this._mediaPlayer.CommandManager.PlayReceived += this.CommandManager_PlayReceived;
            this._mediaPlayer.CommandManager.PauseReceived += this.CommandManager_PauseReceived;
        }

        #region 处理按钮点击
        private void CommandManager_PauseReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPauseReceivedEventArgs args) => this.Pause();
        private void CommandManager_PlayReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPlayReceivedEventArgs args) => this.Play();
        private void CommandManager_ShuffleReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerShuffleReceivedEventArgs args) {
            this.PlayMode = args.IsShuffleRequested ? PlayMode.Shuffle : PlayMode.ListRepeat;
        }

        private void CommandManager_PreviousReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args) => this.Previous();
        private void CommandManager_NextReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args) => this.Next();
        #endregion

        private void updateSMTCTimeline() {
            var pros = new SystemMediaTransportControlsTimelineProperties();
            pros.StartTime = TimeSpan.Zero;
            pros.MinSeekTime = TimeSpan.Zero;
            pros.MaxSeekTime = this.Duration;
            pros.Position = this.Position;
            pros.EndTime = this.Duration;

            this._mediaPlayer.SystemMediaTransportControls.UpdateTimelineProperties(pros);
        }

        private void updateSMTC(Music music, MediaPlaybackItem mediaPlaybackItem) {
            var pros = mediaPlaybackItem.GetDisplayProperties();

            if (music != null) {
                pros.Type = MediaPlaybackType.Music;
                if (music.alias != null) {
                    pros.MusicProperties.AlbumTitle = (string)music.alias;
                }

                var artists = "";
                foreach (var temp in music.artists) {
                    artists += temp.name.origin + " ";
                }

                pros.MusicProperties.AlbumArtist = artists;
                pros.MusicProperties.Artist = artists;
                pros.MusicProperties.Title = music.name;
                pros.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(music.picUrl));
            } else {
                pros.ClearAll();
            }

            mediaPlaybackItem.ApplyDisplayProperties(pros);
        }
        #endregion

        #region 播放列表
        /// <summary>
        /// 下一曲
        /// </summary>
        public void Next() {
            if (this.PlayList.Count != 0) {
                switch (this.PlayMode) {
                    case PlayMode.Shuffle:
                        break;
                    default:
                        if (this.PlayList.IndexOf(this.NowPlayingMusic) == this.PlayList.Count - 1) {
                            this.SetMusic(this.PlayList.First());
                        } else {
                            this.SetMusic(this.PlayList[this.PlayList.IndexOf(this.NowPlayingMusic) + 1]);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 上一曲
        /// </summary>
        public void Previous() {
            if (this.PlayList.Count != 0) {
                switch (this.PlayMode) {
                    case PlayMode.Shuffle:
                        break;
                    default:
                        if (this.PlayList.IndexOf(this.NowPlayingMusic) == 0) {
                            this.SetMusic(this.PlayList.Last());
                        } else {
                            this.SetMusic(this.PlayList[this.PlayList.IndexOf(this.NowPlayingMusic) - 1]);
                        }

                        break;
                }
            }
        }
        #endregion

        #region ViewModel
        private async void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            await App.RootFrame.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
        #endregion
    }

    /// <summary>
    /// 播放模式
    /// </summary>
    public enum PlayMode {
        /// <summary>
        /// 列表循环
        /// </summary>
        ListRepeat,
        /// <summary>
        /// 单曲循环
        /// </summary>
        SingleRepeat,
        /// <summary>
        /// 随机播放
        /// </summary>
        Shuffle
    }
}
