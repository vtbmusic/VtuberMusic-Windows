using VtuberMusic_UWP.Pages;
using VtuberMusic_UWP.Service;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Account {
    /// <summary>
    /// 账户面板
    /// </summary>
    public sealed partial class AccountPanel : UserControl {
        private AccountService account => App.Client.Account;

        public AccountPanel() {
            this.InitializeComponent();

            this.load();
        }

        private async void load() {
            try {
                var subcount = await App.Client.Account.GetAccountSubCouent();
                this.ArtistSubCount.Text = subcount.Data.artistCount.ToString();
                this.PlaylistCount.Text = subcount.Data.createdPlaylistCount.ToString();
                this.LikeMusicCount.Text = subcount.Data.songCount.ToString();
            } catch { }
        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
            if (App.ViewModel.MainPage.ContentFrame.Content.GetType() != typeof(Profile)) {
                App.ViewModel.MainPage.ContentFrame.Navigate(typeof(Profile), App.Client.Account.Profile, new DrillInNavigationTransitionInfo());
            }
        }
    }
}
