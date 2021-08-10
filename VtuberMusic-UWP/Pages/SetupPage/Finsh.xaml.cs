using System;
using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages.SetupPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Finsh : Page, ISetupStep
    {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE10B" };

        public Finsh()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            Title.Text = "欢迎！" + App.Client.Account.Profile.nickname;
            Nickname.Text = App.Client.Account.Profile.nickname;
            Email.Text = App.Client.Account.Account.userName;
            Avatar.ProfilePicture = new BitmapImage(new Uri(App.Client.Account.Profile.avatarUrl));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            App.RootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }
    }
}
