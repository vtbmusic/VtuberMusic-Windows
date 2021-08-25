using System;
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
                loadData(args.Album, args.IsLikeMusic);
                return;
            }

            loadData((Models.VtuberMusic.Album)e.Parameter);
        }

        private async void loadData(Models.VtuberMusic.Album album, bool likeMusic = false)
        {
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
    }

    public class AlbumPageArgs
    {
        public Models.VtuberMusic.Album Album { get; set; }
        public bool IsLikeMusic { get; set; }
    }
}
