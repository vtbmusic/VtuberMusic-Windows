using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VtuberMusic_UWP.Components;
using VtuberMusic_UWP.Components.Lyric;
using VtuberMusic_UWP.Models.Lyric;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Service;
using VtuberMusic_UWP.Tools;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 画中画模式页
    /// </summary>
    public sealed partial class CompactOverlay : Page {
        public bool canUpdatePosition = true;
        public Lyric[] lyrics;
        private LyricItem nowLyricItem;
        private int nowLyricIndex = -1;
        private Player player => App.Player;

        public CompactOverlay() {
            this.InitializeComponent();

            App.Player.PositionChanged += this.positionUpdate;
            App.Player.NowPlayingMusicChanged += this.Player_NowPlayingMusicChanged;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            Analytics.TrackEvent("进入画中画");
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            App.Player.PositionChanged -= this.positionUpdate;
            App.Player.NowPlayingMusicChanged -= this.Player_NowPlayingMusicChanged;
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
        }

        private async void positionUpdate(object sender, TimeSpan e) {
            if (!this.canUpdatePosition) return;
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate {
                if (App.RootFrame.Content.GetType() != typeof(CompactOverlay)) return;

                this.lyricTick();
            });
        }

        private async void Player_NowPlayingMusicChanged(object sender, Models.VtuberMusic.Music e) =>
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.load();
            }));

        private async void load() {
            if (App.Player.PlayList.Count != 0) {
                Music NextMusic = null;
                switch (App.Player.PlayMode) {
                    case PlayMode.SingleRepeat:
                        NextMusic = this.player.NowPlayingMusic;
                        break;
                    case PlayMode.Shuffle:
                        break;
                    default:
                        if (this.player.PlayList.IndexOf(this.player.NowPlayingMusic) == App.Player.PlayList.Count - 1) {
                            NextMusic = App.Player.PlayList.First();
                        } else {
                            NextMusic = App.Player.PlayList[App.Player.PlayList.IndexOf(App.Player.NowPlayingMusic) + 1];
                        }
                        break;
                }

                NextMusicInfo.Text = $"{ NextMusic.name } - { UsefullTools.GetArtistsString(NextMusic.artists)}";
            }

            if (string.IsNullOrEmpty(App.Player.NowPlayingMusic.vrcUrl)) {
                this.lyrics = new Lyric[]
                {
                    new Lyric { Source = "暂无歌词", Time = TimeSpan.Zero, Translation = "" }
                };
            } else {
                var client = new RestClient();
                var request = new RestRequest(App.Player.NowPlayingMusic.vrcUrl, Method.Get);
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
                        new Lyric { Source = $"获取歌词失败: { response.StatusCode }", Time = TimeSpan.Zero, Translation = "" }
                    };

                    Crashes.TrackError(response.ErrorException, new Dictionary<string, string>(){
                        { "Song_Id", App.Player.NowPlayingMusic.id },
                        { "Lyric_Url", App.Player.NowPlayingMusic.vrcUrl }
                    });
                };
            }

            this.LyricView.ItemsSource = this.lyrics;
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
                 -point.Y + this.LyricScrollViwer.VerticalOffset - ( this.LyricScrollViwer.ActualHeight / 5 ),
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

        private void NextButton_Click(object sender, RoutedEventArgs e) => App.Player.Next();
        private void PreviousButton_Click(object sender, RoutedEventArgs e) => App.Player.Previous();
        private void ExitButton_Click(object sender, RoutedEventArgs e) => this.Frame.GoBack();
        private void Share_Click(object sender, RoutedEventArgs e) => ShareTools.ShareMusic(App.Player.NowPlayingMusic);

        private async void Like_Click(object sender, RoutedEventArgs e) {
            Like.IsEnabled = false;
            try {
                await App.Client.Account.LikeMusic(App.Player.NowPlayingMusic.id, !App.Player.NowPlayingMusic.like);
                App.Player.NowPlayingMusic.like = !App.Player.NowPlayingMusic.like;
            } catch (Exception ex) {
                Crashes.TrackError(ex, new Dictionary<string, string>() { { "music_id", App.Player.NowPlayingMusic.id } });
                InfoBarPopup.Show("无法喜欢音乐", ex.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
            }

            Like.IsEnabled = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e) {

        }
        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            if (App.Player.PlayState == Windows.Media.Playback.MediaPlaybackState.Playing) {
                App.Player.Pause();
            } else {
                App.Player.Play();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => this.load();
    }
}
