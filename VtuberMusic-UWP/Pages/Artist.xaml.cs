using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Artist : Page
    {
        private ConnectedAnimation imageAnimation = null;

        public Artist()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadData((Models.VtuberMusic.Artist)e.Parameter);
        }

        private async void loadData(Models.VtuberMusic.Artist artist)
        {
            ArtistName.Text = artist.name.origin;
            // awsl
            if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn))
            {
                OtherNameAndGroup.Text = artist.name.cn;
            }
            if (artist.name.jp != artist.name.origin && !string.IsNullOrEmpty(artist.name.jp))
            {
                if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn)) OtherNameAndGroup.Text += " / ";
                OtherNameAndGroup.Text += artist.name.jp;
            }

            if (artist.name.en != artist.name.origin && !string.IsNullOrEmpty(artist.name.en))
            {
                if (artist.name.jp != artist.name.origin && !string.IsNullOrEmpty(artist.name.jp))
                {
                    OtherNameAndGroup.Text += " / ";
                }
                else if (artist.name.cn != artist.name.origin && !string.IsNullOrEmpty(artist.name.cn)) OtherNameAndGroup.Text += " / ";

                OtherNameAndGroup.Text += artist.name.en;
            }

            if (OtherNameAndGroup.Text != "") OtherNameAndGroup.Text += " - ";
            OtherNameAndGroup.Text += artist.groupName;

            Avater.ProfilePicture = new BitmapImage(new Uri(artist.imgUrl));

            imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ArtistForwardConnectedAnimation");
            if (imageAnimation != null) imageAnimation.TryStart(Avater, new UIElement[] { InfoPanel });

            MusicCount.Text = artist.musicSize.ToString();
            AlbumCount.Text = artist.albumSize.ToString();
            FanCount.Text = artist.likeSize.ToString();

            DataView.ItemsSource = (await App.Client.GetArtistSong(artist.id, "time", 1000)).Data;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if ((e.SourcePageType == typeof(Home) || (e.SourcePageType == typeof(Search))) && imageAnimation != null)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ArtistBackConnectedAnimation", Avater);
            }
        }
    }
}
