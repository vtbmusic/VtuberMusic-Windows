using System;
using System.Collections.Generic;
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
    public sealed partial class Album : Page
    {
        public Album()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadData((string)e.Parameter);
        }

        private async void loadData(string id)
        {
            var data = await App.Client.GetPlayListSong(id);

            if (data.Success)
            {
                AlbumName.Text = data.Data.playlist.name;
                CreateTime.Text = data.Data.playlist.createTime.ToString();
                if (data.Data.playlist.description != null)
                {
                    Introduction.Text = data.Data.playlist.description;
                }
                else
                {
                    Introduction.Text = "这个作者很懒没写简介哦～";
                }

                DataView.ItemsSource = data.Data.songs;

                var image = new BitmapImage();
                image.UriSource = new Uri(data.Data.playlist.coverImgUrl);
                CoverImg.Source = image;
            }
        }
    }
}
