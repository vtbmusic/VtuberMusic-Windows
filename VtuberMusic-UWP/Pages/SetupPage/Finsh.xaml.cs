using System;
using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages.SetupPage {
    /// <summary>
    /// 初始化完成页面
    /// </summary>
    public sealed partial class Finsh : Page, ISetupStep {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE10B" };

        public Finsh() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            this.Title.Text = "欢迎！" + App.Client.Account.Profile.nickname;
            this.Nickname.Text = App.Client.Account.Profile.nickname;
            this.Email.Text = App.Client.Account.Account.userName;
            this.Avatar.ProfilePicture = new BitmapImage(new Uri(App.Client.Account.Profile.avatarUrl));
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            this.Frame.GoBack();
        }

        private void Next_Click(object sender, RoutedEventArgs e) {
            GC.Collect();
            App.RootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }
    }
}
