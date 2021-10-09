using System;
using VtuberMusic_UWP.Models.Main;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages.SetupPage {
    /// <summary>
    /// 初始化账户页面
    /// </summary>
    public sealed partial class Account : Page, ISetupStep {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE13D" };

        public Account() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            this.Frame.GoBack();
        }

        private async void Next_Click(object sender, RoutedEventArgs e) {
            this.Username.IsEnabled = false;
            this.Password.IsEnabled = false;
            this.Next.IsEnabled = false;
            this.Back.IsEnabled = false;
            this.LoadingBar.Visibility = Visibility.Visible;

            try {
                await App.Client.Account.Login(this.Username.Text, this.Password.Password);
                App.LocalSettings.Username = this.Username.Text;
                App.LocalSettings.Password = this.Password.Password;

                this.Frame.Navigate(typeof(Finsh), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            } catch (Exception ex) {
                this.Username.IsEnabled = true;
                this.Password.IsEnabled = true;
                this.Next.IsEnabled = true;
                this.Back.IsEnabled = true;
                this.LoadingBar.Visibility = Visibility.Collapsed;

                this.ErrorInfoBar.IsOpen = true;
                this.ErrorInfoBar.Message = ex.Message;
            }
        }

        private void Username_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args) {
            if (!string.IsNullOrWhiteSpace(this.Username.Text) && !string.IsNullOrWhiteSpace(this.Password.Password)) {
                this.Next.IsEnabled = true;
            }
        }

        private void Password_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args) {
            if (!string.IsNullOrWhiteSpace(this.Username.Text) && !string.IsNullOrWhiteSpace(this.Password.Password)) {
                this.Next.IsEnabled = true;
            }
        }
    }
}
