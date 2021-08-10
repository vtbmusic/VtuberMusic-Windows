using VtuberMusic_UWP.Pages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Account
{
    public sealed partial class AccountPanel : UserControl
    {
        public AccountPanel()
        {
            this.InitializeComponent();

            load();
        }

        public async void load()
        {
            Nickname.Text = App.Client.Account.Profile.nickname;
            LevelText.Text = $"Lv.{ App.Client.Account.Profile.level }";
            if (App.Client.Account.Profile.nextexperience.GetValueOrDefault() == 0)
            {
                LevelExpText.Text = $"{ App.Client.Account.Profile.experience.GetValueOrDefault().ToString() } / -";
            }
            else
            {
                LevelExpText.Text = $"{ App.Client.Account.Profile.experience.GetValueOrDefault().ToString() } / { App.Client.Account.Profile.nextexperience.GetValueOrDefault().ToString() }";
            }

            LevelExp.Value = App.Client.Account.Profile.experience.GetValueOrDefault();
            LevelExp.MaxHeight = App.Client.Account.Profile.nextexperience.GetValueOrDefault();

            Avatar.UriSource = new System.Uri(App.Client.Account.Profile.avatarUrl);

            var subcount = await App.Client.Account.GetAccountSubCouent();
            ArtistSubCount.Text = subcount.Data.artistCount.ToString();
            PlaylistCount.Text = subcount.Data.createdPlaylistCount.ToString();
            LikeMusicCount.Text = subcount.Data.songCount.ToString();

        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (App.ViewModel.MainPage.ContentFrame.Content.GetType() != typeof(Profile))
            {
                App.ViewModel.MainPage.ContentFrame.Navigate(typeof(Profile), App.Client.Account.Account.id, new DrillInNavigationTransitionInfo());
            }
        }
    }
}
