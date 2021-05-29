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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace VtuberMusic_UWP.Pages
{
    public sealed partial class Album : Page
    {
        public Album()
        {
            this.InitializeComponent();
            CoverImg.Translation = new System.Numerics.Vector3(0, 0, 32);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadData((Models.VtuberMusic.Album)e.Parameter);
        }

        private async void loadData(Models.VtuberMusic.Album album)
        {
            AlbumName.Text = album.name;
            CreatorInfo.Text = $"{ album.creator.nickname } 创建于 { ConvertUnixTimeStamp(album.createTime).ToString("yyyy/M/d") }";
            if (album.description != null)
            {
                Introduction.Text = album.description;
            }
            else
            {
                Introduction.Text = "这个作者很懒没写简介哦～";
            }

            var image = new BitmapImage();
            image.UriSource = new Uri(album.coverImgUrl);
            CoverImg.Source = image;

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if(imageAnimation != null) imageAnimation.TryStart(CoverImg, new UIElement[] { InfoPanel });

            var data = await App.Client.GetPlayListSong(album.id);

            if (data.Success)
            {
                DataView.ItemsSource = data.Data.songs;
            }
        }

        private DateTime ConvertUnixTimeStamp(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", CoverImg);
        }
    }
}
