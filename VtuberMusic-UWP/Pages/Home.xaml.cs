using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages
{
    public sealed partial class Home : Page
    {
        private object _albumItem;
        private object _artistItem;

        private bool cachePage = false;

        public Home()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            loadData();
        }

        private async void loadData()
        {
            var bannerData = await App.Client.GetBanner();
            BannerPipsPager.NumberOfPages = bannerData.Data.Length;
            BannerDataView.ItemsSource = bannerData.Data;
            var bannerImageUri = new Uri(bannerData.Data.First().BannerImg);

            if (App.ViewModel.BackgroundImageUri != bannerImageUri)
                App.ViewModel.SetAppBackgroundImage(bannerImageUri);

            var newMusicData = await App.Client.GetNewSong(12);
            NewMusicDataView.ItemsSource = newMusicData.Data;

            var artistData = await App.Client.GetArtistList();
            VtuberDataView.ItemsSource = artistData.Data;

            var albumData = await App.Client.GetPlayListList();
            AlbumDataView.ItemsSource = albumData.Data;
        }

        private void AlbumDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            cachePage = true;

            _albumItem = ((GridViewItem)AlbumDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = AlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                _albumItem,
                "CoverImgBorder");

            Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void AlbumDataView_Loaded(object sender, RoutedEventArgs e)
        {
            cachePage = false;

            AlbumDataView.ScrollIntoView(_albumItem);
            AlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null)
            {
                await AlbumDataView.TryStartConnectedAnimationAsync(animation, _albumItem, "CoverImgBorder");
            }
        }

        private void VtuberDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            cachePage = true;

            _artistItem = ((GridViewItem)VtuberDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = VtuberDataView.PrepareConnectedAnimation("ArtistForwardConnectedAnimation",
                _artistItem,
                "Avater");

            Frame.Navigate(typeof(Artist), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void VtuberDataView_Loaded(object sender, RoutedEventArgs e)
        {
            cachePage = false;

            VtuberDataView.ScrollIntoView(_artistItem);
            VtuberDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistBackConnectedAnimation");
            if (animation != null)
            {
                await VtuberDataView.TryStartConnectedAnimationAsync(animation, _artistItem, "Avater");
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!cachePage) this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }
    }
}
