using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 搜索页
    /// </summary>
    public sealed partial class Search : Page {
        private string keyWord = "";
        private object _artistItem = null;
        private object _albumItem = null;

        public Search() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// 搜索关键词
        /// </summary>
        /// <param name="keyword">关键词</param>
        public async void SearchKeyword(string keyword) {
            this.MusicDataList.ItemsSource = null;
            this.VtuberDataView.ItemsSource = null;
            this.PlayListDataView.ItemsSource = null;

            this.MusicLoading.Visibility = Visibility.Visible;
            this.MusicDataList.ItemsSource = ( await App.Client.SearchMusic(keyword) ).Data;
            this.MusicLoading.Visibility = Visibility.Collapsed;

            this.VtuberLoading.Visibility = Visibility.Visible;
            this.VtuberDataView.ItemsSource = ( await App.Client.SearchArtist(keyword) ).Data;
            this.VtuberLoading.Visibility = Visibility.Collapsed;

            this.PlayListLoading.Visibility = Visibility.Visible;
            this.PlayListDataView.ItemsSource = ( await App.Client.SearchPlaylist(keyword) ).Data;
            this.PlayListLoading.Visibility = Visibility.Collapsed;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var keyword = (string)e.Parameter;
            if (keyword == this.keyWord) return;

            this.keyWord = keyword;
            this.SearchKeyword((string)e.Parameter);
        }

        private void VtuberDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this._artistItem = ( (GridViewItem)this.VtuberDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.VtuberDataView.PrepareConnectedAnimation("ArtistForwardConnectedAnimation",
                this._artistItem,
                "ArtistAvater");

            this.Frame.Navigate(typeof(Artist), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void VtuberDataView_Loaded(object sender, RoutedEventArgs e) {
            this.VtuberDataView.ScrollIntoView(this._artistItem);
            this.VtuberDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistBackConnectedAnimation");
            if (animation != null) {
                await this.VtuberDataView.TryStartConnectedAnimationAsync(animation, this._artistItem, "ArtistAvater");
            }
        }

        private void PlayListDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this._albumItem = ( (GridViewItem)this.PlayListDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.PlayListDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                this._albumItem,
                "CoverImgBorder");

            this.Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void PlayListDataView_Loaded(object sender, RoutedEventArgs e) {
            this.PlayListDataView.ScrollIntoView(this._albumItem);
            this.PlayListDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null) {
                await this.PlayListDataView.TryStartConnectedAnimationAsync(animation, this._albumItem, "CoverImgBorder");
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            if (e.SourcePageType != typeof(Album) && e.SourcePageType != typeof(Artist)) {
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
                GC.Collect();
            }
        }
    }
}
