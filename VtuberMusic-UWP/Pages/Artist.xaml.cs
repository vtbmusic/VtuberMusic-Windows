using System;
using System.Collections.ObjectModel;
using VtuberMusic_UWP.Models.VtuberMusic;
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
        private Models.VtuberMusic.Artist artist;
        private ObservableCollection<Music> dataList = new ObservableCollection<Music>();

        private bool isLoading = false;
        private int offset = 0;
        private int limit = 20;

        public Artist() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            artist = (Models.VtuberMusic.Artist)e.Parameter;
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

            foreach (var temp in ( await App.Client.GetArtistSong(this.artist.id) ).Data) {
                dataList.Add(temp);
            }

            this.offset++;
            this.isLoading = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            base.OnNavigatingFrom(e);

            if (( e.SourcePageType == typeof(Home) || ( e.SourcePageType == typeof(Search) ) ) && this.imageAnimation != null) {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ArtistBackConnectedAnimation", this.Avater);
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) {
            if (!this.isLoading) {
                if (RootScrollViewer.VerticalOffset <= RootScrollViewer.ScrollableHeight - 500) return;
                this.isLoading = true;

                foreach (var temp in ( await App.Client.GetArtistSong(this.artist.id, "time", this.limit, this.offset) ).Data) {
                    dataList.Add(temp);
                }

                this.offset++;
                this.isLoading = false;
            }
        }
    }
}
