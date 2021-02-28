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
            var data = await App.Client.GetAlbumDataAsync(id);

            if (data.Success)
            {
                AlbumName.Text = data.Data.Name;
                CreateTime.Text = data.Data.CreateTime.ToString();
                if (data.Data.introduce != null)
                {
                    Introduction.Text = data.Data.introduce;
                }
                else
                {
                    Introduction.Text = "这个作者很懒没写简介哦～";
                }

                DataView.ItemsSource = data.Data.Data;

                var image = new BitmapImage();
                image.UriSource = new Uri(data.Data.CoverImg);
                CoverImg.Source = image;
            }
        }
    }
}
