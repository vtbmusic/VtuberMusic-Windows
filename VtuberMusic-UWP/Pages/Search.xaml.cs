using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Search : Page
    {
        private string keyWord = "";
        private object _artistItem = null;
        private object _albumItem = null;

        public Search()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        public async void SearchKeyword(string keyword)
        {
            MusicDataList.ItemsSource = null;
            VtuberDataView.ItemsSource = null;
            PlayListDataView.ItemsSource = null;

            MusicLoading.Visibility = Visibility.Visible;
            MusicDataList.ItemsSource = (await App.Client.SearchMusic(keyword)).Data;
            MusicLoading.Visibility = Visibility.Collapsed;

            VtuberLoading.Visibility = Visibility.Visible;
            VtuberDataView.ItemsSource = (await App.Client.SearchArtist(keyword)).Data;
            VtuberLoading.Visibility = Visibility.Collapsed;

            PlayListLoading.Visibility = Visibility.Visible;
            PlayListDataView.ItemsSource = (await App.Client.SearchPlaylist(keyword)).Data;
            PlayListLoading.Visibility = Visibility.Collapsed;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var keyword = (string)e.Parameter;
            if (keyword == keyWord) return;

            keyWord = keyword;
            SearchKeyword((string)e.Parameter);
        }

        private void VtuberDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _artistItem = ((GridViewItem)VtuberDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = VtuberDataView.PrepareConnectedAnimation("ArtistForwardConnectedAnimation",
                _artistItem,
                "ArtistAvater");

            Frame.Navigate(typeof(Artist), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void VtuberDataView_Loaded(object sender, RoutedEventArgs e)
        {
            VtuberDataView.ScrollIntoView(_artistItem);
            VtuberDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistBackConnectedAnimation");
            if (animation != null)
            {
                await VtuberDataView.TryStartConnectedAnimationAsync(animation, _artistItem, "ArtistAvater");
            }
        }

        private void PlayListDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _albumItem = ((GridViewItem)PlayListDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = PlayListDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                _albumItem,
                "CoverImgBorder");

            Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void PlayListDataView_Loaded(object sender, RoutedEventArgs e)
        {
            PlayListDataView.ScrollIntoView(_albumItem);
            PlayListDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null)
            {
                await PlayListDataView.TryStartConnectedAnimationAsync(animation, _albumItem, "CoverImgBorder");
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType != typeof(Album) && e.SourcePageType != typeof(Artist))
            {
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
                GC.Collect();
            }
        }
    }
}
