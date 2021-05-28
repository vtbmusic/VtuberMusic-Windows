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
        public Home()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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

        private void AlbumDataView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlbumDataView.SelectedIndex != -1)
            {
                //ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", )
                Frame.Navigate(typeof(Album), AlbumDataView.SelectedItem, new SuppressNavigationTransitionInfo());
            }

            AlbumDataView.SelectedIndex = -1;
        }
    }
}
