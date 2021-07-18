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

        public Home()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            loadData();
        }

        private async void loadData()
        {
            var bannerData = await App.Client.GetBanner();
            BannerDataView.ItemsSource = bannerData.Data;
            App.ViewModel.SetAppBackgroundImage(new Uri(bannerData.Data.First().BannerImg));

            var newMusicData = await App.Client.GetNewSong(12);
            //SubMusicDataView.ItemsSource = newMusicData.Data;
            NewMusicDataView.ItemsSource = newMusicData.Data;

            var artistData = await App.Client.GetArtistList();
            VtuberDataView.ItemsSource = artistData.Data;

            var albumData = await App.Client.GetPlayListList();
            AlbumDataView.ItemsSource = albumData.Data;
        }

        private void AlbumDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _albumItem = ((GridViewItem)AlbumDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = AlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                _albumItem,
                "AlbumCover");

            Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void AlbumDataView_Loaded(object sender, RoutedEventArgs e)
        {
            AlbumDataView.ScrollIntoView(_albumItem);
            AlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null)
            {
                await AlbumDataView.TryStartConnectedAnimationAsync(animation, _albumItem, "AlbumCover");
            }
        }

        private void VtuberDataView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _artistItem = ((GridViewItem)VtuberDataView.ContainerFromItem(e.ClickedItem)).Content;
            var animation = VtuberDataView.PrepareConnectedAnimation("ArtistForwardConnectedAnimation",
                _artistItem,
                "Avater");

            Frame.Navigate(typeof(Artist), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void VtuberDataView_Loaded(object sender, RoutedEventArgs e)
        {
            VtuberDataView.ScrollIntoView(_artistItem);
            VtuberDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistBackConnectedAnimation");
            if (animation != null)
            {
                await VtuberDataView.TryStartConnectedAnimationAsync(animation, _artistItem, "Avater");
            }
        }
    }
}
