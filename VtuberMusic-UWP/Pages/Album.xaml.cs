using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace VtuberMusic_UWP.Pages
{
    public sealed partial class Album : Page
    {
        private bool turnOffConnectedAnimation = false;

        public Album()
        {
            this.InitializeComponent();
            CoverImg.Translation = new System.Numerics.Vector3(0, 0, 32);
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
            if (imageAnimation == null)
            {
                turnOffConnectedAnimation = true;
            }
            else
            {
                imageAnimation.TryStart(CoverImgBorder, new UIElement[] { InfoPanel });
            }

            if (likeMusic)
            {
                DataView.ItemsSource = (await App.Client.Account.GetLikeMusicSong()).Data.songs;
            }
            else
            {
                DataView.ItemsSource = (await App.Client.GetPlayListSong(album.id)).Data.songs;
            }
        }

        private DateTime ConvertUnixTimeStamp(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (!turnOffConnectedAnimation && e.SourcePageType == typeof(Home) | e.SourcePageType == typeof(Search))
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", CoverImg);
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
