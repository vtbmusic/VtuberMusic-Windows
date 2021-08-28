using System;
using VtuberMusic_UWP.Components.Dialog;
using VtuberMusic_UWP.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace VtuberMusic_UWP.Pages
{
    public sealed partial class Album : Page
    {
        private ConnectedAnimation imageAnimation = null;
        private Models.VtuberMusic.Album album = null;
        private bool isLkeMusic = false;

        public Album()
        {
            this.InitializeComponent();
            CoverImgBorder.Translation = new System.Numerics.Vector3(0, 0, 32);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Tag = e.Parameter;
            if (e.Parameter.GetType() == typeof(AlbumPageArgs))
            {
                var args = (AlbumPageArgs)e.Parameter;
                album = args.Album;
                isLkeMusic = true;

                loadData(args.IsLikeMusic);
                return;
            }

            album = e.Parameter as Models.VtuberMusic.Album;
            loadData();
        }

        private async void loadData(bool likeMusic = false)
        {
            if (album.creator.userId == App.Client.Account.Account.id && !isLkeMusic) Edit.Visibility = Visibility.Visible;
            AlbumName.Text = album.name;
            CreatorInfo.Text = $"{ album.creator.nickname } 创建于 { UsefullTools.ConvertUnixTimeStamp(album.createTime).ToString("yyyy/M/d") }";
            if (album.description != null)
            {
                Introduction.Text = album.description;
            }
            else
            {
                Introduction.Text = "这个作者很懒没写简介哦～";
            }

            CoverImg.ImageSource = new BitmapImage(new Uri(album.coverImgUrl));

            imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (imageAnimation != null) imageAnimation.TryStart(CoverImgBorder, new UIElement[] { InfoPanel });

            if (likeMusic)
            {
                DataView.ItemsSource = (await App.Client.Account.GetLikeMusicSong()).Data.songs;
            }
            else
            {
                DataView.ItemsSource = (await App.Client.GetPlayListSong(album.id)).Data.songs;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if ((e.SourcePageType == typeof(Home) | e.SourcePageType == typeof(Search)) && imageAnimation != null)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", CoverImgBorder);
            }
        }

        private void PlayAll_Click(object sender, RoutedEventArgs e)
        {
            if (DataView.ItemsSource.Length != 0)
            {
                App.Player.PlayList.Clear();
                foreach (var music in DataView.ItemsSource)
                {
                    App.Player.PlayList.Add(music);
                }

                App.Player.SetMusic(App.Player.PlayList[0]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            DataView.ItemsSource = null;
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            await new EditAlbumInfoDialog().ShowAsync(album.id);
            album = (await App.Client.GetPlayListSong(album.id)).Data.playlist;

            loadData();
        }

        private void Share_Click(object sender, RoutedEventArgs e) =>
            ShareTools.ShareAlbum(album);
    }

    public class AlbumPageArgs
    {
        public Models.VtuberMusic.Album Album { get; set; }
        public bool IsLikeMusic { get; set; }
    }
}
