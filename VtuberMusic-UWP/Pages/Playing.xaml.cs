using Microsoft.AppCenter.Crashes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using VtuberMusic_UWP.Components;
using VtuberMusic_UWP.Components.Lyric;
using VtuberMusic_UWP.Models.Lyric;
using VtuberMusic_UWP.Service;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 播放中页面
    /// </summary>
    public sealed partial class Playing : Page {
        private Lyric[] lyrics;
        private int nowLyricIndex = -1;
        private LyricItem nowLyricItem = null;
        private bool canUpdatePosition = true;
        private Player player => App.Player;

        public Playing() {
            this.InitializeComponent();

            this.ShareShadow.Receivers.Add(this.ShadowBackground);
            this.CoverImgGrid.Translation = new Vector3(0, 0, 32);

            switch (App.Player.PlayState) {
                case MediaPlaybackState.Playing:
                    this.PlayButtonIcon.Symbol = Symbol.Pause;
                    break;
                default:
                    this.PlayButtonIcon.Symbol = Symbol.Play;
                    break;
            }

            App.Player.NowPlayingMusicChanged += this.NowPlayingMusicChanged;
            App.Player.PositionChanged += this.positionUpdate;

            this.init();

            this.CommentShow.Completed += delegate {
                LyricScrollViwer.Visibility = Visibility.Collapsed;
            };
            this.CommentHide.Completed += delegate {
                Comment.Visibility = Visibility.Collapsed;
            };
        }

        public async void init() {
            Comment.ItemsSource = (await App.Client.GetMusicComment(this.player.NowPlayingMusic.id)).Data;

            if (string.IsNullOrEmpty(App.Player.NowPlayingMusic.vrcUrl)) {
                this.lyrics = new Lyric[]
                {
                    new Lyric { Source = "暂无歌词", Time = TimeSpan.Zero, Translation = "" }
                };
            } else {
                var client = new RestClient();
                var request = new RestRequest(App.Player.NowPlayingMusic.vrcUrl, Method.GET);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful) {
                    try {
                        this.lyrics = await Task.Run(() => LyricPaser.Parse(response.Content));
                    } catch (Exception ex) {
                        Crashes.TrackError(ex, new Dictionary<string, string>()
                        {
                            { "Song_Id", App.Player.NowPlayingMusic.id },
                            { "Lyric_Url", App.Player.NowPlayingMusic.vrcUrl },
                            { "LyricRaw", response.Content }
                        });
                    }
                } else {
                    this.lyrics = new Lyric[]
                    {
                        new Lyric { Source = $"获取歌词失败: { response.ErrorMessage }", Time = TimeSpan.Zero, Translation = "" }
                    };

                    Crashes.TrackError(response.ErrorException, new Dictionary<string, string>(){
                        { "Song_Id", App.Player.NowPlayingMusic.id },
                        { "Lyric_Url", App.Player.NowPlayingMusic.vrcUrl }
                    });
                };
            }

            this.LyricView.ItemsSource = this.lyrics;
        }

        private async void NowPlayingMusicChanged(object sender, Models.VtuberMusic.Music e) {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.init();
            }));
        }

        private async void positionUpdate(object sender, TimeSpan e) {
            if (!this.canUpdatePosition) return;
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate {
                if (App.RootFrame.Content.GetType() != typeof(Playing)) return;

                this.Position.Value = e.TotalMilliseconds;

                this.lyricTick();
            });
        }

        #region Lyric
        private async void lyricTick() {
            if (this.lyrics == null) return;
            for (int i = 0; i != this.lyrics.Length; i++) {
                if (i == this.lyrics.Length - 1 && this.lyrics[i].Time <= App.Player.Position) {
                    if (this.nowLyricIndex == i) return;

                    await this.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                        this.ToLyric(i);
                    }));

                    return;
                }

                if (this.lyrics[i].Time >= App.Player.Position) {
                    if (this.nowLyricIndex == i - 1) return;

                    await this.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                        this.ToLyric(i - 1);
                    }));

                    return;
                }
            }
        }

        private void ToLyric(int index) {
            this.nowLyricIndex = index;
            if (this.nowLyricItem != null) this.nowLyricItem.Hide();
            if (index < 0) return;

            var itemContainer = (UIElement)this.LyricView.ContainerFromItem(this.lyrics[index]);
            if (itemContainer == null) return;

            this.LyricView.UpdateLayout();

            GeneralTransform generalTransform = this.LyricScrollViwer.TransformToVisual(itemContainer);
            Point point = generalTransform.TransformPoint(new Point());

            try {
                this.nowLyricItem = this.FindVisualChild<LyricItem>(itemContainer);
                this.nowLyricItem.Show();

                this.LyricScrollViwer.ChangeView(0,
                -point.Y + this.LyricScrollViwer.VerticalOffset - ( this.LyricScrollViwer.ActualHeight / 3 ),
                null);
            } catch { }
        }

        private ChildType FindVisualChild<ChildType>(DependencyObject obj)
        where ChildType : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildType) {
                    return ( child as ChildType );
                } else {
                    ChildType childOfChild = this.FindVisualChild<ChildType>(child);
                    if (childOfChild != null)
                        return ( childOfChild );
                }
            }

            return ( null );
        }
        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            App.Player.PositionChanged -= this.positionUpdate;
            App.Player.NowPlayingMusicChanged -= this.NowPlayingMusicChanged;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => this.Frame.GoBack();
        private void PreviousButton_Click(object sender, RoutedEventArgs e) => App.Player.Previous();
        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            switch (App.Player.PlayState) {
                case MediaPlaybackState.Playing:
                    App.Player.Pause();
                    break;
                default:
                    App.Player.Play();
                    break;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) => App.Player.Next();
        private void Position_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e) => this.canUpdatePosition = false;
        private void Position_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            App.Player.Position = TimeSpan.FromMilliseconds(this.Position.Value);
            this.canUpdatePosition = true;
        }

        private void Position_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e) => this.canUpdatePosition = true;
        private void CompactOverlayButton_Click(object sender, RoutedEventArgs e) => this.Frame.Navigate(typeof(CompactOverlay), null, new DrillInNavigationTransitionInfo());

        private async void LikeButton_Click(object sender, RoutedEventArgs e) {
            LikeButton.IsEnabled = false;
            try {
                await App.Client.Account.LikeMusic(this.player.NowPlayingMusic.id, !this.player.NowPlayingMusic.like);
                this.player.NowPlayingMusic.like = !this.player.NowPlayingMusic.like;
            } catch (Exception ex) {
                InfoBarPopup.Show("喜欢音乐失败", ex.Message);
                Crashes.TrackError(ex, new Dictionary<string, string>() { { "song_id", this.player.NowPlayingMusic.id }, { "like", ( !this.player.NowPlayingMusic.like ).ToString() } });
            }

            LikeButton.IsEnabled = true;
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e) {
            this.CommentShow.Stop();
            this.CommentHide.Stop();

            if (this.CommentButton.IsChecked.GetValueOrDefault()) {
                this.Comment.Visibility = Visibility.Visible;
                this.CommentShow.Begin();
            } else {
                this.LyricScrollViwer.Visibility = Visibility.Visible;
                this.CommentHide.Begin();
            }
        }
    }
}
