using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages
{
    public sealed partial class Home : Page
    {
        private object _albumItem;

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
            SubMusicDataView.ItemsSource = newMusicData.Data;
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

            Frame.Navigate(typeof(Album), e.ClickedItem);
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
    }
}
