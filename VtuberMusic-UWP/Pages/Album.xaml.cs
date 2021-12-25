using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using VtuberMusic_UWP.Components.Dialog;
using VtuberMusic_UWP.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 歌单页
    /// </summary>
    public sealed partial class Album : Page {
        private ConnectedAnimation imageAnimation = null;
        private Models.VtuberMusic.Album album = new Models.VtuberMusic.Album();
        private bool isLkeMusic = false;

        public Album() {
            this.InitializeComponent();
            this.CoverImgBorder.Translation = new System.Numerics.Vector3(0, 0, 32);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.Tag = e.Parameter;
            if (e.Parameter.GetType() == typeof(AlbumPageArgs)) {
                var args = (AlbumPageArgs)e.Parameter;
                this.album = args.Album;
                this.isLkeMusic = true;

                this.loadData(args.IsLikeMusic);
                return;
            }

            this.album = e.Parameter as Models.VtuberMusic.Album;
            this.loadData();
        }

        private async void loadData(bool likeMusic = false) {
            var analyticsData = new Dictionary<string, string>() {
                { "LikeMusicList", likeMusic.ToString() }
            };

            if (likeMusic) analyticsData.Add("Album_Id", $"Like_{ album.id }"); else analyticsData.Add("Album_Id", album.id);
            Analytics.TrackEvent("浏览歌单", analyticsData);

            if (this.album.creator.userId == App.Client.Account.Account.id && !this.isLkeMusic) this.Edit.Visibility = Visibility.Visible;
            if (!this.isLkeMusic && this.album.creator.userId != App.Client.Account.Account.id) this.Like.Visibility = Visibility.Visible;

            this.CreatorInfo.Text = $"创建于 { UsefullTools.ConvertUnixTimeStamp(this.album.createTime).ToString("yyyy/M/d") }";
            this.Like.Icon = this.album.like ? new FontIcon { Glyph = "\uE735" } : new FontIcon { Glyph = "\uE734" };

            this.imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (this.imageAnimation != null) this.imageAnimation.TryStart(this.CoverImgBorder, new UIElement[] { this.InfoPanel });

            this.DataView.ItemsSource = likeMusic
                ? ( await App.Client.GetLikeMusicSong(this.album.id) ).Data.songs
                : ( await App.Client.GetPlayListSong(this.album.id) ).Data.songs;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            base.OnNavigatingFrom(e);

            if (( e.SourcePageType == typeof(Home) | e.SourcePageType == typeof(Search) | e.SourcePageType == typeof(Profile) ) && this.imageAnimation != null) {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", this.CoverImgBorder);
            }
        }

        private void PlayAll_Click(object sender, RoutedEventArgs e) {
            App.Player.PlayList.Clear();
            foreach (var music in this.DataView.ItemsSource) {
                App.Player.PlayList.Add(music);
            }

            App.Player.SetMusic(App.Player.PlayList[0]);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            this.DataView.ItemsSource = null;
        }

        private async void Edit_Click(object sender, RoutedEventArgs e) {
            await App.ContentDialogManager.ShowAsync(new EditAlbumInfoDialog(this.album.id));
            this.album = ( await App.Client.GetPlayListSong(this.album.id) ).Data.playlist;

            this.loadData();
        }

        private void Share_Click(object sender, RoutedEventArgs e) =>
            ShareTools.ShareAlbum(this.album);

        private void CreatorLink_Click(object sender, RoutedEventArgs e) => this.Frame.Navigate(typeof(Profile), this.album.creator, new DrillInNavigationTransitionInfo());

        private async void Like_Click(object sender, RoutedEventArgs e) {
            try {
                await App.Client.Account.SubscribeAlbum(this.album.id, !this.album.like);
                this.album.like = !this.album.like;

                this.Like.Icon = this.album.like ? new FontIcon { Glyph = "\uE735" } : new FontIcon { Glyph = "\uE734" };
                App.ViewModel.MainPage.Load();
            } catch (Exception ex) {
                Crashes.TrackError(ex, new Dictionary<string, string>() {
                    { "Album_Id", this.album.id },
                    { "Like", (!this.album.like).ToString() }
                });
            }
        }

        private void DataView_Loaded(object sender, RoutedEventArgs e) {
            ParallaxView.Source = UsefullTools.FindVisualChild<ScrollViewer>(DataView);
        }
    }

    public class AlbumPageArgs {
        public Models.VtuberMusic.Album Album { get; set; }
        public bool IsLikeMusic { get; set; }
    }
}
