using System;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 首页
    /// </summary>
    public sealed partial class Home : Page {
        private object _albumItem;
        private object _artistItem;

        private bool cachePage = false;

        public Home() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.loadData();
        }

        private async void loadData() {
            Date.Text = DateTime.Now.ToString("M");

            var bannerData = await App.Client.GetBanner();
            this.BannerDataView.ItemsSource = bannerData.Data;

            var newMusicData = await App.Client.GetNewSong(12);
            this.NewMusicDataView.ItemsSource = newMusicData.Data;

            var hotMusicData = await App.Client.GetPersonalizedMusic();
            this.HotMusicDataView.ItemsSource = hotMusicData.Data;

            var artistData = await App.Client.GetArtistList();
            this.VtuberDataView.ItemsSource = artistData.Data;

            var albumData = await App.Client.GetPlayListList();
            this.AlbumDataView.ItemsSource = albumData.Data;
        }

        private void AlbumDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this.cachePage = true;

            this._albumItem = ( (GridViewItem)this.AlbumDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.AlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                this._albumItem,
                "CoverImgBorder");

            this.Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void AlbumDataView_Loaded(object sender, RoutedEventArgs e) {
            this.cachePage = false;

            this.AlbumDataView.ScrollIntoView(this._albumItem);
            this.AlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null) {
                try {
                    await this.AlbumDataView.TryStartConnectedAnimationAsync(animation, this._albumItem, "CoverImgBorder");
                } catch { }
            }
        }

        private void VtuberDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this.cachePage = true;

            this._artistItem = ( (GridViewItem)this.VtuberDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.VtuberDataView.PrepareConnectedAnimation("ArtistForwardConnectedAnimation",
                this._artistItem,
                "Avater");

            this.Frame.Navigate(typeof(Artist), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void VtuberDataView_Loaded(object sender, RoutedEventArgs e) {
            this.cachePage = false;

            this.VtuberDataView.ScrollIntoView(this._artistItem);
            this.VtuberDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistBackConnectedAnimation");
            if (animation != null) {
                await this.VtuberDataView.TryStartConnectedAnimationAsync(animation, this._artistItem, "Avater");
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            if (!this.cachePage) this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Avater_Loaded(object sender, RoutedEventArgs e) {
            ( sender as PersonPicture ).Translation = new Vector3(0, 0, 32);
        }
    }
}
