using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 歌手页
    /// </summary>
    public sealed partial class Artist : Page {
        private ConnectedAnimation imageAnimation = null;

        public Artist() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            this.loadData((Models.VtuberMusic.Artist)e.Parameter);
        }

        private async void loadData(Models.VtuberMusic.Artist artist) {
            this.ArtistName.Text = artist.name.origin;
            // awsl
            if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn)) {
                this.OtherNameAndGroup.Text = artist.name.cn;
            }

            if (artist.name.jp != artist.name.origin && !string.IsNullOrEmpty(artist.name.jp)) {
                if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn)) this.OtherNameAndGroup.Text += " / ";
                this.OtherNameAndGroup.Text += artist.name.jp;
            }

            if (artist.name.en != artist.name.origin && !string.IsNullOrEmpty(artist.name.en)) {
                if (artist.name.jp != artist.name.origin && !string.IsNullOrEmpty(artist.name.jp)) {
                    this.OtherNameAndGroup.Text += " / ";
                } else if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn)) this.OtherNameAndGroup.Text += " / ";

                this.OtherNameAndGroup.Text += artist.name.en;
            }

            if (this.OtherNameAndGroup.Text != "") this.OtherNameAndGroup.Text += " - ";
            this.OtherNameAndGroup.Text += artist.groupName;

            this.Avater.ProfilePicture = new BitmapImage(new Uri(artist.imgUrl));

            this.imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistForwardConnectedAnimation");
            if (this.imageAnimation != null) this.imageAnimation.TryStart(this.Avater, new UIElement[] { this.InfoPanel });

            this.MusicCount.Text = artist.musicSize.ToString();
            this.AlbumCount.Text = artist.albumSize.ToString();
            this.FanCount.Text = artist.likeSize.ToString();

            this.DataView.ItemsSource = ( await App.Client.GetArtistSong(artist.id, "time", 1000) ).Data;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            base.OnNavigatingFrom(e);

            if (( e.SourcePageType == typeof(Home) || ( e.SourcePageType == typeof(Search) ) ) && this.imageAnimation != null) {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ArtistBackConnectedAnimation", this.Avater);
            }
        }
    }
}
