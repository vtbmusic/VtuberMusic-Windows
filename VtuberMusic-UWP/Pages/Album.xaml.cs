using System;
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
        private Models.VtuberMusic.Album album = null;
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
            if (this.album.creator.userId == App.Client.Account.Account.id && !this.isLkeMusic) this.Edit.Visibility = Visibility.Visible;
            this.AlbumName.Text = this.album.name;
            this.CreatorInfo.Text = $"{ this.album.creator.nickname } 创建于 { UsefullTools.ConvertUnixTimeStamp(this.album.createTime).ToString("yyyy/M/d") }";
            this.Introduction.Text = this.album.description != null ? this.album.description : "这个作者很懒没写简介哦～";

            this.CoverImg.ImageSource = new BitmapImage(new Uri(this.album.coverImgUrl));

            this.imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (this.imageAnimation != null) this.imageAnimation.TryStart(this.CoverImgBorder, new UIElement[] { this.InfoPanel });

            this.DataView.ItemsSource = likeMusic
                ? ( await App.Client.Account.GetLikeMusicSong() ).Data.songs
                : ( await App.Client.GetPlayListSong(this.album.id) ).Data.songs;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            base.OnNavigatingFrom(e);

            if (( e.SourcePageType == typeof(Home) | e.SourcePageType == typeof(Search) ) && this.imageAnimation != null) {
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
            await new EditAlbumInfoDialog().ShowAsync(this.album.id);
            this.album = ( await App.Client.GetPlayListSong(this.album.id) ).Data.playlist;

            this.loadData();
        }

        private void Share_Click(object sender, RoutedEventArgs e) =>
            ShareTools.ShareAlbum(this.album);
    }

    public class AlbumPageArgs {
        public Models.VtuberMusic.Album Album { get; set; }
        public bool IsLikeMusic { get; set; }
    }
}
