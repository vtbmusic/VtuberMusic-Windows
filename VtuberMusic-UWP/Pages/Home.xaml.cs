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
            var bannerData = await App.Client.GetBannerListAsync(new ListRequestUpper
            {
                PageIndex = 1,
                PageRows = 100,
                SortType = "desc"
            });

            var hotMusicData = (await App.Client.GetHotMusicListAsync(new ListRequest
            {
                pageIndex = 1,
                pageRows = 10,
                sortField = nameof(MusicData.Likes),
                sortType = "desc"
            })).Data;

            var newMusicData = (await App.Client.GetMusicListAsync(new ListRequest
            {
                pageIndex = 1,
                pageRows = 10,
                sortField = nameof(MusicData.CreateTime),
                sortType = "desc"
            })).Data;

            var vtuberData = (await App.Client.GetVocalListAsync(new ListRequestUpper
            {
                PageIndex = 1,
                PageRows = 30,
                SortField = nameof(Artist.Watchs),
                SortType = "desc"
            })).Data;

            var albumData = (await App.Client.GetAlbumListAsync(new ListRequestUpper
            {
                PageIndex = 1,
                PageRows = 10,
                SortField = nameof(AlbumData.CreateTime),
                SortType = "desc"
            })).Data;

            if (App.Player.NowPlayingMusic == null)
            {
                var image = new BitmapImage();
                image.UriSource = new Uri(bannerData.Data[0].BannerImg);
                App.ViewModel.SetBackgroundImage(image);
            }

            BannerDataView.ItemsSource = bannerData.Data;
            VtuberDataView.ItemsSource = vtuberData;
            AlbumDataView.ItemsSource = albumData;
            HotMusicDataView.ItemsSource = hotMusicData;
            SubMusicDataView.ItemsSource = newMusicData;
            NewMusicDataView.ItemsSource = newMusicData;
        }

        private void AlbumDataView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlbumDataView.SelectedIndex != -1)
            {
                Frame.Navigate(typeof(Album), ((AlbumData)AlbumDataView.SelectedItem).Id);
                AlbumDataView.SelectedIndex = -1;
            }
        }
    }
}
