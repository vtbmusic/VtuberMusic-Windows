using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
            var newMusicData = await App.Client.GetNewSong(12);

            BannerDataView.ItemsSource = bannerData.Data;
            SubMusicDataView.ItemsSource = newMusicData.Data;
            NewMusicDataView.ItemsSource = newMusicData.Data;
        }

        private void AlbumDataView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
